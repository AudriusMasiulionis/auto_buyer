using AutoDokas.Components.Pages.Contract.ViewModels;
using AutoDokas.Components.Shared;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Vehicle : FormComponentBase<VehicleViewModel>
{
    [Inject] private AppDbContext _context { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Parameter] public Guid? ContractId { get; set; }

    [SupplyParameterFromForm(FormName = "VehicleForm")]
    protected override VehicleViewModel Model { get => base.Model; set => base.Model = value; }

    private List<VehicleContract.Vehicle.Defect> _selectedDefects = [];
    private VehicleContract? _contract;
    private VehicleContract.Vehicle.Defect[] _defectValues => Enum.GetValues<VehicleContract.Vehicle.Defect>();

    protected override async Task OnInitializedAsync()
    {
        Model = new VehicleViewModel();

        // Loading = true;
        if (ContractId.HasValue)
        {
            _contract = await _context.VehicleContracts.FindAsync(ContractId);
            if (_contract?.VehicleInfo != null)
            {
                Model = VehicleViewModel.FromEntity(_contract.VehicleInfo);
                _selectedDefects = Model.Defects;
            }
        }

        InitializeEditContext();
    }

    private void ToggleSelection(VehicleContract.Vehicle.Defect value, ChangeEventArgs e)
    {
        if (e.Value is bool isChecked && isChecked)
        {
            if (!_selectedDefects.Contains(value))
            {
                _selectedDefects.Add(value);
            }
        }
        else
        {
            _selectedDefects.Remove(value);
        }
    }

    private async Task Submit()
    {
        try
        {
            Model.Defects = _selectedDefects;

            if (_contract != null && ContractId.HasValue)
            {
                // Convert ViewModel to Entity
                var vehicleEntity = Model.ToEntity();
                _contract.VehicleInfo = vehicleEntity;

                _context.VehicleContracts.Update(_contract);
                await _context.SaveChangesAsync();
                Navigation.NavigateTo($"/Payment/{ContractId.Value}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Loading = false;
        }
    }
}