using UnityEngine;

/// <summary>
/// Scribble.rs Pad triggers namespace
/// </summary>
namespace ScribblersPad.Triggers
{
    /// <summary>
    /// A class that describes a trigger that enables V-Sync
    /// </summary>
    public class EnableVSyncTriggerScript : MonoBehaviour, IEnableVSyncTrigger
    {
        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            QualitySettings.vSyncCount = 1;
            Destroy(gameObject);
        }
    }
}
