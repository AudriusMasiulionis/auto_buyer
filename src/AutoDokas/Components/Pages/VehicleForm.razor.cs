using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages;

public partial class VehicleForm : ComponentBase
{
    [Inject] private AppDbContext _context { get; set; }
    [Parameter] public Guid ContractId { get; set; }
    [SupplyParameterFromForm] private VehicleContract.Vehicle Model { get; set; } = new();
    private List<VehicleContract.Vehicle.Defect> _selectedDefects = [];
    private bool _submitted = false;
    private bool _loading = false;
    private VehicleContract _contract;
    private VehicleContract.Vehicle.Defect[] _defectValues => Enum.GetValues<VehicleContract.Vehicle.Defect>();

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        _contract = await _context.VehicleContracts.FindAsync(ContractId);
        // handle null
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
            _context.Update(_contract);
            await _context.SaveChangesAsync();
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