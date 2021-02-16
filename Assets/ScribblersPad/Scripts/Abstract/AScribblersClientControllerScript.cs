using ScribblersPad.Managers;
using System;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// An abstract class that describes a Scribble.rs client controller script
    /// </summary>
    public abstract class AScribblersClientControllerScript : MonoBehaviour, IScribblersClientController
    {
        /// <summary>
        /// Is keeping events active when disabled
        /// </summary>
        [SerializeField]
        private bool isKeepingEventsActiveWhenDisabled;

        /// <summary>
        /// Is keeping events active when disabled
        /// </summary>
        public bool IsKeepingEventsActiveWhenDisabled
        {
            get => isKeepingEventsActiveWhenDisabled;
            set
            {
                isKeepingEventsActiveWhenDisabled = value;
                if (!enabled)
                {
                    (isKeepingEventsActiveWhenDisabled ? (Action)EnableScribblersClient : DisableScribblersClient)();
                }
            }
        }

        /// <summary>
        /// Scribble.rs client manager
        /// </summary>
        public ScribblersClientManagerScript ScribblersClientManager { get; private set; }

        /// <summary>
        /// Enables Scribble.rs client
        /// </summary>
        private void EnableScribblersClient()
        {
            if (!ScribblersClientManager)
            {
                ScribblersClientManager = ScribblersClientManagerScript.Instance;
                if (ScribblersClientManager)
                {
                    SubscribeScribblersClientEvents();
                }
            }
        }

        /// <summary>
        /// Disables Scribble.rs client
        /// </summary>
        private void DisableScribblersClient()
        {
            if (ScribblersClientManager)
            {
                UnsubscribeScribblersClientEvents();
                ScribblersClientManager = null;
            }
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected abstract void SubscribeScribblersClientEvents();

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected abstract void UnsubscribeScribblersClientEvents();

        /// <summary>
        /// Gets invoked when script has been initialized
        /// </summary>
        protected virtual void Awake()
        {
            if (isKeepingEventsActiveWhenDisabled)
            {
                EnableScribblersClient();
            }
        }

        /// <summary>
        /// Gets invoked when script gets enabled
        /// </summary>
        protected virtual void OnEnable() => EnableScribblersClient();

        /// <summary>
        /// Gets invoked when script gets disabled
        /// </summary>
        protected virtual void OnDisable()
        {
            if (!isKeepingEventsActiveWhenDisabled)
            {
                DisableScribblersClient();
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected virtual void Start() => EnableScribblersClient();
    }
}
