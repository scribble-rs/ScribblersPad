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
    /// A class that describes a game UI layout controller script
    /// </summary>
    public class GameUILayoutControllerScript : AScribblersClientControllerScript, IGameUILayoutController
    {
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
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onYourTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when drawing board has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onDrawingBoardShown = default;

        /// <summary>
        /// Gets invoked when chat has been shown
        /// </summary>
        [SerializeField]
        private UnityEvent onChatShown = default;

        /// <summary>
        /// Game UI layout state
        /// </summary>
        private EGameUILayoutState gameUILayoutState = EGameUILayoutState.Nothing;

        /// <summary>
        /// Game UI layout state
        /// </summary>
        public EGameUILayoutState GameUILayoutState
        {
            get => gameUILayoutState;
            set
            {
                if (gameUILayoutState != value)
                {
                    switch (value)
                    {
                        case EGameUILayoutState.DrawingBoard:
                            gameUILayoutState = EGameUILayoutState.DrawingBoard;
                            if (onDrawingBoardShown != null)
                            {
                                onDrawingBoardShown.Invoke();
                            }
                            OnDrawingBoardShown?.Invoke();
                            break;
                        case EGameUILayoutState.Chat:
                            gameUILayoutState = EGameUILayoutState.Chat;
                            if (onChatShown != null)
                            {
                                onChatShown.Invoke();
                            }
                            OnChatShown?.Invoke();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when drawing board has been shown
        /// </summary>
        public event DrawingBoardShownDelegate OnDrawingBoardShown;

        /// <summary>
        /// Gets invoked when chat has been shown
        /// </summary>
        public event ChatShownDelegate OnChatShown;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            GameUILayoutState = EGameUILayoutState.DrawingBoard;
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
            GameUILayoutState = EGameUILayoutState.DrawingBoard;
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        /// <param name="words">Words to choose</param>
        private void ScribblersClientManagerYourTurnGameMessageReceivedEvent(IReadOnlyList<string> words)
        {
            GameUILayoutState = EGameUILayoutState.DrawingBoard;
            if (onYourTurnGameMessageReceived != null)
            {
                onYourTurnGameMessageReceived.Invoke();
            }
            OnYourTurnGameMessageReceived?.Invoke(words);
        }

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        /// <param name="player">Player who have guessed correctly</param>
        private void ScribblersClientManagerCorrectGuessGameMessageReceived(IPlayer player)
        {
            if (player.ID == ScribblersClientManager.MyPlayer.ID)
            {
                GameUILayoutState = EGameUILayoutState.Chat;
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
            ScribblersClientManager.OnYourTurnGameMessageReceived += ScribblersClientManagerYourTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived += ScribblersClientManagerCorrectGuessGameMessageReceived;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnYourTurnGameMessageReceived -= ScribblersClientManagerYourTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived -= ScribblersClientManagerCorrectGuessGameMessageReceived;
        }

        /// <summary>
        /// Shows drawing board
        /// </summary>
        public void ShowDrawingBoard() => GameUILayoutState = EGameUILayoutState.DrawingBoard;

        /// <summary>
        /// Shows chat
        /// </summary>
        public void ShowChat() => GameUILayoutState = EGameUILayoutState.Chat;
    }
}
