using UnityEngine;

namespace ScribblersPad.Controllers
{
    [RequireComponent(typeof(Animator))]
    public class TestConnectionStateAnimatorControllerScript : MonoBehaviour
    {
        /// <summary>
        /// "testConnectionState" hash
        /// </summary>
        private static readonly int testConnectionStateHash = Animator.StringToHash("testConnectionState");

        private ETestConnectionState lastTestConnectionState = ETestConnectionState.Nothing;

        public Animator ConnectionStateAnimator { get; private set; }

        public ETestConnectionState TestConnectionState { get; set; }

        public void SetNothingTestConnectionState() => TestConnectionState = ETestConnectionState.Nothing;

        public void SetTestingTestConnectionState() => TestConnectionState = ETestConnectionState.Testing;

        public void SetSuccessfulTestConnectionState() => TestConnectionState = ETestConnectionState.Successful;

        public void SetInsecureTestConnectionState() => TestConnectionState = ETestConnectionState.Insecure;

        public void SetFailedTestConnectionState() => TestConnectionState = ETestConnectionState.Failed;

        private void Start()
        {
            if (TryGetComponent(out Animator connection_state_animator))
            {
                ConnectionStateAnimator = connection_state_animator;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(Animator) }\" component to this game object.", this);
            }
        }

        private void OnDisable() => lastTestConnectionState = ETestConnectionState.Nothing;

        private void Update()
        {
            if ((lastTestConnectionState != TestConnectionState) && ConnectionStateAnimator)
            {
                lastTestConnectionState = TestConnectionState;
                ConnectionStateAnimator.SetInteger(testConnectionStateHash, (int)TestConnectionState);
            }
        }
    }
}
