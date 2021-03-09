using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// used to signal when barycentric position has been changed
    /// </summary>
    /// <param name="newBarycentricPosition">New barycentric position</param>
    public delegate void BarycentricPositionChangedDelegate(Vector3 newBarycentricPosition);
}
