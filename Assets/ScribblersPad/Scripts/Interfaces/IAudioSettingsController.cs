/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents an audio settings controller
    /// </summary>
    public interface IAudioSettingsController : IBehaviour
    {
        /// <summary>
        /// Is audio muted
        /// </summary>
        bool IsMuted { get; set; }

        /// <summary>
        /// Gets invoked when audio has been muted
        /// </summary>
        event MutedDelegate OnMuted;

        /// <summary>
        /// Gets invoked when audio has been unmuted
        /// </summary>
        event UnmutedDelegate OnUnmuted;

        /// <summary>
        /// Toggles is muted state
        /// </summary>
        void ToggleIsMutedState();
    }
}
