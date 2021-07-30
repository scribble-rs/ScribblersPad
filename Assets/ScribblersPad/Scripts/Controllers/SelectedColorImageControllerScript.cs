using UnityEngine;
using UnityEngine.UI;

namespace ScribblersPad.Controllers
{
    [RequireComponent(typeof(Image))]
    public class SelectedColorImageControllerScript : MonoBehaviour
    {
        private Color32 lastDrawingToolColor = Color.white;

        public Image SelectedColorImage { get; private set; }

        public DrawingToolInputControllerScript DrawingToolInputController { get; private set; }

        private void Start()
        {
            if (TryGetComponent(out Image selected_color_image))
            {
                SelectedColorImage = selected_color_image;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(Image) }\" component to this game object.", this);
            }
            DrawingToolInputController = FindObjectOfType<DrawingToolInputControllerScript>(true);
            if (!DrawingToolInputController)
            {
                Debug.LogError($"Failed to find a game object in scene with a \"{ nameof(DrawingToolInputControllerScript) }\" component attached.", this);
            }
        }

        private void Update()
        {
            if (SelectedColorImage && DrawingToolInputController)
            {
                Color32 drawing_tool_color = DrawingToolInputController.DrawingToolColor;
                if
                (
                    (lastDrawingToolColor.r != drawing_tool_color.r) ||
                    (lastDrawingToolColor.g != drawing_tool_color.g) ||
                    (lastDrawingToolColor.b != drawing_tool_color.b)
                )
                {
                    lastDrawingToolColor = drawing_tool_color;
                    SelectedColorImage.color = drawing_tool_color;
                }
            }
        }
    }
}
