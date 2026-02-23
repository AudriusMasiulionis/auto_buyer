using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;

namespace AutoDokas.Components.Shared;

public partial class SignatureField : ComponentBase
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private ILogger<SignatureField> Logger { get; set; } = null!;

    [Parameter] public string Title { get; set; } = "Signature";
    [Parameter] public byte[]? SignatureData { get; set; }
    [Parameter] public EventCallback<byte[]?> SignatureDataChanged { get; set; }
    [Parameter] public bool Disabled { get; set; }

    private ElementReference canvasElement;
    private bool _isEditing = false;
    private bool _isSigning = false;
    private bool _canvasInitialized = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isSigning && !_canvasInitialized)
        {
            await Task.Delay(50);
            try
            {
                await JS.InvokeVoidAsync("signatureInitializeCanvas", canvasElement);
                _canvasInitialized = true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error initializing signature canvas");
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void StartSigning()
    {
        _isSigning = true;
        _canvasInitialized = false;
    }

    private async Task StartEditing()
    {
        _isEditing = true;
        _isSigning = true;
        _canvasInitialized = false;
        StateHasChanged();

        await Task.Delay(100);
        try
        {
            await JS.InvokeVoidAsync("signatureInitializeCanvas", canvasElement);
            _canvasInitialized = true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error initializing signature canvas");
        }
    }

    private async Task ClearCanvas()
    {
        try
        {
            await JS.InvokeVoidAsync("signatureClearCanvas", canvasElement);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error clearing signature canvas");
        }
    }

    private async Task SaveSignature()
    {
        try
        {
            var dataUrl = await JS.InvokeAsync<string>("signatureGetData", canvasElement);
            if (!string.IsNullOrEmpty(dataUrl) && dataUrl.StartsWith("data:image/png;base64,"))
            {
                var base64Data = dataUrl.Substring("data:image/png;base64,".Length);
                var signatureBytes = Convert.FromBase64String(base64Data);

                SignatureData = signatureBytes;
                await SignatureDataChanged.InvokeAsync(signatureBytes);

                _isEditing = false;
                _isSigning = false;
                _canvasInitialized = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error saving signature");
        }
    }
}
