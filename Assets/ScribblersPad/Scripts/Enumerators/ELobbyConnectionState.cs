/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Lobby connection state enumerator
    /// </summary>
    public enum ELobbyConnectionState
    {
        /// <summary>
        /// Is client connecting
        /// </summary>
        Connecting,

        /// <summary>
        /// Is client connected
        /// </summary>
        Connected,

        /// <summary>
        /// Is client disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// Is client kicked
        /// </summary>
        Kicked
    }
}
