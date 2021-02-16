/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a main menu controller
    /// </summary>
    public interface IMainMenuController : IBehaviour
    {
        /// <summary>
        /// Connects to server
        /// </summary>
        void Connect();

        /// <summary>
        /// Quits game
        /// </summary>
        void QuitGame();
    }
}
