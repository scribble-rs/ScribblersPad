using ScribblersPad.Controllers;
using ScribblersSharp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad triggers namespace
/// </summary>
namespace ScribblersPad.Triggers
{
    /// <summary>
    /// A classd that describes a brush size slider trigger script
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class BrushSizeSliderTriggerScript : AScribblersClientControllerScript, IBrushSizeSliderTrigger
    {
        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            ILobby lobby = ScribblersClientManager.Lobby;
            if (TryGetComponent(out Slider brush_size_slider))
            {
                if (lobby.MinimalBrushSize > brush_size_slider.maxValue)
                {
                    brush_size_slider.maxValue = lobby.MaximalBrushSize;
                    brush_size_slider.minValue = lobby.MinimalBrushSize;
                }
                else
                {
                    brush_size_slider.minValue = lobby.MinimalBrushSize;
                    brush_size_slider.maxValue = lobby.MaximalBrushSize;
                }
                brush_size_slider.value = lobby.MinimalBrushSize + ((lobby.MaximalBrushSize - lobby.MinimalBrushSize) / 4U);
            }
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
            Destroy(this);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents() => ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents() => ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
    }
}
