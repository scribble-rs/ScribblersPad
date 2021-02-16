using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a radial selector controller script
    /// </summary>
    [ExecuteInEditMode]
    public class RadialSelectorControllerScript : Selectable, IRadialSelectorController
    {
        /// <summary>
        /// Selection value
        /// </summary>
        [SerializeField]
        [Range(0.0f, 1.0f - float.Epsilon)]
        private float selectionValue;

        /// <summary>
        /// Radius
        /// </summary>
        [SerializeField]
        private float radius = 128.0f;

        /// <summary>
        /// Selection rectangle rectangle transform
        /// </summary>
        [SerializeField]
        private RectTransform selectionRectangleTransform = default;

        /// <summary>
        /// Gets invoked when selection value has been changed
        /// </summary>
        [SerializeField]
        private UnityEvent<float> onSelectionValueChanged = default;

        /// <summary>
        /// Rectangle transform
        /// </summary>
        public RectTransform RectangleTransform { get; private set; }

        /// <summary>
        /// Selection value
        /// </summary>
        public float SelectionValue
        {
            get => selectionValue;
            set
            {
                float old_selection_value = selectionValue;
                selectionValue = Mathf.Repeat(value, 1.0f - float.Epsilon);
                if (old_selection_value != selectionValue)
                {
                    if (onSelectionValueChanged != null)
                    {
                        onSelectionValueChanged.Invoke(selectionValue);
                    }
                    OnSelectionValueChanged?.Invoke(selectionValue);
                }
            }
        }

        /// <summary>
        /// Radius
        /// </summary>
        public float Radius
        {
            get => radius;
            set => radius = Mathf.Max(value, 0.0f);
        }

        /// <summary>
        /// Selection rectangle transform
        /// </summary>
        public RectTransform SelectionRectangleTransform
        {
            get => selectionRectangleTransform;
            set => selectionRectangleTransform = value;
        }

        /// <summary>
        /// Gets invoked when selection value has been changed
        /// </summary>
        public event SelectionValueChangedDelegate OnSelectionValueChanged;

        /// <summary>
        /// Handles pointer event
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        private void HandlePointerEvent(PointerEventData eventData)
        {
            if (RectangleTransform && selectionRectangleTransform)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectangleTransform, eventData.position, null, out Vector2 local_point))
                {
                    SelectionValue = Mathf.Repeat(Vector2.SignedAngle((local_point - RectangleTransform.rect.center).normalized, Vector2.up), 360.0f - float.Epsilon) / 360.0f;
                }
            }
        }
        /// <summary>
        /// Sets selection value without notifying to event listeners
        /// </summary>
        /// <param name="selectionValue">Selection value</param>
        public void SetSelectionValueWithoutNotifying(float selectionValue) => this.selectionValue = Mathf.Repeat(selectionValue, 1.0f - float.Epsilon);

        /// <summary>
        /// Gets invoked when pointer drag has begun
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnBeginDrag(PointerEventData eventData) => HandlePointerEvent(eventData);

        /// <summary>
        /// Gets invoked when pointer is dragging
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnDrag(PointerEventData eventData) => HandlePointerEvent(eventData);

        /// <summary>
        /// Gets invoked when pointer drag has ended
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnEndDrag(PointerEventData eventData) => HandlePointerEvent(eventData);

        /// <summary>
        /// Gets invoked when pointer has clicked
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerClick(PointerEventData eventData) => HandlePointerEvent(eventData);

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
#if UNITY_EDITOR
        protected override void OnValidate()
#else
        private void OnValidate()
#endif
        {
#if UNITY_EDITOR
            base.OnValidate();
#endif
            selectionValue = Mathf.Repeat(selectionValue, 1.0f - float.Epsilon);
            radius = Mathf.Max(radius, 0.0f);
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
            }
            else
            {
                Debug.LogError($"Please assign a \"{ nameof(RectTransform) }\" to this game object.");
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (selectionRectangleTransform)
            {
                selectionRectangleTransform.anchoredPosition = new Vector2(Mathf.Sin(selectionValue * 2.0f * Mathf.PI) * radius, Mathf.Cos(selectionValue * 2.0f * Mathf.PI) * radius);
            }
        }
    }
}
