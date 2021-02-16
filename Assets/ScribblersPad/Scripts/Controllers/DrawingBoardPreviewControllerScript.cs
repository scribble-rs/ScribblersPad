using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a drawing board preview controller script
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class DrawingBoardPreviewControllerScript : MonoBehaviour, IDrawingBoardPreviewController
    {
        /// <summary>
        /// Raw image
        /// </summary>
        public RawImage RawImage { get; private set; }

        /// <summary>
        /// Drawing board controller 
        /// </summary>
        public DrawingBoardControllerScript DrawingBoardController { get; private set; }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out RawImage raw_image))
            {
                RawImage = raw_image;
            }
            else
            {
                Debug.LogError("Please assign a raw image component to this game object.", this);
            }
            DrawingBoardController = FindObjectOfType<DrawingBoardControllerScript>(true);
            if (!DrawingBoardController)
            {
                Debug.LogError("Could not find any drawing board in this scene.", this);
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (DrawingBoardController)
            {
                if (!DrawingBoardController.isActiveAndEnabled)
                {
                    DrawingBoardController.ProcessNextDrawCommandInQueue();
                }
                if (DrawingBoardController.RawImage && RawImage)
                {
                    RawImage.texture = DrawingBoardController.RawImage.texture;
                }
            }
        }
    }
}
