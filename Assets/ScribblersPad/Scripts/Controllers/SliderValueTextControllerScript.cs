using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a slider value text controller script
    /// </summary>
    [RequireComponent(typeof(Slider))]
    [ExecuteInEditMode]
    public class SliderValueTextControllerScript : MonoBehaviour, ISliderValueTextController
    {
        /// <summary>
        /// Default slider value string format
        /// </summary>
        private static readonly string defaultSliderValueStringFormat = "{0}";

        /// <summary>
        /// Slider value string format
        /// </summary>
        [SerializeField]
        private string sliderValueStringFormat = defaultSliderValueStringFormat;

        /// <summary>
        /// Slider value text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI sliderValueText = default;

        /// <summary>
        /// Last slider value
        /// </summary>
        private float lastSliderValue;

        /// <summary>
        /// Slider value string format
        /// </summary>
        public string SliderValueStringFormat
        {
            get => sliderValueStringFormat ?? defaultSliderValueStringFormat;
            set => sliderValueStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Slider value text
        /// </summary>
        public TextMeshProUGUI SliderValueText
        {
            get => sliderValueText;
            set => sliderValueText = value;
        }

        /// <summary>
        /// Slider
        /// </summary>
        public Slider Slider { get; private set; }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out Slider slider))
            {
                Slider = slider;
            }
            else
            {
                Debug.LogError($"Please assign a \"{ nameof(Slider) }\" component to this game object.", this);
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (!Slider && TryGetComponent(out Slider slider))
            {
                Slider = slider;
            }
            if (Slider && sliderValueText)
            {
                float slider_value = Slider.value;
                if (lastSliderValue != slider_value)
                {
                    lastSliderValue = slider_value;
                    sliderValueText.text = string.Format(sliderValueStringFormat ?? defaultSliderValueStringFormat, Slider.wholeNumbers ? (object)Mathf.RoundToInt(slider_value) : slider_value);
                }
            }
        }
    }
}
