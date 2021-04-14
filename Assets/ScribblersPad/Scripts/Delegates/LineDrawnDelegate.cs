using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Used to signal when a line has been drawn
    /// </summary>
    /// <param name="from">Line start position</param>
    /// <param name="to">Line end position</param>
    /// <param name="color">Line color</param>
    /// <param name="lineWidth">Line width</param>
    public delegate void LineDrawnDelegate(Vector2 from, Vector2 to, IColor color, float lineWidth);
}
