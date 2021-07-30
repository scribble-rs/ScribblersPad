using ScribblersSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a drawing too input controller script
    /// </summary>
    [RequireComponent(typeof(DrawingBoardControllerScript))]
    public class DrawingToolInputControllerScript : AScribblersClientControllerScript, IDrawingToolInputController
    {
        /// <summary>
        /// Last pointer position lookup
        /// </summary>
        private readonly Dictionary<int, Vector2> lastPointerPositionLookup = new Dictionary<int, Vector2>();

        /// <summary>
        /// Drawing tool
        /// </summary>
        [SerializeField]
        private EDrawingTool drawingTool = EDrawingTool.Pen;

        /// <summary>
        /// Brush size
        /// </summary>
        [SerializeField]
        private float brushSize = 16.0f;

        /// <summary>
        /// Drawing tool color
        /// </summary>
        [SerializeField]
        private Color32 drawingToolColor = new Color32(0x00, 0x00, 0x00, 0xFF);

        /// <summary>
        /// Minimal brush size
        /// </summary>
        private uint minimalBrushSize = 8U;

        /// <summary>
        /// Maximal brush size
        /// </summary>
        private uint maximalBrushSize = 32U;

        /// <summary>
        /// Touch screen keyboard
        /// </summary>
        private TouchScreenKeyboard touchScreenKeyboard;

        /// <summary>
        /// Drawing tool
        /// </summary>
        public EDrawingTool DrawingTool
        {
            get => drawingTool;
            set => drawingTool = value;
        }

        /// <summary>
        /// Brush size
        /// </summary>
        public float BrushSize
        {
            get => brushSize;
            set => brushSize = Mathf.Clamp(value, minimalBrushSize, maximalBrushSize);
        }

        /// <summary>
        /// Drawing tool color
        /// </summary>
        public Color32 DrawingToolColor
        {
            get => drawingToolColor;
            set => drawingToolColor = value;
        }

        /// <summary>
        /// Canvas color
        /// </summary>
        public Color32 CanvasColor { get; private set; } = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

        /// <summary>
        /// Drawing board controller
        /// </summary>
        public DrawingBoardControllerScript DrawingBoardController { get; private set; }

        /// <summary>
        /// Gets the brush position from screen position
        /// </summary>
        /// <param name="screenPosition">Screen position</param>
        /// <returns>Brush point in texture coordinates</returns>
        private Vector2 GetBrushPositionFromScreenPosition(Vector2 screenPosition)
        {
            Vector2 ret = Vector2.zero;
            if (DrawingBoardController && DrawingBoardController.RectangleTransform && DrawingBoardController.ScribblersClientManager && (DrawingBoardController.ScribblersClientManager.Lobby != null))
            {
                RectTransform rectangle_transform = DrawingBoardController.RectangleTransform;
                Rect rectangle = rectangle_transform.rect;
                ILobby lobby = DrawingBoardController.ScribblersClientManager.Lobby;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle_transform, screenPosition, null, out Vector2 local_point))
                {
                    ret = new Vector2((local_point.x - rectangle.position.x) * lobby.DrawingBoardBaseWidth / rectangle.size.x, (rectangle.size.y - (local_point.y - rectangle.position.y)) * lobby.DrawingBoardBaseHeight / rectangle.size.y);
                }
            }
            return ret;
        }

        /// <summary>
        /// Handles dragging
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        private void HandleDragging(PointerEventData eventData)
        {
            if (lastPointerPositionLookup.ContainsKey(eventData.pointerId))
            {
                Vector2 pointer_position = GetBrushPositionFromScreenPosition(eventData.position);
                Vector2 last_pointer_position = lastPointerPositionLookup[eventData.pointerId];
                if (DrawingBoardController && (drawingTool != EDrawingTool.FloodFill))
                {
                    Color32 color = (drawingTool == EDrawingTool.Pen) ? drawingToolColor : CanvasColor;
                    DrawingBoardController.DrawLine(last_pointer_position, pointer_position, new ScribblersSharp.Color(color.r, color.g, color.b), brushSize);
                }
                lastPointerPositionLookup[eventData.pointerId] = pointer_position;
            }
        }

        /// <summary>
        /// Activates touch screen keyboard
        /// </summary>
        private void ActivateTouchScreenKeyboard() => touchScreenKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false, false, false, string.Empty);

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            // ...
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            // ...
        }

        /// <summary>
        /// Selects pen
        /// </summary>
        public void SelectPen() => drawingTool = EDrawingTool.Pen;

        /// <summary>
        /// Selects flood fill
        /// </summary>
        public void SelectFloodFill() => drawingTool = EDrawingTool.FloodFill;

        /// <summary>
        /// Selects eraser
        /// </summary>
        public void SelectEraser() => drawingTool = EDrawingTool.Eraser;

        /// <summary>
        /// Gets invoked when pointer is pressing down
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!lastPointerPositionLookup.ContainsKey(eventData.pointerId))
            {
                Vector2 pointer_position = GetBrushPositionFromScreenPosition(eventData.position);
                lastPointerPositionLookup.Add(eventData.pointerId, pointer_position);
                if (DrawingBoardController)
                {
                    switch (drawingTool)
                    {
                        case EDrawingTool.Pen:
                            DrawingBoardController.DrawLine(pointer_position, pointer_position, new ScribblersSharp.Color(drawingToolColor.r, drawingToolColor.g, drawingToolColor.b), brushSize);
                            break;
                        case EDrawingTool.FloodFill:
                            DrawingBoardController.Fill(pointer_position, new ScribblersSharp.Color(drawingToolColor.r, drawingToolColor.g, drawingToolColor.b));
                            break;
                        case EDrawingTool.Eraser:
                            DrawingBoardController.DrawLine(pointer_position, pointer_position, new ScribblersSharp.Color(CanvasColor.r, CanvasColor.g, CanvasColor.b), brushSize);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets invoked when pointer pressing has been finished
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (lastPointerPositionLookup.ContainsKey(eventData.pointerId))
            {
                Vector2 last_pointer_position = lastPointerPositionLookup[eventData.pointerId];
                if (DrawingBoardController)
                {
                    if (ScribblersClientManager && (ScribblersClientManager.Lobby != null) && ScribblersClientManager.Lobby.IsPlayerAllowedToDraw)
                    {
                        if (drawingTool != EDrawingTool.FloodFill)
                        {
                            Color32 color = (drawingTool == EDrawingTool.Pen) ? drawingToolColor : CanvasColor;
                            DrawingBoardController.DrawLine(last_pointer_position, GetBrushPositionFromScreenPosition(eventData.position), new ScribblersSharp.Color(color.r, color.g, color.b), brushSize);
                        }
                    }
                    else
                    {
                        ActivateTouchScreenKeyboard();
                    }
                }
                lastPointerPositionLookup.Remove(eventData.pointerId);
            }
        }

        /// <summary>
        /// Sets selection value without notifying to event listeners
        /// </summary>
        /// <param name="selectionValue">Selection value</param>
        public void OnBeginDrag(PointerEventData eventData) => HandleDragging(eventData);

        /// <summary>
        /// Gets invoked when pointer is dragging
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnDrag(PointerEventData eventData) => HandleDragging(eventData);

        /// <summary>
        /// Gets invoked when pointer drag has ended
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnEndDrag(PointerEventData eventData) => HandleDragging(eventData);

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate() => brushSize = Mathf.Clamp(brushSize, 10.0f, 40.0f);

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out DrawingBoardControllerScript drawing_board_controller))
            {
                DrawingBoardController = drawing_board_controller;
            }
            else
            {
                Debug.LogError($"Component \"{ typeof(DrawingBoardControllerScript) }\" is missing.", this);
            }
            if (ScribblersClientManager && (ScribblersClientManager.Lobby != null))
            {
                ILobbyLimits lobby_limits = ScribblersClientManager.Lobby.Limits;
                minimalBrushSize = lobby_limits.MinimalBrushSize;
                maximalBrushSize = lobby_limits.MaximalBrushSize;
                brushSize = Mathf.Clamp(brushSize, minimalBrushSize, maximalBrushSize);
                CanvasColor = new Color32(ScribblersClientManager.Lobby.CanvasColor.Red, ScribblersClientManager.Lobby.CanvasColor.Green, ScribblersClientManager.Lobby.CanvasColor.Blue, 0xFF);
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if ((touchScreenKeyboard != null) && (touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done))
            {
                ScribblersClientManager.SendMessageGameMessage(touchScreenKeyboard.text);
                touchScreenKeyboard = null;
            }
        }
    }
}
