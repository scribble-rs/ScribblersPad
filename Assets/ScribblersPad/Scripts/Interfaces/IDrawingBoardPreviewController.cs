using ScribblersPad.Controllers;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a drawing board preview controller
    /// </summary>
    public interface IDrawingBoardPreviewController : IBehaviour
    {
        /// <summary>
        /// Raw image
        /// </summary>
        RawImage RawImage { get; }

        /// <summary>
        /// Drawing board controller 
        /// </summary>
        DrawingBoardControllerScript DrawingBoardController { get; }
    }
}
