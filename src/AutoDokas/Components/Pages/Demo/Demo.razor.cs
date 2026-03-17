using AutoDokas.Components.Pages.Contract;
using AutoDokas.Components.Pages.Contract.Sections;
using AutoDokas.Data.Models;
using AutoDokas.Resources;
using AutoDokas.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDokas.Components.Pages.Demo;

public partial class Demo : ComponentBase, IDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private ICsvReader CsvReader { get; set; } = null!;
    [Inject] private IWebHostEnvironment Env { get; set; } = null!;

    private ContractViewModel Model { get; set; } = new();
    private List<Country> Countries { get; set; } = [];

    private SectionState _vehicleState = SectionState.ReadOnly;
    private SectionState _paymentState = SectionState.ReadOnly;
    private SectionState _sellerState = SectionState.ReadOnly;
    private SectionState _buyerMethodState = SectionState.ReadOnly;
    private SectionState _buyerInfoState = SectionState.ReadOnly;

    private DotNetObjectReference<Demo>? _dotnetRef;

    protected override async Task OnInitializedAsync()
    {
        Countries = [.. await CsvReader.ReadAllAsync<Country>("countries.csv")];
        InitializeDemoData();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotnetRef = DotNetObjectReference.Create(this);
            var steps = new[]
            {
                new { selector = "#demo-step-intro", title = Text.DemoTitle, description = Text.DemoSubtitle },
                new { selector = "#demo-step-vehicle", title = Text.DemoStep1Title, description = Text.DemoStep1Desc },
                new { selector = "#demo-step-payment", title = Text.DemoStep2Title, description = Text.DemoStep2Desc },
                new { selector = "#demo-step-seller", title = Text.DemoStep3Title, description = Text.DemoStep3Desc },
                new { selector = "#demo-step-buyer-method", title = Text.DemoStep4Title, description = Text.DemoStep4Desc },
                new { selector = "#demo-step-buyer-info", title = Text.DemoStep5Title, description = Text.DemoStep5Desc },
                new { selector = "#demo-step-download", title = Text.DemoDownloadTitle, description = Text.DemoDownloadDesc }
            };
            await Task.Delay(500);
            await JS.InvokeVoidAsync("demoTour.start", _dotnetRef, steps);
        }
    }

    [JSInvokable]
    public void OnTourComplete()
    {
    }

    private void InitializeDemoData()
    {
        var lithuania = Countries.FirstOrDefault(c => c.Name == "Lietuva");

        // Load signature images from wwwroot
        var sellerSigPath = Path.Combine(Env.WebRootPath, "img", "sig-seller.png");
        var buyerSigPath = Path.Combine(Env.WebRootPath, "img", "sig-buyer.png");
        var sellerSig = File.Exists(sellerSigPath) ? File.ReadAllBytes(sellerSigPath) : null;
        var buyerSig = File.Exists(buyerSigPath) ? File.ReadAllBytes(buyerSigPath) : null;

        Model = new ContractViewModel
        {
            // Vehicle
            VehicleSdk = "Volkswagen",
            VehicleMake = "Golf VII 1.6 TDI",
            VehicleRegistrationNumber = "AAA 000",
            VehicleRegistrationCertificate = "AA 0000000",
            VehicleMillage = 100000,
            VehicleIdentificationNumber = "WVWZZZ0KZAA000000",
            VehicleIsInspected = true,
            VehicleHasBeenDamaged = false,
            VehicleDamagedDuringOwnership = false,
            VehicleDamageIncidentsKnown = false,
            VehicleDefects = [],

            // Payment
            PaymentPrice = 5000m,
            PaymentMethod = VehicleContract.Payment.PaymentType.BankTransfer,
            PaymentAtContractFormation = true,
            TransferInsurance = false,

            // Seller
            SellerName = "Vardenis Pavardenis",
            SellerCode = "30000000001",
            SellerPhone = "+37000000001",
            SellerAddress = "Pavyzdinė g. 1, Vilnius",
            SellerEmail = "vardenis@pavyzdys.lt",
            Origin = lithuania,
            HasConsented = true,
            SignatureData = sellerSig,

            // Buyer
            BuyerName = "Pirkenis Pavardenis",
            BuyerCode = "30000000002",
            BuyerPhone = "+37000000002",
            BuyerAddress = "Pavyzdinė g. 2, Kaunas",
            BuyerEmail = "pirkenis@pavyzdys.lt",
            BuyerSignatureData = buyerSig
        };
    }

    public void Dispose()
    {
        _dotnetRef?.Dispose();
    }
}
