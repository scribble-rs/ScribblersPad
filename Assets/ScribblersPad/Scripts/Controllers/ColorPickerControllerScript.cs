using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a color picker controller script
    /// </summary>
    [ExecuteInEditMode]
    public class ColorPickerControllerScript : MonoBehaviour, IColorPickerController
    {
        /// <summary>
        /// Color
        /// </summary>
        [SerializeField]
        private Color color = Color.black;

        /// <summary>
        /// Color hue radial selector
        /// </summary>
        [SerializeField]
        private RadialSelectorControllerScript colorHueRadialSelector = default;

        /// <summary>
        /// Color saturation slider
        /// </summary>
        [SerializeField]
        private Slider colorSaturationSlider = default;

        /// <summary>
        /// Color saturation slider background color input material controller
        /// </summary>
        [SerializeField]
        private ColorInputMaterialControllerScript colorSaturationSliderBackgroundColorInputMaterialController = default;

        /// <summary>
        /// Color intensity slider
        /// </summary>
        [SerializeField]
        private Slider colorIntensitySlider = default;

        /// <summary>
        /// Color intensity slider background color input material controller
        /// </summary>
        [SerializeField]
        private ColorInputMaterialControllerScript colorIntensitySliderBackgroundColorInputMaterialController = default;

        /// <summary>
        /// Color preview image
        /// </summary>
        [SerializeField]
        private Image colorPreviewImage = default;

        /// <summary>
        /// Previously selected color preview image
        /// </summary>
        [SerializeField]
        private Image previouslySelectedColorPreviewImage = default;

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get
            {
                Color.RGBToHSV(color, out float hue, out float saturation, out float intensity);
                if (colorHueRadialSelector)
                {
                    hue = colorHueRadialSelector.SelectionValue;
                }
                if (colorSaturationSlider)
                {
                    saturation = colorSaturationSlider.value;
                }
                if (colorIntensitySlider)
                {
                    intensity = colorIntensitySlider.value;
                }
                return Color.HSVToRGB(hue, saturation, intensity);
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    UpdateSelection();
                }
            }
        }

        /// <summary>
        /// Color hue radial selector
        /// </summary>
        public RadialSelectorControllerScript ColorHueRadialSelector
        {
            get => colorHueRadialSelector;
            set => colorHueRadialSelector = value;
        }

        /// <summary>
        /// Color saturation slider
        /// </summary>
        public Slider ColorSaturationSlider
        {
            get => colorSaturationSlider;
            set => colorSaturationSlider = value;
        }

        /// <summary>
        /// Color saturation slider background color input material controller
        /// </summary>
        public ColorInputMaterialControllerScript ColorSaturationSliderBackgroundColorInputMaterialController
        {
            get => colorSaturationSliderBackgroundColorInputMaterialController;
            set => colorSaturationSliderBackgroundColorInputMaterialController = value;
        }

        /// <summary>
        /// Color intensity slider
        /// </summary>
        public Slider ColorIntensitySlider
        {
            get => colorIntensitySlider;
            set => colorIntensitySlider = value;
        }

        /// <summary>
        /// Color intensity slider background color input material controller
        /// </summary>
        public ColorInputMaterialControllerScript ColorIntensitySliderBackgroundColorInputMaterialController
        {
            get => colorIntensitySliderBackgroundColorInputMaterialController;
            set => colorIntensitySliderBackgroundColorInputMaterialController = value;
        }

        /// <summary>
        /// Color preview image
        /// </summary>
        public Image ColorPreviewImage
        {
            get => colorPreviewImage;
            set => colorPreviewImage = value;
        }

        /// <summary>
        /// Previously selected color preview image
        /// </summary>
        public Image PreviouslySelectedColorPreviewImage
        {
            get => previouslySelectedColorPreviewImage;
            set => previouslySelectedColorPreviewImage = value;
        }

        /// <summary>
        /// Updates selection
        /// </summary>
        private void UpdateSelection()
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float intensity);
            if (colorHueRadialSelector)
            {
                colorHueRadialSelector.SetSelectionValueWithoutNotifying(hue);
            }
            if (colorSaturationSlider)
            {
                colorSaturationSlider.SetValueWithoutNotify(saturation);
            }
            if (colorIntensitySlider)
            {
                colorIntensitySlider.SetValueWithoutNotify(intensity);
            }
            UpdateVisuals();
        }

        /// <summary>
        /// Updates visuals
        /// </summary>
        private void UpdateVisuals()
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out _);
            if (colorHueRadialSelector)
            {
                hue = colorHueRadialSelector.SelectionValue;
            }
            if (colorSaturationSlider)
            {
                saturation = colorSaturationSlider.value;
            }
            if (colorSaturationSliderBackgroundColorInputMaterialController)
            {
                colorSaturationSliderBackgroundColorInputMaterialController.Color = Color.HSVToRGB(hue, 1.0f, 1.0f);
            }
            if (colorIntensitySliderBackgroundColorInputMaterialController)
            {
                colorIntensitySliderBackgroundColorInputMaterialController.Color = Color.HSVToRGB(hue, saturation, 1.0f);
            }
            if (colorPreviewImage)
            {
                colorPreviewImage.color = color;
            }
        }

        /// <summary>
        /// Fetches previously selected color
        /// </summary>
        public void FetchPreviouslySelectedColor()
        {
            if (previouslySelectedColorPreviewImage)
            {
                DrawingToolInputControllerScript drawing_tool_input_controller = FindObjectOfType<DrawingToolInputControllerScript>(true);
                if (drawing_tool_input_controller)
                {
                    previouslySelectedColorPreviewImage.color = drawing_tool_input_controller.DrawingToolColor;
                }
            }
        }

        /// <summary>
        /// Submits selected color
        /// </summary>
        public void SubmitSelectedColor()
        {
            DrawingToolInputControllerScript drawing_tool_input_controller = FindObjectOfType<DrawingToolInputControllerScript>(true);
            if (drawing_tool_input_controller)
            {
                drawing_tool_input_controller.DrawingToolColor = color;
            }
            else
            {
                Debug.LogError($"Could not find \"{ nameof(DrawingToolInputControllerScript) }\" in scene.");
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate() => UpdateSelection();

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (colorHueRadialSelector)
            {
                colorHueRadialSelector.OnSelectionValueChanged += (newSelectionValue) =>
                {
                    Color.RGBToHSV(color, out _, out float saturation, out float intensity);
                    if (colorSaturationSlider)
                    {
                        saturation = colorSaturationSlider.value;
                    }
                    if (colorIntensitySlider)
                    {
                        intensity = colorIntensitySlider.value;
                    }
                    color = Color.HSVToRGB(newSelectionValue, saturation, intensity);
                    UpdateVisuals();
                };
                colorSaturationSlider.onValueChanged.AddListener((newValue) =>
                {
                    Color.RGBToHSV(color, out float hue, out float _, out float intensity);
                    if (colorHueRadialSelector)
                    {
                        hue = colorHueRadialSelector.SelectionValue;
                    }
                    if (colorIntensitySlider)
                    {
                        intensity = colorIntensitySlider.value;
                    }
                    color = Color.HSVToRGB(hue, newValue, intensity);
                    UpdateVisuals();
                });
                colorIntensitySlider.onValueChanged.AddListener((newValue) =>
                {
                    Color.RGBToHSV(color, out float hue, out float saturation, out _);
                    if (colorHueRadialSelector)
                    {
                        hue = colorHueRadialSelector.SelectionValue;
                    }
                    if (colorSaturationSlider)
                    {
                        saturation = colorSaturationSlider.value;
                    }
                    color = Color.HSVToRGB(hue, saturation, newValue);
                    UpdateVisuals();
                });
            }
        }
    }
}
