using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a chat box preview element controller
    /// </summary>
    public interface IChatBoxPreviewElementController : IBaseChatBoxElementController
    {
        /// <summary>
        /// Author and content string format
        /// </summary>
        string AuthorAndContentStringFormat { get; set; }

        /// <summary>
        /// Author and content string translation
        /// </summary>
        StringTranslationObjectScript AuthorAndContentStringFormatTranslation { get; set; }
    }
}
