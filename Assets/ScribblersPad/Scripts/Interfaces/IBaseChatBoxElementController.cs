/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a chat box element controller
    /// </summary>
    public interface IBaseChatBoxElementController : IBehaviour
    {
        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        public abstract void SetValues(string author, string content);
    }
}
