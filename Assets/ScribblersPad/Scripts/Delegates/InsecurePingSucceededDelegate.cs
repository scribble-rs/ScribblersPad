/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// Used to signal when pinging an insecure host was succeessful
    /// </summary>
    /// <param name="pingTime">Ping time in seconds</param>
    public delegate void InsecurePingSucceededDelegate(float pingTime);
}
