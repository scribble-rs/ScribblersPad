using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// A structure that describes a 2D Vector triplet
    /// </summary>
    public readonly struct Vector2Triplet
    {
        /// <summary>
        /// A
        /// </summary>
        public Vector2 A { get; }

        /// <summary>
        /// B
        /// </summary>
        public Vector2 B { get; }

        /// <summary>
        /// C
        /// </summary>
        public Vector2 C { get; }

        /// <summary>
        /// Constructs a 2D vector triplet
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <param name="c">C</param>
        public Vector2Triplet(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
