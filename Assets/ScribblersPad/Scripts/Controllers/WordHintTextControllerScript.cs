using ScribblersSharp;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a word hint controller script
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WordHintTextControllerScript : AScribblersClientControllerScript, IWordHintTextController
    {
        /// <summary>
        /// Default word color
        /// </summary>
        [SerializeField]
        private Color defaultWordColor = Color.black;

        /// <summary>
        /// Correctly guessed word color
        /// </summary>
        [SerializeField]
        private Color correctlyGuessedWordColor = new Color(0.0f, 0.25f, 0.0f);

        /// <summary>
        /// Gets invoked when my player has guessed correctly
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectlyGuessed = default;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNextTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onUpdateWordhintGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived = default;

        /// <summary>
        /// Default word color
        /// </summary>
        public Color DefaultWordColor
        {
            get => defaultWordColor;
            set => defaultWordColor = value;
        }

        /// <summary>
        /// Correctly guessed word color
        /// </summary>
        public Color CorrectlyGuessedWordColor
        {
            get => correctlyGuessedWordColor;
            set => correctlyGuessedWordColor = value;
        }

        /// <summary>
        /// Word text
        /// </summary>
        public TextMeshProUGUI WordText { get; private set; }

        /// <summary>
        /// Gets invoked when my player has guessed correctly
        /// </summary>
        public event CorrectlyGuessedDelegate OnCorrectlyGuessed;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received
        /// </summary>
        public event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Updates word
        /// </summary>
        /// <param name="wordHints">Word hint</param>
        /// <param name="isStarting">Is round starting</param>
        private void UpdateWord(IReadOnlyList<IWordHint> wordHints, bool isStarting)
        {
            if (WordText)
            {
                StringBuilder word_string_builder = new StringBuilder();
                bool is_first = true;
                foreach (IWordHint word_hint in wordHints)
                {
                    if (is_first)
                    {
                        is_first = false;
                    }
                    else
                    {
                        word_string_builder.Append(' ');
                    }
                    word_string_builder.Append((word_hint.Character == '\0') ? "_" : $"<u>{ word_hint.Character }</u>");
                }
                WordText.text = word_string_builder.ToString();
                word_string_builder.Clear();
                if (isStarting)
                {
                    WordText.color = defaultWordColor;
                }
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent(ILobby lobby)
        {
            UpdateWord(lobby.WordHints, true);
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void ScribblersClientManagerNextTurnGameMessageReceivedEvent(ILobby lobby)
        {
            if (WordText)
            {
                WordText.color = defaultWordColor;
                WordText.text = string.Empty;
            }
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received
        /// </summary>
        /// <param name="wordHints">Word hints</param>
        private void ScribblersClientManagerUpdateWordhintGameMessageReceivedEvent(IReadOnlyList<IWordHint> wordHints)
        {
            UpdateWord(wordHints, false);
            if (onUpdateWordhintGameMessageReceived != null)
            {
                onUpdateWordhintGameMessageReceived.Invoke();
            }
            OnUpdateWordhintGameMessageReceived?.Invoke(wordHints);
        }

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerCorrectGuessGameMessageReceivedEvent(IPlayer player)
        {
            if (ScribblersClientManager.MyPlayer.ID == player.ID)
            {
                if (WordText)
                {
                    WordText.color = correctlyGuessedWordColor;
                }
                if (onCorrectlyGuessed != null)
                {
                    onCorrectlyGuessed.Invoke();
                }
                OnCorrectlyGuessed?.Invoke();
            }
            if (onCorrectGuessGameMessageReceived != null)
            {
                onCorrectGuessGameMessageReceived.Invoke();
            }
            OnCorrectGuessGameMessageReceived?.Invoke(player);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived += ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdateWordhintGameMessageReceived += ScribblersClientManagerUpdateWordhintGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived += ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdateWordhintGameMessageReceived -= ScribblersClientManagerUpdateWordhintGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived -= ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out TextMeshProUGUI word_text))
            {
                WordText = word_text;
            }
            else
            {
                Debug.LogError($"Required component \"{ nameof(TextMeshProUGUI) }\" is missing.");
            }
        }
    }
}
