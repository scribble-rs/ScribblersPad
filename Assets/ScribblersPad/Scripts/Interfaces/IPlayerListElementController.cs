using ScribblersSharp;
using TMPro;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a behaviour
    /// </summary>
    public interface IPlayerListElementController : IBehaviour
    {
        /// <summary>
        /// Name text
        /// </summary>
        TextMeshProUGUI NameText { get; set; }

        /// <summary>
        /// Score text
        /// </summary>
        TextMeshProUGUI ScoreText { get; set; }

        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="player">Player</param>
        void SetValues(IPlayer player);
    }
}
