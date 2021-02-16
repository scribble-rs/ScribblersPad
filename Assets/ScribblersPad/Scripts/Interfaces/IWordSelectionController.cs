using ScribblersPad.Controllers;
using ScribblersSharp;
using System.Collections.Generic;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a word selection controller
    /// </summary>
    public interface IWordSelectionController : IScribblersClientController
    {
        /// <summary>
        /// Word selection buttons
        /// </summary>
        public List<WordSelectionButtonControllerScript> WordSelectionButtons { get; set; }

        /// <summary>
        /// "your-turn" game message received event
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when word selection has been hidden
        /// </summary>
        event WordSelectionHiddenDelegate OnWordSelectionHidden;

        /// <summary>
        /// Hides word selection
        /// </summary>
        void HideWordSelection();
    }
}
