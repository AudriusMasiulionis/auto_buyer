using AutoDokas.Components.Pages.Contract.Sections;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using AutoDokas.Resources;
using AutoDokas.Services;

using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages.Contract;

public partial class Contract : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IEmailService EmailService { get; set; } = null!;

    [Inject] private ICsvReader CsvReader { get; set; } = null!;

    [Parameter] public Guid? ContractId { get; set; }

    private VehicleContract contract = new();
    private bool _loading = false;
    private bool _isBuyerMode;
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

                if (_isBuyerMode)
                {
                    _vehicleState = SectionState.Completed;
                    _paymentState = SectionState.Completed;
                    _sellerState = SectionState.Completed;
                    _buyerMethodState = SectionState.Completed;
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
        _vehicleState = MapStatus(contract.VehicleStatus);
        _paymentState = MapStatus(contract.PaymentStatus);
        _sellerState = MapStatus(contract.SellerStatus);
        _buyerMethodState = MapStatus(contract.BuyerMethodStatus);
        _buyerInfoState = MapStatus(contract.BuyerInfoStatus);
        _currentSection = DetermineCurrentSection();
    }

    private static SectionState MapStatus(VehicleContract.FormSectionStatus status) => status switch
    {
        VehicleContract.FormSectionStatus.Completed => SectionState.Completed,
        VehicleContract.FormSectionStatus.InProgress => SectionState.Active,
        _ => SectionState.Disabled
    };

    private FormSection DetermineCurrentSection()
    {
        if (contract.VehicleStatus != VehicleContract.FormSectionStatus.Completed) return FormSection.Vehicle;
        if (contract.PaymentStatus != VehicleContract.FormSectionStatus.Completed) return FormSection.Payment;
        if (contract.SellerStatus != VehicleContract.FormSectionStatus.Completed) return FormSection.Seller;
        if (contract.BuyerMethodStatus != VehicleContract.FormSectionStatus.Completed) return FormSection.BuyerMethod;
        return FormSection.BuyerInfo;
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
            UpdateSectionStatus(FormSection.Vehicle, VehicleContract.FormSectionStatus.Completed);

            if (contract.Id == Guid.Empty)
            {
                UpdateSectionStatus(FormSection.Payment, VehicleContract.FormSectionStatus.InProgress);
                await Context.AddAsync(contract);
                await Context.SaveChangesAsync();

                ContractId = contract.Id;
                Navigation.NavigateTo($"/contract/{contract.Id}", replace: true);
            }
            else
            {
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
            Console.WriteLine($"Error saving vehicle data: {ex.Message}");
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
                UpdateSectionStatus(FormSection.BuyerMethod, VehicleContract.FormSectionStatus.Completed);
                await SaveProgress();

                var buyerLink = $"{Navigation.BaseUri}buyer/{contract.Id}";
                var emailBody = $"You have been invited to sign a vehicle purchase contract.\n\nPlease open the following link to fill in your information:\n{buyerLink}";
                await EmailService.SendEmailAsync("noreply@autodokas.lt", Model.BuyerEmail, "Vehicle Contract - Buyer Information Required", emailBody);

                Navigation.NavigateTo("/BuyerNotificationSent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending buyer notification: {ex.Message}");
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
            UpdateSectionStatus(FormSection.BuyerInfo, VehicleContract.FormSectionStatus.Completed);
            await SaveProgress();
            Navigation.NavigateTo("/ContractCompleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving buyer info: {ex.Message}");
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
            // Mark current section as completed
            UpdateSectionStatus(_currentSection, VehicleContract.FormSectionStatus.Completed);
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
                UpdateSectionStatus(_currentSection + 1, VehicleContract.FormSectionStatus.InProgress);
                await SaveProgress();
                _currentSection++;
                SetSectionState(_currentSection, SectionState.Active);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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

    private void UpdateSectionStatus(FormSection section, VehicleContract.FormSectionStatus status)
    {
        switch (section)
        {
            case FormSection.Vehicle:
                contract.VehicleStatus = status;
                break;
            case FormSection.Payment:
                contract.PaymentStatus = status;
                break;
            case FormSection.Seller:
                contract.SellerStatus = status;
                break;
            case FormSection.BuyerMethod:
                contract.BuyerMethodStatus = status;
                break;
            case FormSection.BuyerInfo:
                contract.BuyerInfoStatus = status;
                break;
        }
    }

    private string FormatBoolean(bool value) => value ? Text.Yes : Text.No;

    private string FormatInspection(bool value) => value ? Text.Valid : Text.Invalid;
}
