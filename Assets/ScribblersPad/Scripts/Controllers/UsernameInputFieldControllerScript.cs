using ScribblersSharp;
using System.Collections.Generic;
using TMPro;
using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a username input field controller script
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class UsernameInputFieldControllerScript : AScribblersClientControllerScript, IUsernameInputFieldController
    {
        /// <summary>
        /// Username is too long title string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript usernameIsTooLongTitleStringTranslation = default;

        /// <summary>
        /// Username is too long message string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript usernameIsTooLongMessageStringTranslation = default;

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
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onUpdatePlayersGameMessageReceived = default;

        /// <summary>
        /// Username is too long title string translation
        /// </summary>
        public StringTranslationObjectScript UsernameIsTooLongTitleStringTranslation
        {
            get => usernameIsTooLongTitleStringTranslation;
            set => usernameIsTooLongTitleStringTranslation = value;
        }

        /// <summary>
        /// Username is too long message string translation
        /// </summary>
        public StringTranslationObjectScript UsernameIsTooLongMessageStringTranslation
        {
            get => usernameIsTooLongMessageStringTranslation;
            set => usernameIsTooLongMessageStringTranslation = value;
        }

        /// <summary>
        /// Username input field
        /// </summary>
        public TMP_InputField UsernameInputField { get; private set; }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Updates visuals
        /// </summary>
        private void UpdateVisuals()
        {
            if (UsernameInputField && ScribblersClientManager && (ScribblersClientManager.Lobby != null))
            {
                UsernameInputField.SetTextWithoutNotify(ScribblersClientManager.Lobby.MyPlayer.Name);
            }
        }

        /// <summary>
        /// Updates visuals when enabled
        /// </summary>
        private void UpdateVisualsWhenEnabled()
        {
            if (!UsernameInputField && TryGetComponent(out TMP_InputField username_input_field))
            {
                UsernameInputField = username_input_field;
            }
            UpdateVisuals();
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            UpdateVisuals();
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        private void ScribblersClientManagerNextTurnGameMessageReceivedEvent()
        {
            UpdateVisuals();
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        private void ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent(IReadOnlyDictionary<string, IPlayer> players)
        {
            UpdateVisuals();
            if (onUpdatePlayersGameMessageReceived != null)
            {
                onUpdatePlayersGameMessageReceived.Invoke();
            }
            OnUpdatePlayersGameMessageReceived?.Invoke(players);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived += ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdatePlayersGameMessageReceived += ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdatePlayersGameMessageReceived -= ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent;
        }

        /// <summary>
        /// Submits username
        /// </summary>
        public void SubmitUsernameChanges()
        {
            if (UsernameInputField && ScribblersClientManager && (ScribblersClientManager.Lobby != null))
            {
                string trimmed_username = UsernameInputField.text.Trim();
                if (trimmed_username.Length > Rules.maximalUsernameLength)
                {
                    Dialog.Show((usernameIsTooLongTitleStringTranslation == null) ? string.Empty : usernameIsTooLongTitleStringTranslation.ToString(), (usernameIsTooLongMessageStringTranslation == null) ? string.Empty : usernameIsTooLongMessageStringTranslation.ToString(), EDialogType.Error, EDialogButtons.OK);
                    UsernameInputField.SetTextWithoutNotify(ScribblersClientManager.Lobby.MyPlayer.Name);
                }
                else if (ScribblersClientManager.Lobby.MyPlayer.Name != trimmed_username)
                {
                    ScribblersClientManager.SendNameChangeGameMessageAsync(trimmed_username);
                }
            }
        }

        /// <summary>
        /// Gets invoked when script gets enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateVisualsWhenEnabled();
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            UpdateVisualsWhenEnabled();
        }
    }
}
