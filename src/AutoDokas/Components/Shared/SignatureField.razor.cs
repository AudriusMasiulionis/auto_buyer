using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;

namespace AutoDokas.Components.Shared;

public partial class SignatureField : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    
    [Parameter] public string Title { get; set; } = "Signature";
    [Parameter] public byte[]? SignatureData { get; set; }
    [Parameter] public EventCallback<byte[]?> SignatureDataChanged { get; set; }
    
    // Canvas-related fields and methods
    private bool showSignatureModal = false;
    private ElementReference canvasElement;
    private bool isDrawing = false;
    private IJSObjectReference? _jsModule;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                // Use the correct path for static resources
                _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/AutoDokas/Components/Shared/SignatureField.razor.js");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JS module: {ex.Message}");
                // Try alternative path
                try
                {
                    _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                        "import", "./Components/Shared/SignatureField.razor.js");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Error loading JS module (alternative path): {ex2.Message}");
                }
            }
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }
    
    private async Task OpenSignatureModal()
    {
        showSignatureModal = true;
        
        // Add class to body to prevent scrolling when modal is open
        await JS.InvokeVoidAsync("document.body.classList.add", "modal-open");
        
        await Task.Delay(100); // Wait for the modal to render
        await _jsModule!.InvokeVoidAsync("initializeCanvas", canvasElement);
    }
    
    private async Task CloseSignatureModal()
    {
        showSignatureModal = false;
        
        // Remove class from body to restore scrolling
        await JS.InvokeVoidAsync("document.body.classList.remove", "modal-open");
    }
    
    private async Task ClearCanvas()
    {
        await _jsModule!.InvokeVoidAsync("clearCanvas", canvasElement);
    }
    
    private async Task SaveSignature()
    {
        var dataUrl = await _jsModule!.InvokeAsync<string>("getSignatureData", canvasElement);
        if (!string.IsNullOrEmpty(dataUrl) && dataUrl.StartsWith("data:image/png;base64,"))
        {
            var base64Data = dataUrl.Substring("data:image/png;base64,".Length);
            var signatureBytes = Convert.FromBase64String(base64Data);
            
            // Update the parameter value and notify parent
            SignatureData = signatureBytes;
            await SignatureDataChanged.InvokeAsync(signatureBytes);
            
            showSignatureModal = false;
        }
    }
    
    private void StartDrawing()
    {
        isDrawing = true;
    }
    
    private async Task Draw(MouseEventArgs e)
    {
        if (isDrawing)
        {
            await _jsModule!.InvokeVoidAsync("drawOnCanvas", canvasElement, e.ClientX, e.ClientY);
        }
    }
    
    private void StopDrawing()
    {
        isDrawing = false;
        _jsModule!.InvokeVoidAsync("endDrawing", canvasElement);
    }
    
    private void StartDrawingTouch(TouchEventArgs e)
    {
        isDrawing = true;
    }
    
    private async Task DrawTouch(TouchEventArgs e)
    {
        if (isDrawing && e.Touches.Length > 0)
        {
            var touch = e.Touches[0];
            await _jsModule!.InvokeVoidAsync("drawOnCanvas", canvasElement, touch.ClientX, touch.ClientY);
        }
    }

    private async Task ClearSignatureData()
    {
        // Clear the signature data
        SignatureData = null;
        await SignatureDataChanged.InvokeAsync(null);
        
        // Update the UI
        StateHasChanged();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
        }
    }
}