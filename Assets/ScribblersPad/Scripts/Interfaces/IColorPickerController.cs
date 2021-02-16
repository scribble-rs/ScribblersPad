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
        /// Color saturation slider
        /// </summary>
        Slider ColorSaturationSlider { get; set; }

        /// <summary>
        /// Color saturation slider background color input material controller
        /// </summary>
        ColorInputMaterialControllerScript ColorSaturationSliderBackgroundColorInputMaterialController { get; set; }

        /// <summary>
        /// Color intensity slider
        /// </summary>
        Slider ColorIntensitySlider { get; set; }

        /// <summary>
        /// Color intensity slider background color input material controller
        /// </summary>
        ColorInputMaterialControllerScript ColorIntensitySliderBackgroundColorInputMaterialController { get; set; }

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
