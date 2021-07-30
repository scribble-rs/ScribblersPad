using UnityEngine;

/// <summary>
/// Scribble.rs Pad managers namespace
/// </summary>
namespace ScribblersPad.Managers
{
    /// <summary>
    /// An abstract class that describes a manager script
    /// </summary>
    /// <typeparam name="T">Manager script type</typeparam>
    public abstract class AManagerScript<T> : MonoBehaviour, IManager where T : AManagerScript<T>
    {
        /// <summary>
        /// Manager instance
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Gets invoked when script gets enabled
        /// </summary>
        protected virtual void OnEnable()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
        }

        /// <summary>
        /// Gets invoked when script gets disabled
        /// </summary>
        protected virtual void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
