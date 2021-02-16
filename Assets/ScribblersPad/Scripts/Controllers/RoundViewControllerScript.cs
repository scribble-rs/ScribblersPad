using ScribblersSharp;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a round view controller script
    /// </summary>
    public class RoundViewControllerScript : AScribblersClientControllerScript, IRoundViewController
    {
        /// <summary>
        /// Default current
        /// </summary>
        private static readonly string defaultCurrentRoundStringFormat = $"Round:{ Environment.NewLine }{{0}}/{{1}}";

        /// <summary>
        /// Current round string format
        /// </summary>
        [SerializeField]
        [TextArea]
        private string currentRoundStringFormat = defaultCurrentRoundStringFormat;

        /// <summary>
        /// Current round string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript currentRoundStringTranslation = default;

        /// <summary>
        /// Round text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI roundText = default;

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
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Updates visuals
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void UpdateVisuals(ILobby lobby)
        {
            if (roundText)
            {
                roundText.text = string.Format(currentRoundStringTranslation ? currentRoundStringTranslation.ToString() : currentRoundStringFormat, lobby.Round, lobby.MaximalRounds);
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent(ILobby lobby)
        {
            UpdateVisuals(lobby);
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
            UpdateVisuals(lobby);
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived += ScribblersClientManagerNextTurnGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (currentRoundStringTranslation)
            {
                currentRoundStringFormat = currentRoundStringTranslation.ToString();
            }
            else if (string.IsNullOrWhiteSpace(currentRoundStringFormat))
            {
                currentRoundStringFormat = defaultCurrentRoundStringFormat;
            }
        }
    }
}
