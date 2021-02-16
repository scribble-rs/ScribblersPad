/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// A class that represents save game data
    /// </summary>
    public interface ISaveGameData
    {
        /// <summary>
        /// Scribble.rs host
        /// </summary>
        string ScribblersHost { get; set; }

        /// <summary>
        /// Scribble.rs lobby ID
        /// </summary>
        string ScribblersLobbyID { get; set; }

        /// <summary>
        /// Scribble.rs username
        /// </summary>
        string ScribblersUsername { get; set; }
    }
}
