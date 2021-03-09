using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a hue material input controller script
    /// </summary>
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class HueMaterialInputControllerScript : MonoBehaviour, IHueMaterialInputController
    {
        /// <summary>
        /// "Hue" name ID
        /// </summary>
        private static readonly int hueID = Shader.PropertyToID("Hue");

        /// <summary>
        /// Hue
        /// </summary>
        [SerializeField]
        private float hue = 0.0f;

        /// <summary>
        /// Hue
        /// </summary>
        public float Hue
        {
            get => hue;
            set => hue = Mathf.Repeat(value, 1.0f - float.Epsilon);
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate() => hue = Mathf.Repeat(hue, 1.0f - float.Epsilon);

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
                Image.material.SetFloat(hueID, hue);
            }
        }
    }
}
