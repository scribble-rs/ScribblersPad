using ScribblersSharp;
using System.Collections.Generic;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Used to signal when listing lobbies has been finished
    /// </summary>
    /// <param name="lobbyViews">Lobby views</param>
    public delegate void ListLobbyRequestFinishedDelegate(IEnumerable<ILobbyView> lobbyViews);
}
