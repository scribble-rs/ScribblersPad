using ScribblersPad.Data;
using ScribblersPad.Managers;
using ScribblersSharp;
using System;
using UnitySaveGame;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// A class that describes the game manager
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// Scribble.rs client
        /// </summary>
        public static IScribblersClient ScribblersClient => ScribblersClientManagerScript.Instance ? ScribblersClientManagerScript.Instance.ScribblersClient : null;

        /// <summary>
        /// Sets Scribble.rs host
        /// </summary>
        /// <param name="host">Scribble.rs host</param>
        public static void SetScribblersHost(string host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                if (save_game.Data.ScribblersHost != host)
                {
                    save_game.Data.ScribblersHost = host;
                    if (ScribblersClientManagerScript.Instance)
                    {
                        ScribblersClientManagerScript.Instance.CreateClient();
                    }
                }
            }
        }
    }
}
