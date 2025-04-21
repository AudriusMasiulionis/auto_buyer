using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace AutoDokas.Components.Pages.Contract;

public partial class Vehicle : ComponentBase
{
    [Inject] private AppDbContext _context { get; set; }
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Parameter] public Guid? ContractId { get; set; }
    private EditContext _editContext = null!;

    [SupplyParameterFromForm(FormName = "VehicleForm")]
    private VehicleContract.Vehicle Model { get; set; } = new();

    private List<VehicleContract.Vehicle.Defect> _selectedDefects = [];
    private bool _submitted = false;
    private bool _loading = false;
    private VehicleContract _contract;
    private VehicleContract.Vehicle.Defect[] _defectValues => Enum.GetValues<VehicleContract.Vehicle.Defect>();

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        if (ContractId.HasValue)
        {
            _contract = await _context.VehicleContracts.FindAsync(ContractId);
            if (_contract?.VehicleInfo != null)
            {
                Model = _contract.VehicleInfo;
                _selectedDefects = Model.Defects;
            }
        }

        _editContext = new EditContext(Model);
    }

    private void ToggleSelection(VehicleContract.Vehicle.Defect value, ChangeEventArgs e)
    {
        if ((bool)e.Value == true)
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
            _loading = true;
            _contract.VehicleInfo = Model;
            _contract.VehicleInfo.Defects = _selectedDefects;
            _context.VehicleContracts.Update(_contract);
            await _context.SaveChangesAsync();
            Navigation.NavigateTo($"/Payment/{ContractId.Value}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _loading = false;
        }
    }
}