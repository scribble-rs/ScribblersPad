﻿using ScribblersSharp;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a time view controller script
    /// </summary>
    public class TimeViewControllerScript : AScribblersClientControllerScript, ITimeViewController
    {
        /// <summary>
        /// Default time 
        /// </summary>
        private static readonly string defaultCurrentTimeInSecondsStringFormat = $"Time:{ Environment.NewLine }{{0}} s";

        /// <summary>
        /// Current time in seconds string format
        /// </summary>
        [SerializeField]
        [TextArea]
        private string currentTimeInSecondsStringFormat = defaultCurrentTimeInSecondsStringFormat;

        /// <summary>
        /// Current time in seconds string format string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript currentTimeInSecondsStringFormatStringTranslation = default;

        /// <summary>
        /// Current time text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI currentTimeText = default;

        /// <summary>
        /// Time progress image
        /// </summary>
        [SerializeField]
        private Image timeProgressImage = default;

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
        /// Current time in seconds string format
        /// </summary>
        public string CurrentTimeInSecondsStringFormat
        {
            get => currentTimeInSecondsStringFormat ?? defaultCurrentTimeInSecondsStringFormat;
            set => currentTimeInSecondsStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Current time in seconds string format string translation
        /// </summary>
        public StringTranslationObjectScript CurrentTimeInSecondsStringFormatStringTranslation
        {
            get => currentTimeInSecondsStringFormatStringTranslation;
            set => currentTimeInSecondsStringFormatStringTranslation = value;
        }

        /// <summary>
        /// Current time text
        /// </summary>
        public TextMeshProUGUI CurrentTimeText
        {
            get => currentTimeText;
            set => currentTimeText = value;
        }

        /// <summary>
        /// Time progress image
        /// </summary>
        public Image TimeProgressImage
        {
            get => timeProgressImage;
            set => timeProgressImage = value;
        }

        /// <summary>
        /// Round start date and time
        /// </summary>
        public DateTime RoundStartDateTime { get; private set; }

        /// <summary>
        /// Round start time in seconds
        /// </summary>
        public double RoundStartTime { get; private set; }

        /// <summary>
        /// Current time in seconds
        /// </summary>
        public double CurrentTime => Math.Max(RoundStartTime - (DateTime.UtcNow - RoundStartDateTime).TotalSeconds, 0.0);

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Updates current time
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void UpdateCurrentTime(ILobby lobby)
        {
            RoundStartDateTime = DateTime.UtcNow;
            RoundStartTime = lobby.CurrentDrawingTime * 0.001;
            UpdateVisuals(lobby);
        }

        /// <summary>
        /// Updates visuals
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void UpdateVisuals(ILobby lobby)
        {
            if (currentTimeText)
            {
                currentTimeText.text = string.Format(currentTimeInSecondsStringFormatStringTranslation ? currentTimeInSecondsStringFormatStringTranslation.ToString() : currentTimeInSecondsStringFormat, (int)Math.Round(CurrentTime));
            }
            if (timeProgressImage)
            {
                timeProgressImage.fillAmount = (float)(Math.Round(CurrentTime * 2.0) * 500.0 / lobby.CurrentDrawingTime);
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            UpdateCurrentTime(ScribblersClientManager.Lobby);
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
            UpdateCurrentTime(ScribblersClientManager.Lobby);
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
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
            if (currentTimeInSecondsStringFormatStringTranslation)
            {
                currentTimeInSecondsStringFormat = currentTimeInSecondsStringFormatStringTranslation.ToString();
            }
            else if (string.IsNullOrWhiteSpace(currentTimeInSecondsStringFormat))
            {
                currentTimeInSecondsStringFormat = defaultCurrentTimeInSecondsStringFormat;
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (ScribblersClientManager)
            {
                ILobby lobby = ScribblersClientManager.Lobby;
                if (lobby != null)
                {
                    if (lobby.GameState == EGameState.Ongoing)
                    {
                        UpdateVisuals(lobby);
                    }
                }
            }
        }
    }
}
