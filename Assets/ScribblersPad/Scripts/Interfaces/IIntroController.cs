using UnityEngine.EventSystems;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents an intro controller
    /// </summary>
    public interface IIntroController : IBehaviour, IPointerUpHandler
    {
        /// <summary>
        /// Shows main menu
        /// </summary>
        void ShowMainMenu();
    }
}
