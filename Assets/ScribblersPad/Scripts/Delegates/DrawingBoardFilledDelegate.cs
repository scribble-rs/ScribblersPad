using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Used to signal when drawing board has been flood filled
    /// </summary>
    /// <param name="position">Position</param>
    /// <param name="color">Color</param>
    public delegate void DrawingBoardFilledDelegate(Vector2 position, IColor color);
}
