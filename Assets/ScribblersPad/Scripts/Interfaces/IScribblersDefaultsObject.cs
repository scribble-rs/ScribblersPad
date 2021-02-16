/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a Scribble.rs defaults object
    /// </summary>
    public interface IScribblersDefaultsObject : IScriptableObject
    {
        /// <summary>
        /// Host
        /// </summary>
        string Host { get; set; }
    }
}
