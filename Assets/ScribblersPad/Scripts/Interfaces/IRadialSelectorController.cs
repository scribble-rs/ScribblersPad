using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a radial selector controller
    /// </summary>
    public interface IRadialSelectorController : ISelectable, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Rectangle transform
        /// </summary>
        RectTransform RectangleTransform { get; }

        /// <summary>
        /// Selection value
        /// </summary>
        float SelectionValue { get; set; }

        /// <summary>
        /// Radius
        /// </summary>
        float Radius { get; set; }

        /// <summary>
        /// Selection rectangle transform
        /// </summary>
        RectTransform SelectionRectangleTransform { get; set; }

        /// <summary>
        /// Gets invoked when selection value has been changed
        /// </summary>
        event SelectionValueChangedDelegate OnSelectionValueChanged;

        /// <summary>
        /// Sets selection value without notifying to event listeners
        /// </summary>
        /// <param name="selectionValue">Selection value</param>
        void SetSelectionValueWithoutNotifying(float selectionValue);
    }
}
