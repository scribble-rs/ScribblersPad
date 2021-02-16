using ScribblersSharp;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Used to signal before own game has been started
    /// </summary>
    /// <param name="lobby">Lobby</param>
    public delegate void BeforeOwningGameStartedDelegate(ILobby lobby);
}
