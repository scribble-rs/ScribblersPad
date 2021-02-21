using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a color button controller script
    /// </summary>
    public class ColorButtonControllerScript : MonoBehaviour, IColorButtonController
    {
        /// <summary>
        /// Color
        /// </summary>
        [SerializeField]
        private Color color = Color.white;

        /// <summary>
        /// Color image
        /// </summary>
        [SerializeField]
        private Image colorImage = default;

        /// <summary>
        /// Color picker controller
        /// </summary>
        [SerializeField]
        private ColorPickerControllerScript colorPickerController = default;

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// Color image
        /// </summary>
        public Image ColorImage
        {
            get => colorImage;
            set => colorImage = value;
        }

        /// <summary>
        /// Color picker controller
        /// </summary>
        public ColorPickerControllerScript ColorPickerController
        {
            get => colorPickerController;
            set => colorPickerController = value;
        }

        /// <summary>
        /// Gets invoked when pointer is not pressing anymore
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerUp(PointerEventData eventData) => OnClickedEvent();

        /// <summary>
        /// Gets invoked when pointer is pressing down
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerDown(PointerEventData eventData) => OnClickedEvent();

        /// <summary>
        /// Gets invoked when button has been clicked
        /// </summary>
        private void OnClickedEvent()
        {
            if (colorPickerController)
            {
                colorPickerController.Color = color;
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (colorImage)
            {
                colorImage.color = color;
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (!colorImage)
            {
                Debug.LogError($"Please assign a color image to this component.", this);
            }
            if (!colorPickerController)
            {
                Debug.LogError($"Please assign a color picker to this component.", this);
            }
        }
    }
}
