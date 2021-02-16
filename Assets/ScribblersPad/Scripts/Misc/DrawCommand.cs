using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// A structure that describes a draw command
    /// </summary>
    public readonly struct DrawCommand : IDrawCommand
    {
        /// <summary>
        /// Draw comand type
        /// </summary>
        public EDrawCommandType Type { get; }

        /// <summary>
        /// From position
        /// </summary>
        public Vector2 From { get; }

        /// <summary>
        /// To position
        /// </summary>
        public Vector2 To { get; }

        /// <summary>
        /// Color
        /// </summary>
        public Color32 Color { get; }

        /// <summary>
        /// Line width
        /// </summary>
        public float LineWidth { get; }

        /// <summary>
        /// Constructs a draw command
        /// </summary>
        /// <param name="type">Draw command type</param>
        /// <param name="from">From position</param>
        /// <param name="to">To position</param>
        /// <param name="color">Color</param>
        /// <param name="lineWidth">Line width</param>
        public DrawCommand(EDrawCommandType type, Vector2 from, Vector2 to, Color32 color, float lineWidth)
        {
            Type = type;
            From = from;
            To = to;
            Color = color;
            LineWidth = lineWidth;
        }
    }
}
