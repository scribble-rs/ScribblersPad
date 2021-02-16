using ScribblersPad.Data;
using TMPro;
using UnityEngine;
using UnitySaveGame;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a debug main menu controller script class
    /// </summary>
    public class DebugMainMenuControllerScript : MonoBehaviour, IDebugMainMenuController
    {
        /// <summary>
        /// Host input field
        /// </summary>
        [SerializeField]
        private TMP_InputField hostInputField = default;

        /// <summary>
        /// Lobby ID input field
        /// </summary>
        [SerializeField]
        private TMP_InputField lobbyIDInputField = default;

        /// <summary>
        /// Username input field
        /// </summary>
        [SerializeField]
        private TMP_InputField usernameInputField = default;

        /// <summary>
        /// Host input field
        /// </summary>
        public TMP_InputField HostInputField
        {
            get => hostInputField;
            set => hostInputField = value;
        }

        /// <summary>
        /// Lobby ID input field
        /// </summary>
        public TMP_InputField LobbyIDInputField
        {
            get => lobbyIDInputField;
            set => lobbyIDInputField = value;
        }

        /// <summary>
        /// Username input field
        /// </summary>
        public TMP_InputField UsernameInputField
        {
            get => usernameInputField;
            set => usernameInputField = value;
        }

        /// <summary>
        /// Saves input
        /// </summary>
        public void SaveInput()
        {
            if (hostInputField && lobbyIDInputField && usernameInputField)
            {
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    save_game.Data.ScribblersHost = hostInputField.text.Trim();
                    save_game.Data.ScribblersLobbyID = lobbyIDInputField.text.Trim();
                    save_game.Data.ScribblersUsername = usernameInputField.text.Trim();
                    save_game.Save();
                }
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                if (hostInputField)
                {
                    hostInputField.text = save_game.Data.ScribblersHost.Trim();
                }
                else
                {
                    Debug.LogError("Please assign a host input field to this component.", this);
                }
                if (lobbyIDInputField)
                {
                    lobbyIDInputField.text = save_game.Data.ScribblersLobbyID.Trim();
                }
                else
                {
                    Debug.LogError("Please assign a lobby ID input field to this component.", this);
                }
                if (usernameInputField)
                {
                    usernameInputField.text = save_game.Data.ScribblersUsername.Trim();
                }
                else
                {
                    Debug.LogError("Please assign a username input field to this component.", this);
                }
            }
        }
    }
}
