using TMPro;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a chat input controller script
    /// </summary>
    public class ChatInputControllerScript : AScribblersClientControllerScript, IChatInputController
    {
        /// <summary>
        /// Chat input field
        /// </summary>
        [SerializeField]
        private TMP_InputField chatInputField = default;

        /// <summary>
        /// Chat input field
        /// </summary>
        public TMP_InputField ChatInputField
        {
            get => chatInputField;
            set => chatInputField = value;
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            // ...
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            // ...
        }

        /// <summary>
        /// Submits input
        /// </summary>
        public void SubmitInput()
        {
            if (chatInputField && !string.IsNullOrWhiteSpace(chatInputField.text))
            {
                string input = chatInputField.text.Trim();
                ScribblersClientManager.SendMessageGameMessage(input);
                chatInputField.text = string.Empty;
                chatInputField.ActivateInputField();
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (!chatInputField)
            {
                Debug.LogError("Please assign a chat input field reference to this component.", this);
            }
        }
    }
}
