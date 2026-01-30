// signature.js - Functions for handling the signature canvas

const canvasState = new WeakMap();

// Initialize the canvas and get its context
function initializeCanvas(canvas) {
    if (!canvas) return;

    // Set the canvas dimensions to match its CSS display size
    const rect = canvas.getBoundingClientRect();
    canvas.width = rect.width;
    canvas.height = rect.height;

    const context = canvas.getContext('2d');
    if (!context) return;

    // Set canvas styles for drawing
    context.lineWidth = 2;
    context.lineCap = 'round';
    context.lineJoin = 'round';
    context.strokeStyle = '#000000';

    // Store state for this canvas
    const state = {
        context: context,
        isDrawing: false,
        lastX: 0,
        lastY: 0
    };
    canvasState.set(canvas, state);

    // Remove old listeners if any (use stored references)
    if (canvas._mouseDownHandler) {
        canvas.removeEventListener('mousedown', canvas._mouseDownHandler);
        canvas.removeEventListener('mousemove', canvas._mouseMoveHandler);
        canvas.removeEventListener('mouseup', canvas._mouseUpHandler);
        canvas.removeEventListener('mouseleave', canvas._mouseUpHandler);
        canvas.removeEventListener('touchstart', canvas._touchStartHandler);
        canvas.removeEventListener('touchmove', canvas._touchMoveHandler);
        canvas.removeEventListener('touchend', canvas._touchEndHandler);
    }

    // Create bound handlers for this canvas
    canvas._mouseDownHandler = handleMouseDown;
    canvas._mouseMoveHandler = handleMouseMove;
    canvas._mouseUpHandler = handleMouseUp;
    canvas._touchStartHandler = handleTouchStart;
    canvas._touchMoveHandler = handleTouchMove;
    canvas._touchEndHandler = handleTouchEnd;

    // Add native event listeners
    canvas.addEventListener('mousedown', canvas._mouseDownHandler);
    canvas.addEventListener('mousemove', canvas._mouseMoveHandler);
    canvas.addEventListener('mouseup', canvas._mouseUpHandler);
    canvas.addEventListener('mouseleave', canvas._mouseUpHandler);
    canvas.addEventListener('touchstart', canvas._touchStartHandler, { passive: false });
    canvas.addEventListener('touchmove', canvas._touchMoveHandler, { passive: false });
    canvas.addEventListener('touchend', canvas._touchEndHandler);

    // Clear any previous drawings
    clearCanvas(canvas);
}

function getCanvasCoordinates(canvas, clientX, clientY) {
    const rect = canvas.getBoundingClientRect();
    return {
        x: clientX - rect.left,
        y: clientY - rect.top
    };
}

function handleMouseDown(e) {
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state) return;

    state.isDrawing = true;
    const coords = getCanvasCoordinates(canvas, e.clientX, e.clientY);
    state.lastX = coords.x;
    state.lastY = coords.y;
    state.context.beginPath();
    state.context.moveTo(coords.x, coords.y);
}

function handleMouseMove(e) {
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state || !state.isDrawing) return;

    const coords = getCanvasCoordinates(canvas, e.clientX, e.clientY);

    state.context.beginPath();
    state.context.moveTo(state.lastX, state.lastY);
    state.context.lineTo(coords.x, coords.y);
    state.context.stroke();

    state.lastX = coords.x;
    state.lastY = coords.y;
}

function handleMouseUp(e) {
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state) return;

    state.isDrawing = false;
}

function handleTouchStart(e) {
    e.preventDefault(); // Prevent scrolling
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state || e.touches.length === 0) return;

    state.isDrawing = true;
    const touch = e.touches[0];
    const coords = getCanvasCoordinates(canvas, touch.clientX, touch.clientY);
    state.lastX = coords.x;
    state.lastY = coords.y;
    state.context.beginPath();
    state.context.moveTo(coords.x, coords.y);
}

function handleTouchMove(e) {
    e.preventDefault(); // Prevent scrolling
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state || !state.isDrawing || e.touches.length === 0) return;

    const touch = e.touches[0];
    const coords = getCanvasCoordinates(canvas, touch.clientX, touch.clientY);

    state.context.beginPath();
    state.context.moveTo(state.lastX, state.lastY);
    state.context.lineTo(coords.x, coords.y);
    state.context.stroke();

    state.lastX = coords.x;
    state.lastY = coords.y;
}

function handleTouchEnd(e) {
    const canvas = e.currentTarget;
    const state = canvasState.get(canvas);
    if (!state) return;

    state.isDrawing = false;
}

// Clear the canvas
function clearCanvas(canvas) {
    const state = canvasState.get(canvas);
    if (!canvas || !state) return;

    state.context.clearRect(0, 0, canvas.width, canvas.height);
    state.context.beginPath();
}

// Get the signature data as a base64 encoded PNG
function getSignatureData(canvas) {
    if (!canvas) return '';

    try {
        return canvas.toDataURL('image/png');
    } catch (err) {
        console.error('Error getting signature data:', err);
        return '';
    }
}

// Export functions to make them available for .NET
window.signatureInitializeCanvas = initializeCanvas;
window.signatureClearCanvas = clearCanvas;
window.signatureGetData = getSignatureData;
