using ScribblersPad.Controllers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a color picker controller
    /// </summary>
    public interface IColorPickerController : IBehaviour
    {
        /// <summary>
        /// Color
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Color hue radial selector
        /// </summary>
        RadialSelectorControllerScript ColorHueRadialSelector { get; set; }

        /// <summary>
        /// Color saturation and intensity triangle rectangle transform
        /// </summary>
        RectTransform ColorSaturationIntensityTriangleRectangleTransform { get; set; }

        /// <summary>
        /// Color saturation and intensity triangle selector controller
        /// </summary>
        TriangleSelectorControllerScript ColorSaturationIntensityTriangleSelectorController { get; set; }

        /// <summary>
        /// Color preview image
        /// </summary>
        Image ColorPreviewImage { get; set; }

        /// <summary>
        /// Previously selected color preview image
        /// </summary>
        Image PreviouslySelectedColorPreviewImage { get; set; }

        /// <summary>
        /// Fetches previously selected color
        /// </summary>
        void FetchPreviouslySelectedColor();

        /// <summary>
        /// Submits selected color
        /// </summary>
        void SubmitSelectedColor();
    }
}
