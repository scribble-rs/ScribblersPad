using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a triangle selector controller
    /// </summary>
    public interface ITriangleSelectorController : IBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Barycentric position
        /// </summary>
        Vector3 BarycentricPosition { get; set; }

        /// <summary>
        /// Selection rectangle transform
        /// </summary>
        RectTransform SelectionRectangleTransform { get; set; }

        /// <summary>
        /// Rectangle transform
        /// </summary>
        RectTransform RectangleTransform { get; }

        /// <summary>
        /// Gets invoked when barycentric position has been changed
        /// </summary>
        event BarycentricPositionChangedDelegate OnBarycentricPositionChanged;

        /// <summary>
        /// Sets barycenrtic position without notifying
        /// </summary>
        /// <param name="newBarycentricPosition"></param>
        void SetBarycentricPositionWithoutNotifying(Vector3 newBarycentricPosition);
    }
}
