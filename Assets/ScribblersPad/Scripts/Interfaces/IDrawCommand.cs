using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a draw command
    /// </summary>
    public interface IDrawCommand
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
        public IColor Color { get; }

        /// <summary>
        /// Line width
        /// </summary>
        public float LineWidth { get; }
    }
}
