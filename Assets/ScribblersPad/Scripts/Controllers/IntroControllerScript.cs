using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes an intro controller script
    /// </summary>
    public class IntroControllerScript : MonoBehaviour, IIntroController
    {
        /// <summary>
        /// Is any keyboard key being pressed
        /// </summary>
        private bool isAnyKeyboardKeyPressed;

        /// <summary>
        /// Shows main menu
        /// </summary>
        public void ShowMainMenu() => SceneLoaderManager.LoadScene("MainMenuScene");

        /// <summary>
        /// Gets invoked 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData) => ShowMainMenu();

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if ((keyboard != null) && isAnyKeyboardKeyPressed && !keyboard.anyKey.isPressed)
            {
                ShowMainMenu();
            }
            isAnyKeyboardKeyPressed = (keyboard != null) && keyboard.anyKey.isPressed;
        }
    }
}
