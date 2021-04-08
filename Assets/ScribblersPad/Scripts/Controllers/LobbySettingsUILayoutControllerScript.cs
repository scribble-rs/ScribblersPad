using UnityEngine;

namespace ScribblersPad.Controllers
{
    [RequireComponent(typeof(Animator))]
    public class LobbySettingsUILayoutControllerScript : MonoBehaviour
    {
        /// <summary>
        /// "lobbySettingsPageIndex" hash
        /// </summary>
        private static readonly int lobbySettingsPageIndexHash = Animator.StringToHash("lobbySettingsPageIndex");

        [SerializeField]
        private uint pageCount = 3U;

        private uint pageIndex;

        private uint lastPageIndex;

        public uint PageCount
        {
            get => pageCount;
            set
            {
                pageCount = value;
                if (pageIndex >= pageCount)
                {
                    PageIndex = (pageCount == 0U) ? 0U : (pageCount - 1U);
                }
            }
        }

        public uint PageIndex
        {
            get => pageIndex;
            set => pageIndex = (pageIndex < pageCount) ? value : ((pageCount == 0U) ? 0U : (pageCount - 1U));
        }

        public Animator LobbySettingsUILayoutAnimator { get; private set; }

        public void PreviousPage() => --PageIndex;

        public void NextPage() => ++PageIndex;

        private void OnEnable() => PageIndex = 0U;

        private void Start()
        {
            if (TryGetComponent(out Animator lobby_settings_ui_layout_animator))
            {
                LobbySettingsUILayoutAnimator = lobby_settings_ui_layout_animator;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(Animator) }\" component to this game object.", this);
            }
        }

        private void Update()
        {
            if (lastPageIndex != pageIndex)
            {
                lastPageIndex = pageIndex;
                if (LobbySettingsUILayoutAnimator)
                {
                    LobbySettingsUILayoutAnimator.SetInteger(lobbySettingsPageIndexHash, (int)pageIndex);
                }
            }
        }
    }
}
