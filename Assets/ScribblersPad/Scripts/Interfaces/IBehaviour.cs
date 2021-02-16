using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a behaviour
    /// </summary>
    public interface IBehaviour : IObject
    {
#pragma warning disable IDE1006 // Naming Styles

        /// <summary>
        /// Is behaviour enabled
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        /// Is behaviour active and enabled
        /// </summary>
        bool isActiveAndEnabled { get; }

        /// <summary>
        /// Game object
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// Transform component
        /// </summary>
        Transform transform { get; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Gets component of the specified type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Component</returns>
        T GetComponent<T>();

        /// <summary>
        /// Gets component of the specified type by going up scene hierarchy
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Component</returns>
        T GetComponentInParent<T>();

        /// <summary>
        /// Gets component of the specified type by going down scene hierarchy
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Component</returns>
        T GetComponentInChildren<T>();

        /// <summary>
        /// Gets all components of the specified type
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        T[] GetComponents<T>();

        /// <summary>
        /// Gets all components of the specified type by going up scene hierarchy
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        T[] GetComponentsInParent<T>();

        /// <summary>
        /// Gets all components of the specified type by going down scene hierarchy
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        T[] GetComponentsInChildren<T>();

        /// <summary>
        /// Tries to get component of the specified type if available
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component"></param>
        /// <returns>"true", if component exists, otherwise "false"</returns>
        bool TryGetComponent<T>(out T component);
    }
}
