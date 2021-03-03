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
        /// Color saturation and intensity triangle rectangle transform
        /// </summary>
        [SerializeField]
        private RectTransform colorSaturationIntensityTriangleRectangleTransform = default;

        /// <summary>
        /// Color saturation and intensity triangle selector controller
        /// </summary>
        [SerializeField]
        private TriangleSelectorControllerScript colorSaturationIntensityTriangleSelectorController = default;

        /// <summary>
        /// Hue material input controller
        /// </summary>
        [SerializeField]
        private HueMaterialInputControllerScript hueMaterialInputController = default;

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
        /// Color saturation and intensity triangle rectangle transform
        /// </summary>
        public RectTransform ColorSaturationIntensityTriangleRectangleTransform
        {
            get => colorSaturationIntensityTriangleRectangleTransform;
            set => colorSaturationIntensityTriangleRectangleTransform = value;
        }

        /// <summary>
        /// Color saturation and intensity triangle selector controller
        /// </summary>
        public TriangleSelectorControllerScript ColorSaturationIntensityTriangleSelectorController
        {
            get => colorSaturationIntensityTriangleSelectorController;
            set => colorSaturationIntensityTriangleSelectorController = value;
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
            if (colorSaturationIntensityTriangleSelectorController)
            {
                float u = Mathf.Pow(saturation, 1.0f / 2.2f);
                float v = 1.0f - Mathf.Pow(intensity, 2.2f);
                colorSaturationIntensityTriangleSelectorController.SetBarycentricPositionWithoutNotifying(new Vector3(u, v, 1.0f - u - v));
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
            if (colorSaturationIntensityTriangleRectangleTransform)
            {
                colorSaturationIntensityTriangleRectangleTransform.localRotation = Quaternion.AngleAxis(hue * 360.0f, Vector3.back);
            }
            if (hueMaterialInputController)
            {
                hueMaterialInputController.Hue = hue;
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
                Color32 new_color = color;
                if
                (
                    (drawing_tool_input_controller.DrawingToolColor.r != new_color.r) ||
                    (drawing_tool_input_controller.DrawingToolColor.g != new_color.g) ||
                    (drawing_tool_input_controller.DrawingToolColor.b != new_color.b)
                )
                {
                    drawing_tool_input_controller.DrawingToolColor = new_color;
                    if (drawing_tool_input_controller.DrawingTool == EDrawingTool.Eraser)
                    {
                        drawing_tool_input_controller.DrawingTool = EDrawingTool.Pen;
                    }
                }
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
                    color = Color.HSVToRGB(newSelectionValue, saturation, intensity);
                    UpdateVisuals();
                };
                if (colorSaturationIntensityTriangleSelectorController)
                {
                    colorSaturationIntensityTriangleSelectorController.OnBarycentricPositionChanged += (newBarycentricPosition) =>
                    {
                        float saturation = Mathf.Pow(newBarycentricPosition.x, 1.0f / 2.2f);
                        float intensity = Mathf.Pow(1.0f - newBarycentricPosition.y, 2.2f);
                        color = Color.HSVToRGB(colorHueRadialSelector.SelectionValue, saturation, intensity);
                        UpdateVisuals();
                    };
                }
                else
                {
                    Debug.LogError("Please assign a color saturation and intensity triangle selector to this component.", this);
                }
            }
            else
            {
                Debug.LogError("Please assign a color hue radial selector to this component.", this);
            }
        }
    }
}
