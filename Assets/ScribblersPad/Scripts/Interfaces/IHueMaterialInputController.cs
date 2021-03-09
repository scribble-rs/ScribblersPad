using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a hue material input controller
    /// </summary>
    public interface IHueMaterialInputController : IBehaviour
    {
        /// <summary>
        /// Hue
        /// </summary>
        float Hue { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        Image Image { get; }
    }
}
