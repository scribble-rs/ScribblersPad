using ScribblersSharp;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    // TODO: Add code documentation
    /// <summary>
    /// A class that describes a start game controller script
    /// </summary>
    public class StartGameControllerScript : AScribblersClientControllerScript, IStartGameController
    {
        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived = default;

        /// <summary>
        /// Gets invoked before your own game has started
        /// </summary>
        [SerializeField]
        private UnityEvent onBeforeOwningGameStarted = default;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked before your own game has started
        /// </summary>
        public event BeforeOwningGameStartedDelegate OnBeforeOwningGameStarted;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent(ILobby lobby)
        {
            if ((ScribblersClientManager.GameState == EGameState.Unstarted) && (ScribblersClientManager.MyPlayer.ID == ScribblersClientManager.Owner.ID))
            {
                if (onBeforeOwningGameStarted != null)
                {
                    onBeforeOwningGameStarted.Invoke();
                }
                OnBeforeOwningGameStarted?.Invoke(lobby);
            }
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents() => ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents() => ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;

        public void StartGame() => ScribblersClientManager.SendStartGameMessageAsync();
    }
}
