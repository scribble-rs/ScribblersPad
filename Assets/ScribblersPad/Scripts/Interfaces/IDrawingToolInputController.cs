using ScribblersPad.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a drawing tool input controller
    /// </summary>
    public interface IDrawingToolInputController : IBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Drawing tool
        /// </summary>
        EDrawingTool DrawingTool { get; set; }

        /// <summary>
        /// Brush size
        /// </summary>
        float BrushSize { get; set; }

        /// <summary>
        /// Drawing tool color
        /// </summary>
        Color32 DrawingToolColor { get; set; }

        /// <summary>
        /// Canvas color
        /// </summary>
        Color32 CanvasColor { get; }

        /// <summary>
        /// Drawing board controller
        /// </summary>
        DrawingBoardControllerScript DrawingBoardController { get; }

        /// <summary>
        /// Selects pen
        /// </summary>
        void SelectPen();

        /// <summary>
        /// Selects flood fill
        /// </summary>
        void SelectFloodFill();

        /// <summary>
        /// Selects eraser
        /// </summary>
        void SelectEraser();
    }
}
