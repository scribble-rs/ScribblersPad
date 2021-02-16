using UnityEngine;
using UnityEngine.UI;

// TODO: Implement a more complex behaviour where users can interact with auto scrolling objects

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a scroll rectangle position controller script
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectPositionControllerScript : MonoBehaviour, IScrollRectPositionController
    {
        /// <summary>
        /// Scroll rectangle
        /// </summary>
        public ScrollRect ScrollRectangle { get; private set; }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out ScrollRect scroll_rectangle))
            {
                ScrollRectangle = scroll_rectangle;
            }
            else
            {
                Debug.LogError($"Required component \"{ nameof(ScrollRect) }\" is missing.", this);
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (ScrollRectangle)
            {
                ScrollRectangle.verticalNormalizedPosition = 0.0f;
            }
        }
    }
}
