using TMPro;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a slider value text controller
    /// </summary>
    public interface ISliderValueTextController : IBehaviour
    {
        /// <summary>
        /// Slider value string format
        /// </summary>
        string SliderValueStringFormat { get; set; }

        /// <summary>
        /// Slider value text
        /// </summary>
        TextMeshProUGUI SliderValueText { get; set; }

        /// <summary>
        /// Slider
        /// </summary>
        Slider Slider { get; }
    }
}
