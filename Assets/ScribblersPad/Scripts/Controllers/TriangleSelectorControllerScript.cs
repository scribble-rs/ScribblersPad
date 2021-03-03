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
    /// A class that describes a triangle selector controller script
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class TriangleSelectorControllerScript : MonoBehaviour, ITriangleSelectorController
    {
        /// <summary>
        /// Barycentric position
        /// </summary>
        [SerializeField]
        private Vector3 barycentricPosition = Vector3.right;

        /// <summary>
        /// Selection rectangle transform
        /// </summary>
        [SerializeField]
        private RectTransform selectionRectangleTransform = default;

        /// <summary>
        /// Gets invoked when barycentric position has been changed
        /// </summary>
        [SerializeField]
        private UnityEvent<Vector3> onBarycentricPositionChanged = default;

        /// <summary>
        /// Is passing through drag events
        /// </summary>
        private bool isPassingThroughDragEvents;

        /// <summary>
        /// Barycentric position
        /// </summary>
        public Vector3 BarycentricPosition
        {
            get => barycentricPosition;
            set
            {
                if (barycentricPosition != value)
                {
                    SetBarycentricPositionWithoutNotifying(value);
                    if (onBarycentricPositionChanged != null)
                    {
                        onBarycentricPositionChanged.Invoke(barycentricPosition);
                    }
                    OnBarycentricPositionChanged?.Invoke(barycentricPosition);
                }
            }
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
        /// Rectangle transform
        /// </summary>
        public RectTransform RectangleTransform { get; private set; }

        /// <summary>
        /// Gets invoked when barycentric position has been changed
        /// </summary>
        public event BarycentricPositionChangedDelegate OnBarycentricPositionChanged;

        /// <summary>
        /// Clamps barycentric position
        /// </summary>
        private void ClampBarycentricPosition()
        {
            if (!RectangleTransform && TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
            }
            if (RectangleTransform)
            {
                Vector2 size = RectangleTransform.rect.size * 0.5f;
                float alpha = Mathf.PI * 2.0f / 3.0f;
                Vector2 a = new Vector2(0.0f, size.y);
                Vector2 b = new Vector2(-size.x * Mathf.Sin(alpha), size.y * Mathf.Cos(alpha));
                Vector2 c = new Vector2(-b.x, b.y);
                Vector2 p = (a * barycentricPosition.x) + (b * barycentricPosition.y) + (c * barycentricPosition.z);
                if (barycentricPosition.x < 0.0f)
                {
                    float t = Mathf.Clamp(Vector2.Dot(p - b, c - b) / Vector2.Dot(c - b, c - b), 0.0f, 1.0f);
                    barycentricPosition = new Vector3(0.0f, 1.0f - t, t);
                }
                else if (barycentricPosition.y < 0.0f)
                {
                    float t = Mathf.Clamp(Vector2.Dot(p - c, a - c) / Vector2.Dot(a - c, a - c), 0.0f, 1.0f);
                    barycentricPosition = new Vector3(t, 0.0f, 1.0f - t);
                }
                else if (barycentricPosition.z < 0.0f)
                {
                    float t = Mathf.Clamp(Vector2.Dot(p - a, b - a) / Vector2.Dot(b - a, b - a), 0.0f, 1.0f);
                    barycentricPosition = new Vector3(1.0f - t, t, 0.0f);
                }
            }
            else
            {
                barycentricPosition = Vector3.right;
            }
        }

        /// <summary>
        /// Handles pointer event
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        /// <param name="isHandledOutOfBounds">Is pointer event data handled out of bounds</param>
        /// <returns>"true" if pointer event was handled, otherwise "false"</returns>
        private bool HandlePointerEvent(PointerEventData eventData, bool isHandledOutOfBounds)
        {
            bool ret = false;
            if (RectangleTransform)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectangleTransform, eventData.position, null, out Vector2 local_point))
                {
                    Vector2 size = RectangleTransform.rect.size * 0.5f;
                    float alpha = Mathf.PI * 2.0f / 3.0f;
                    Vector2 a = new Vector2(0.0f, size.y);
                    Vector2 b = new Vector2(-size.x * Mathf.Sin(alpha), size.y * Mathf.Cos(alpha));
                    Vector2Triplet vector_triplet = new Vector2Triplet
                    (
                        b - a,
                        new Vector2(-b.x - a.x, b.y - a.y),
                        local_point - a
                    );
                    float denominator = (vector_triplet.A.x * vector_triplet.B.y) - (vector_triplet.B.x * vector_triplet.A.y);
                    float v = ((vector_triplet.C.x * vector_triplet.B.y) - (vector_triplet.B.x * vector_triplet.C.y)) / denominator;
                    float w = ((vector_triplet.A.x * vector_triplet.C.y) - (vector_triplet.C.x * vector_triplet.A.y)) / denominator;
                    Vector3 uvw = new Vector3(1.0f - v - w, v, w);
                    ret = (uvw.x >= 0.0f) && (uvw.x <= 1.0f) && (uvw.y >= 0.0f) && (uvw.y <= 1.0f) && (uvw.z >= 0.0f) && (uvw.z <= 1.0f);
                    if (isHandledOutOfBounds || ret)
                    {
                        BarycentricPosition = uvw;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Passes through pointer event
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        /// <param name="methodName">Method name to invoke when passing through</param>
        /// <returns>"true" if pointer event has been successfully passed through, otherwise "false"</returns>
        private bool PassThroughPointerEvent(PointerEventData eventData, string methodName)
        {
            bool ret = false;
            if (transform.parent && transform.parent.GetComponentInParent<Selectable>() is Selectable selectable)
            {
                selectable.SendMessage(methodName, eventData);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Handles
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        /// <param name="methodName">Method name to invoke when passing through</param>
        /// <returns>"true" if pointer event data was handled, otherwise "false"</returns>
        private bool HandlePointerEventWithPassthrough(PointerEventData eventData, string methodName) => !HandlePointerEvent(eventData, false) && PassThroughPointerEvent(eventData, methodName);

        /// <summary>
        /// Sets barycenrtic position without notifying
        /// </summary>
        /// <param name="newBarycentricPosition"></param>
        public void SetBarycentricPositionWithoutNotifying(Vector3 newBarycentricPosition)
        {
            barycentricPosition = newBarycentricPosition;
            ClampBarycentricPosition();
        }

        /// <summary>
        /// Sets selection value without notifying to event listeners
        /// </summary>
        /// <param name="selectionValue">Selection value</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (HandlePointerEventWithPassthrough(eventData, nameof(OnBeginDrag)))
            {
                isPassingThroughDragEvents = true;
            }
        }

        /// <summary>
        /// Gets invoked when pointer is dragging
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (isPassingThroughDragEvents)
            {
                PassThroughPointerEvent(eventData, nameof(OnDrag));
            }
            else
            {
                HandlePointerEvent(eventData, true);
            }
        }

        /// <summary>
        /// Gets invoked when pointer drag has ended
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (isPassingThroughDragEvents)
            {
                PassThroughPointerEvent(eventData, nameof(OnEndDrag));
                isPassingThroughDragEvents = false;
            }
            else
            {
                HandlePointerEvent(eventData, true);
            }
        }

        /// <summary>
        /// Gets invoked when pointer has clicked
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerClick(PointerEventData eventData) => HandlePointerEventWithPassthrough(eventData, nameof(OnPointerClick));

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            barycentricPosition = new Vector3(Mathf.Clamp(barycentricPosition.x, 0.0f, 1.0f), Mathf.Clamp(barycentricPosition.y, 0.0f, 1.0f), Mathf.Clamp(barycentricPosition.z, 0.0f, 1.0f));
            if (barycentricPosition.sqrMagnitude > 1.0f)
            {
                barycentricPosition.Normalize();
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
            }
            else
            {
                Debug.LogError($"Please assign a \"{ nameof(RectTransform) }\" component to this game object.", this);
            }
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (RectangleTransform && selectionRectangleTransform)
            {
                Vector2 size = RectangleTransform.rect.size * 0.5f;
                float alpha = Mathf.PI * 2.0f / 3.0f;
                Vector2 b = new Vector2(-size.x * Mathf.Sin(alpha), size.y * Mathf.Cos(alpha));
                selectionRectangleTransform.anchoredPosition = (new Vector2(0.0f, size.y) * barycentricPosition.x) + (b * barycentricPosition.y) + (new Vector2(-b.x, b.y) * barycentricPosition.z);
            }
        }
    }
}
