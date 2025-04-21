// SignatureField.razor.js - Functions for handling the signature canvas

let isDrawing = false;
let lastX = 0;
let lastY = 0;
let context = null;
let canvasRect = null;

// Initialize the canvas and get its context
export function initializeCanvas(canvas) {
    if (!canvas) return;
    
    // Set the canvas dimensions to match its CSS display size
    const rect = canvas.getBoundingClientRect();
    canvas.width = rect.width;
    canvas.height = rect.height;
    
    context = canvas.getContext('2d');
    if (!context) return;
    
    // Set canvas styles for drawing
    context.lineWidth = 2;
    context.lineCap = 'round';
    context.lineJoin = 'round';
    context.strokeStyle = '#000000';
    
    // Get canvas position for accurate drawing
    canvasRect = canvas.getBoundingClientRect();
    
    // Clear any previous drawings
    clearCanvas(canvas);
}

// Clear the canvas
export function clearCanvas(canvas) {
    if (!canvas || !context) return;
    
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.beginPath();
}

// Draw on the canvas at a specific position
export function drawOnCanvas(canvas, clientX, clientY) {
    if (!canvas || !context || !canvasRect) return;
    
    // Calculate position relative to canvas
    const x = clientX - canvasRect.left;
    const y = clientY - canvasRect.top;
    
    if (!isDrawing) {
        // Start a new line
        context.beginPath();
        context.moveTo(x, y);
        isDrawing = true;
        lastX = x;
        lastY = y;
        return;
    }
    
    // Continue the line
    context.beginPath();
    context.moveTo(lastX, lastY);
    context.lineTo(x, y);
    context.stroke();
    
    lastX = x;
    lastY = y;
}

// End the drawing
export function endDrawing() {
    isDrawing = false;
}

// Get the signature data as a base64 encoded PNG
export function getSignatureData(canvas) {
    if (!canvas) return '';
    
    try {
        // Convert canvas to data URL
        return canvas.toDataURL('image/png');
    } catch (err) {
        console.error('Error getting signature data:', err);
        return '';
    }
}