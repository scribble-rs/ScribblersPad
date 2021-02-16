using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a chat box element 
    /// </summary>
    public class ChatBoxElementControllerScript : AChatBoxElementControllerScript, IChatBoxElementControllerScript
    {
        /// <summary>
        /// Author text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI authorText = default;

        /// <summary>
        /// Content text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI contentText = default;

        /// <summary>
        /// Author text
        /// </summary>
        public TextMeshProUGUI AuthorText
        {
            get => authorText;
            set => authorText = value;
        }

        /// <summary>
        /// Content text
        /// </summary>
        public TextMeshProUGUI ContentText
        {
            get => contentText;
            set => contentText = value;
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
            if (authorText)
            {
                authorText.text = author;
            }
            if (contentText)
            {
                contentText.text = content;
            }
        }
    }
}
