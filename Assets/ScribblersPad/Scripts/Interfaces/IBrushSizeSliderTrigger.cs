using ScribblersSharp;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a brush size slider trigger
    /// </summary>
    public interface IBrushSizeSliderTrigger : IScribblersClientController
    {
        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;
    }
}
