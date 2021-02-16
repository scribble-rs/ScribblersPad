using ScribblersSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a word selection controller script
    /// </summary>
    public class WordSelectionControllerScript : AScribblersClientControllerScript, IWordSelectionController
    {
        /// <summary>
        /// Word selection buttons
        /// </summary>
        [SerializeField]
        private List<WordSelectionButtonControllerScript> wordSelectionButtons = default;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onYourTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when word selection has been hidden
        /// </summary>
        [SerializeField]
        private UnityEvent onWordSelectionHidden = default;

        /// <summary>
        /// Word selection buttons
        /// </summary>
        public List<WordSelectionButtonControllerScript> WordSelectionButtons
        {
            get => wordSelectionButtons;
            set => wordSelectionButtons = value;
        }

        /// <summary>
        /// "your-turn" game message received event
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when word selection has been hidden
        /// </summary>
        public event WordSelectionHiddenDelegate OnWordSelectionHidden;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        /// <param name="words"></param>
        private void ScribblersClientManagerYourTurnGameMessageReceivedEvent(IReadOnlyList<string> words)
        {
            HideWordSelection();
            for (int index = 0, length = Mathf.Min(words.Count, wordSelectionButtons.Count); index < length; index++)
            {
                WordSelectionButtonControllerScript word_selection_button = wordSelectionButtons[index];
                if (word_selection_button)
                {
                    word_selection_button.SetValues(words[index], (uint)index, this);
                    word_selection_button.gameObject.SetActive(true);
                }
            }
            if (onYourTurnGameMessageReceived != null)
            {
                onYourTurnGameMessageReceived.Invoke();
            }
            OnYourTurnGameMessageReceived?.Invoke(words);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents() => ScribblersClientManager.OnYourTurnGameMessageReceived += ScribblersClientManagerYourTurnGameMessageReceivedEvent;

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents() => ScribblersClientManager.OnYourTurnGameMessageReceived -= ScribblersClientManagerYourTurnGameMessageReceivedEvent;

        /// <summary>
        /// Hides word selection
        /// </summary>
        public void HideWordSelection()
        {
            wordSelectionButtons ??= new List<WordSelectionButtonControllerScript>();
            foreach (WordSelectionButtonControllerScript word_selection_button in wordSelectionButtons)
            {
                if (word_selection_button)
                {
                    word_selection_button.gameObject.SetActive(false);
                }
            }
            if (onWordSelectionHidden != null)
            {
                onWordSelectionHidden.Invoke();
            }
            OnWordSelectionHidden?.Invoke();
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate() => wordSelectionButtons ??= new List<WordSelectionButtonControllerScript>();

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            wordSelectionButtons ??= new List<WordSelectionButtonControllerScript>();
            foreach (WordSelectionButtonControllerScript word_selection_button in wordSelectionButtons)
            {
                if (!word_selection_button)
                {
                    Debug.LogError("Please assign all word selection buttons to word selection button list.", this);
                    break;
                }
            }
        }
    }
}
