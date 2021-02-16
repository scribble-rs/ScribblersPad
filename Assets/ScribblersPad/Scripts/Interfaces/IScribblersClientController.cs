using ScribblersPad.Managers;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a Scribble.rs client controller
    /// </summary>
    public interface IScribblersClientController : IBehaviour
    {
        /// <summary>
        /// Is keeping events active when disabled
        /// </summary>
        public bool IsKeepingEventsActiveWhenDisabled { get; set; }

        /// <summary>
        /// Scribble.rs client manager
        /// </summary>
        ScribblersClientManagerScript ScribblersClientManager { get; }
    }
}
