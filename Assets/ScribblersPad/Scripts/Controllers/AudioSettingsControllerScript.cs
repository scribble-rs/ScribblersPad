using UnityAudioManager;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes an ausio settings controller script
    /// </summary>
    public class AudioSettingsControllerScript : MonoBehaviour, IAudioSettingsController
    {
        /// <summary>
        /// "isMuted" hash
        /// </summary>
        private static readonly int isMutedHash = Animator.StringToHash("isMuted");

        /// <summary>
        /// Gets invoked when audio has been muted
        /// </summary>
        [SerializeField]
        private UnityEvent onMuted;

        /// <summary>
        /// Gets invoked when audio has been unmuted
        /// </summary>
        [SerializeField]
        private UnityEvent onUnmuted;

        /// <summary>
        /// Last is muted
        /// </summary>
        private bool lastIsMuted = true;

        /// <summary>
        /// Is audio muted
        /// </summary>
        public bool IsMuted
        {
            get => AudioManager.IsMuted;
            set
            {
                if (AudioManager.IsMuted != value)
                {
                    AudioManager.IsMuted = value;
                    bool is_muted = AudioManager.IsMuted;
                    if (is_muted != value)
                    {
                        if (is_muted)
                        {
                            if (onMuted != null)
                            {
                                onMuted.Invoke();
                            }
                            OnMuted?.Invoke();
                        }
                        else
                        {
                            if (onUnmuted != null)
                            {
                                onUnmuted.Invoke();
                            }
                            OnUnmuted?.Invoke();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Audio image animator
        /// </summary>
        public Animator AudioImageAnimator { get; private set; }

        /// <summary>
        /// Gets invoked when audio has been muted
        /// </summary>
        public event MutedDelegate OnMuted;

        /// <summary>
        /// Gets invoked when audio has been unmuted
        /// </summary>
        public event UnmutedDelegate OnUnmuted;

        /// <summary>
        /// Toggles is muted state
        /// </summary>
        public void ToggleIsMutedState() => IsMuted = !IsMuted;

        private void Start()
        {
            if (TryGetComponent(out Animator audio_image_animator))
            {
                AudioImageAnimator = audio_image_animator;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(Animator) }\" component to this game object.", this);
            }
        }

        private void Update()
        {
            bool is_muted = IsMuted;
            if ((lastIsMuted != is_muted) && AudioImageAnimator)
            {
                lastIsMuted = is_muted;
                AudioImageAnimator.SetBool(isMutedHash, !lastIsMuted);
            }
        }
    }
}
