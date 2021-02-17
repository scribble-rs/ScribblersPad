using ScribblersPad.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a color button controller script
    /// </summary>
    public interface IColorButtonController : IBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Color
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Color image
        /// </summary>
        Image ColorImage { get; set; }

        /// <summary>
        /// Color picker controller
        /// </summary>
        ColorPickerControllerScript ColorPickerController { get; set; }
    }
}
