using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a color input material controller
    /// </summary>
    public interface IColorInputMaterialController : IBehaviour
    {
        /// <summary>
        /// Color
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        Image Image { get; }
    }
}
