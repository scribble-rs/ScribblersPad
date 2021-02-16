using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a chat box controller
    /// </summary>
    public interface IChatBoxController : IBehaviour
    {
        /// <summary>
        /// Chat box element limit count
        /// </summary>
        uint ChatBoxElementLimitCount { get; set; }

        /// <summary>
        /// Message chat box element asset
        /// </summary>
        GameObject MessageChatBoxElementAsset { get; set; }

        /// <summary>
        /// Own message chat box element asset
        /// </summary>
        GameObject OwnMessageChatBoxElementAsset { get; set; }

        /// <summary>
        /// Non guessing player message chat box element asset
        /// </summary>
        GameObject NonGuessingPlayerMessageChatBoxElementAsset { get; set; }

        /// <summary>
        /// Own non-guessing player message chat box element asset
        /// </summary>
        GameObject OwnNonGuessingPlayerMessageChatBoxElementAsset { get; set; }

        /// <summary>
        /// Correctly guessed chat box element asset
        /// </summary>
        GameObject CorrectlyGuessedChatBoxElementAsset { get; set; }

        /// <summary>
        /// Own correctly guessed chat box element asset
        /// </summary>
        GameObject OwnCorrectlyGuessedChatBoxElementAsset { get; set; }

        /// <summary>
        /// System message chat box element asset
        /// </summary>
        GameObject SystemMessageChatBoxElementAsset { get; set; }

        /// <summary>
        /// Chat box element parent rectangle transform
        /// </summary>
        RectTransform ChatBoxElementParentRectangleTransform { get; set; }

        /// <summary>
        /// Scroll value
        /// </summary>
        Vector2 ScrollValue { get; set; }

        /// <summary>
        /// Gets invoked when "message" game message has been received
        /// </summary>
        event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "non-guessing-player-message" game message has been received
        /// </summary>
        event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "system-message" game message has been received
        /// </summary>
        event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;
    }
}
