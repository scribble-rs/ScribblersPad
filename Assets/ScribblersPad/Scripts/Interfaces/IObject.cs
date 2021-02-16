using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents an object
    /// </summary>
    public interface IObject
    {
#pragma warning disable IDE1006 // Naming Styles

        /// <summary>
        /// Hide flags
        /// </summary>
        HideFlags hideFlags { get; set; }

        /// <summary>
        /// Name of this object
        /// </summary>
        string name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
