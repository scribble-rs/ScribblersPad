using UnityEngine;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a main menu controller script
    /// </summary>
    public class MainMenuControllerScript : MonoBehaviour, IMainMenuController
    {
        /// <summary>
        /// Is pressing escape key
        /// </summary>
        private bool isPressingEscapeKey;

        /// <summary>
        /// Connects to server
        /// </summary>
        public void Connect() => SceneLoaderManager.LoadScene("GameScene");

        /// <summary>
        /// Quits game
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if ((keyboard != null) && isPressingEscapeKey && !keyboard.escapeKey.isPressed)
            {
                QuitGame();
            }
            isPressingEscapeKey = keyboard.escapeKey.isPressed;
        }
    }
}
