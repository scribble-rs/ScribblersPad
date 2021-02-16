using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a color input material controller script
    /// </summary>
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ColorInputMaterialControllerScript : MonoBehaviour, IColorInputMaterialController
    {
        /// <summary>
        /// "InputColor" name ID
        /// </summary>
        private static readonly int inputColorNameID = Shader.PropertyToID("InputColor");

        /// <summary>
        /// Color
        /// </summary>
        [SerializeField]
        private Color color = Color.white;

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out Image image))
            {
                Image = image;
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (!Image)
            {
                if (TryGetComponent(out Image image))
                {
                    Image = image;
                }
            }
            if (Image)
            {
                Image.material.SetColor(inputColorNameID, color);
            }
        }
    }
}
