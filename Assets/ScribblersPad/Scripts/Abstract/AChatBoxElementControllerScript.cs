using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// An abstract class that describes a chat box element controller script
    /// </summary>
    public abstract class AChatBoxElementControllerScript : MonoBehaviour, IBaseChatBoxElementController
    {
        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        public abstract void SetValues(string author, string content);
    }
}
