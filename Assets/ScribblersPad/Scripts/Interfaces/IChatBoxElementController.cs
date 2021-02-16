using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a chat box element controller script
    /// </summary>
    public interface IChatBoxElementControllerScript : IBaseChatBoxElementController
    {
        /// <summary>
        /// Author text
        /// </summary>
        public TextMeshProUGUI AuthorText { get; set; }

        /// <summary>
        /// Content text
        /// </summary>
        public TextMeshProUGUI ContentText { get; set; }
    }
}
