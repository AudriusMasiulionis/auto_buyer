using AutoDokas.Components.Pages.Contract.Sections;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using AutoDokas.Resources;
using AutoDokas.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace AutoDokas.Components.Pages.Contract;

public partial class Contract : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private EmailNotificationService EmailNotificationService { get; set; } = null!;
    [Inject] private ILogger<Contract> Logger { get; set; } = null!;

    [Inject] private ICsvReader CsvReader { get; set; } = null!;

    [Parameter] public Guid? ContractId { get; set; }

    private VehicleContract contract = new();
    private bool _loading = false;
    private bool _isBuyerMode;
    private bool _isContractCompleted;
    private FormSection _currentSection = FormSection.Vehicle;
    private FormSection? _returnToSection;
    private SectionState _vehicleState = SectionState.Active;
    private SectionState _sellerState = SectionState.Disabled;
    private SectionState _paymentState = SectionState.Disabled;
    private SectionState _buyerMethodState = SectionState.Disabled;
    private SectionState _buyerInfoState = SectionState.Disabled;

    private List<Country> Countries { get; set; } = [];

    [SupplyParameterFromForm(FormName = "Form")]
    private ContractViewModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _isBuyerMode = Navigation.Uri.Contains("/buyer/", StringComparison.OrdinalIgnoreCase);

        // Load countries
        Countries = [.. await CsvReader.ReadAllAsync<Country>("countries.csv")];

        if (ContractId.HasValue)
        {
            var foundContract = await Context.VehicleContracts.FindAsync(ContractId);
            if (foundContract != null)
            {
                contract = foundContract;

                if (contract.Status == VehicleContract.ContractStatus.Completed)
                {
                    _isContractCompleted = true;
                    _vehicleState = SectionState.ReadOnly;
                    _paymentState = SectionState.ReadOnly;
                    _sellerState = SectionState.ReadOnly;
                    _buyerMethodState = SectionState.ReadOnly;
                    _buyerInfoState = SectionState.ReadOnly;
                }
                else if (_isBuyerMode)
                {
                    _vehicleState = SectionState.ReadOnly;
                    _paymentState = SectionState.ReadOnly;
                    _sellerState = SectionState.ReadOnly;
                    _buyerMethodState = SectionState.ReadOnly;
                    _buyerInfoState = SectionState.Active;
                    _currentSection = FormSection.BuyerInfo;
                }
                else
                {
                    RestoreSectionStates();
                }
            }
            else
            {
                _currentSection = FormSection.Vehicle;
                contract = new VehicleContract();
            }
        }
        else
        {
            _currentSection = FormSection.Vehicle;
            contract = new VehicleContract();
        }

        InitializeModel();
    }

    private void RestoreSectionStates()
    {
        var status = (int)contract.Status;
        _vehicleState = SectionStateFor(FormSection.Vehicle, status);
        _paymentState = SectionStateFor(FormSection.Payment, status);
        _sellerState = SectionStateFor(FormSection.Seller, status);
        _buyerMethodState = SectionStateFor(FormSection.BuyerMethod, status);
        _buyerInfoState = SectionStateFor(FormSection.BuyerInfo, status);
        _currentSection = (FormSection)(status + 1);
    }

    private static SectionState SectionStateFor(FormSection section, int status)
    {
        var sectionIndex = (int)section - 1;
        if (sectionIndex < status) return SectionState.Completed;
        if (sectionIndex == status) return SectionState.Active;
        return SectionState.Disabled;
    }

    private void InitializeModel()
    {
        Model = ContractViewModel.FromContract(contract);

        // Set default country to Lithuania for new contracts
        if (Model.Origin == null)
        {
            var lithuania = Countries.FirstOrDefault(c => c.Name == "Lietuva");
            if (lithuania != null)
            {
                Model.Origin = lithuania;
            }
        }
    }

    private void EditSection(FormSection section)
    {
        if (_isContractCompleted) return;
        if (_currentSection > section)
        {
            _returnToSection = _currentSection;
            _currentSection = section;
            SetSectionState(section, SectionState.Active);
        }
    }

    private async Task HandleSellerNext()
    {
        await MoveToNextSection();
    }

    private async Task HandleVehicleNext()
    {
        try
        {
            Model.ApplyToContract(contract);

            if (contract.Id == Guid.Empty)
            {
                contract.Status = VehicleContract.ContractStatus.PaymentEntry;
                await Context.AddAsync(contract);
                await Context.SaveChangesAsync();

                ContractId = contract.Id;
                Navigation.NavigateTo($"/contract/{contract.Id}", replace: true);
            }
            else
            {
                if (!_returnToSection.HasValue)
                    contract.Status = VehicleContract.ContractStatus.PaymentEntry;
                await SaveProgress();
            }

            _vehicleState = SectionState.Completed;

            if (_returnToSection.HasValue)
            {
                _currentSection = _returnToSection.Value;
                _returnToSection = null;
            }
            else
            {
                _paymentState = SectionState.Active;
                _currentSection = FormSection.Payment;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error saving vehicle data");
        }
    }

    private async Task HandlePaymentNext()
    {
        await MoveToNextSection();
    }

    private async Task HandleBuyerMethodNext(BuyerMethodSection.BuyerSigningMethod method)
    {
        if (method == BuyerMethodSection.BuyerSigningMethod.SendNotification)
        {
            try
            {
                _loading = true;
                contract.Status = VehicleContract.ContractStatus.BuyerInfoEntry;
                await SaveProgress();

                await EmailNotificationService.SendBuyerInviteAsync(Model.BuyerEmail, contract);

                Navigation.NavigateTo("/BuyerNotificationSent");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error sending buyer notification");
            }
            finally
            {
                _loading = false;
            }
            return;
        }

        await MoveToNextSection();
    }

    private async Task HandleBuyerInfoNext()
    {
        try
        {
            _loading = true;
            contract.Status = VehicleContract.ContractStatus.Completed;
            await SaveProgress();

            if (!string.IsNullOrWhiteSpace(contract.SellerInfo?.Email))
                await EmailNotificationService.SendContractNotificationAsync(contract.SellerInfo.Email, contract);
            if (!string.IsNullOrWhiteSpace(contract.BuyerInfo?.Email))
                await EmailNotificationService.SendContractNotificationAsync(contract.BuyerInfo.Email, contract);

            Navigation.NavigateTo("/ContractCompleted");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error saving buyer info");
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task MoveToNextSection()
    {
        try
        {
            SetSectionState(_currentSection, SectionState.Completed);

            if (_returnToSection.HasValue)
            {
                // Editing a previous section — save and return to where the user was
                await SaveProgress();
                _currentSection = _returnToSection.Value;
                _returnToSection = null;
            }
            else if (_currentSection < FormSection.BuyerInfo)
            {
                // Normal forward flow
                contract.Status = (VehicleContract.ContractStatus)((int)_currentSection);
                await SaveProgress();
                _currentSection++;
                SetSectionState(_currentSection, SectionState.Active);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error moving to next section");
        }
        finally
        {
            _loading = false;
        }
    }

    private void SetSectionState(FormSection section, SectionState state)
    {
        switch (section)
        {
            case FormSection.Vehicle:
                _vehicleState = state;
                break;
            case FormSection.Payment:
                _paymentState = state;
                break;
            case FormSection.Seller:
                _sellerState = state;
                break;
            case FormSection.BuyerMethod:
                _buyerMethodState = state;
                break;
            case FormSection.BuyerInfo:
                _buyerInfoState = state;
                break;
        }
    }

    private async Task SaveProgress()
    {
        Model.ApplyToContract(contract);
        Context.VehicleContracts.Update(contract);
        await Context.SaveChangesAsync();
    }

    private string FormatBoolean(bool value) => value ? Text.Yes : Text.No;

    private string FormatInspection(bool value) => value ? Text.Valid : Text.Invalid;
}
