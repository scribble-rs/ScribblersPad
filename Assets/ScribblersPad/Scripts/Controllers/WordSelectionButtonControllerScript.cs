using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a word selection button controller script
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class WordSelectionButtonControllerScript : AScribblersClientControllerScript, IWordSelectionButtonController
    {
        /// <summary>
        /// Word text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI wordText = default;

        /// <summary>
        /// Word text
        /// </summary>
        public TextMeshProUGUI WordText
        {
            get => wordText;
            set => wordText = value;
        }

        /// <summary>
        /// Word
        /// </summary>
        public string Word { get; private set; }

        /// <summary>
        /// Index
        /// </summary>
        public uint Index { get; private set; }

        /// <summary>
        /// Parent
        /// </summary>
        public WordSelectionControllerScript Parent { get; private set; }

        /// <summary>
        /// Button
        /// </summary>
        public Button Button { get; private set; }

        /// <summary>
        /// Gets invoked when a click event has been performed
        /// </summary>
        private void ClickEvent()
        {
            if (ScribblersClientManager)
            {
                ScribblersClientManager.SendChooseWordGameMessageAsync(Index);
            }
            if (Parent)
            {
                Parent.HideWordSelection();
            }
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
        /// Sets values in component
        /// </summary>
        /// <param name="word">Word</param>
        /// <param name="index">Index</param>
        /// <param name="parent">Parent</param>
        public void SetValues(string word, uint index, WordSelectionControllerScript parent)
        {
            if (!parent)
            {
                throw new ArgumentException("Drawing board game object does not exist.", nameof(parent));
            }
            Word = word ?? throw new ArgumentNullException(nameof(word));
            Index = index;
            Parent = parent;
            if (wordText)
            {
                wordText.text = word;
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (!wordText)
            {
                Debug.LogError("Please assign a word text to this component.", this);
            }
            if (TryGetComponent(out Button button))
            {
                Button = button;
                button.onClick.AddListener(ClickEvent);
            }
            else
            {
                Debug.LogError($"Required component \"{ nameof(UnityEngine.UI.Button) }\" is missing.", this);
            }
        }
    }
}
