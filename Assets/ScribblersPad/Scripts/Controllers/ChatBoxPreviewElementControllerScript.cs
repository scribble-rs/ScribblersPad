using System;
using TMPro;
using UnityEngine;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a chat box preview element controller script
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ChatBoxPreviewElementControllerScript : AChatBoxElementControllerScript, IChatBoxPreviewElementController
    {
        /// <summary>
        /// Default author and content string format
        /// </summary>
        private static readonly string defaultAuthorAndContentStringFormat = "<b>{0}</b>: {1}";

        /// <summary>
        /// Author and content string format
        /// </summary>
        [SerializeField]
        [TextArea]
        private string authorAndContentStringFormat = defaultAuthorAndContentStringFormat;

        /// <summary>
        /// Author and content string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript authorAndContentStringFormatTranslation = default;

        /// <summary>
        /// Author and content string format
        /// </summary>
        public string AuthorAndContentStringFormat
        {
            get => authorAndContentStringFormat ?? string.Empty;
            set => authorAndContentStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Author and content string translation
        /// </summary>
        public StringTranslationObjectScript AuthorAndContentStringFormatTranslation
        {
            get => authorAndContentStringFormatTranslation;
            set => authorAndContentStringFormatTranslation = value;
        }

        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        public override void SetValues(string author, string content)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (TryGetComponent(out TextMeshProUGUI author_and_content_text))
            {
                author_and_content_text.text = string.Format(authorAndContentStringFormatTranslation ? authorAndContentStringFormatTranslation.ToString() : authorAndContentStringFormat, author, content);
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (authorAndContentStringFormatTranslation)
            {
                authorAndContentStringFormat = authorAndContentStringFormatTranslation.ToString();
            }
            else if (string.IsNullOrWhiteSpace(authorAndContentStringFormat))
            {
                authorAndContentStringFormat = defaultAuthorAndContentStringFormat;
            }
        }
    }
}
