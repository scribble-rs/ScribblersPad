using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a scroll rectangle position controller
    /// </summary>
    public interface IScrollRectPositionController : IBehaviour
    {
        /// <summary>
        /// Scroll rectangle
        /// </summary>
        ScrollRect ScrollRectangle { get; }
    }
}
