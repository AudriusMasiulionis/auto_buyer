using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages;

public partial class SellerForm : ComponentBase
{
    [Inject] private AppDbContext _context { get; set; }
    [Inject] private NavigationManager _navigation { get; set; }
    [SupplyParameterFromForm] private VehicleContract.PartyInfo Model { get; set; } = new();
    bool _submitted = false;
    bool _loading = false;

    protected override void OnInitialized()
    {
        Model ??= new();
    }
    

    private async Task Submit()
    {
        try
        {
            _loading = true;
            var entity = new VehicleContract
            {
                SellerInfo = Model
            };
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            _navigation.NavigateTo($"/VehicleForm/{entity.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _loading = false;
        }
    }
}