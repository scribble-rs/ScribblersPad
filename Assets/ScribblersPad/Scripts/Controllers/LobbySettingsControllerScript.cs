using ScribblersPad.Data;
using ScribblersPad.Objects;
using ScribblersSharp;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnitySaveGame;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a lobby settings controller script
    /// </summary>
    public class LobbySettingsControllerScript : MonoBehaviour, ILobbySettingsController
    {
        /// <summary>
        /// Maximal test connection task time
        /// </summary>
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float maximalTestConnectionTaskTime = 5.0f;

        /// <summary>
        /// Host input field
        /// </summary>
        [SerializeField]
        private TMP_InputField hostInputField = default;

        /// <summary>
        /// Is using secure protocols toggle
        /// </summary>
        [SerializeField]
        private Toggle isUsingSecureProtocolsToggle = default;

        /// <summary>
        /// Username input field
        /// </summary>
        [SerializeField]
        private TMP_InputField usernameInputField = default;

        /// <summary>
        /// Lobby language dropdown controller
        /// </summary>
        [SerializeField]
        private LobbyLanguageDropdownControllerScript lobbyLanguageDropdownController = default;

        /// <summary>
        /// Drawing time slider
        /// </summary>
        [SerializeField]
        private Slider drawingTimeSlider = default;

        /// <summary>
        /// Round count slider
        /// </summary>
        [SerializeField]
        private Slider roundCountSlider = default;

        /// <summary>
        /// Maximal player count slider
        /// </summary>
        [SerializeField]
        private Slider maximalPlayerCountSlider = default;

        /// <summary>
        /// Is lobby public toggle
        /// </summary>
        [SerializeField]
        private Toggle isLobbyPublicToggle = default;

        /// <summary>
        /// Custom words input field
        /// </summary>
        [SerializeField]
        private TMP_InputField customWordsInputField = default;

        /// <summary>
        /// Custom words chance slider
        /// </summary>
        [SerializeField]
        private Slider customWordsChanceSlider = default;

        /// <summary>
        /// Players per IP limit slider
        /// </summary>
        [SerializeField]
        private Slider playersPerIPLimitSlider = default;

        /// <summary>
        /// Is votekicking enabled toggle
        /// </summary>
        [SerializeField]
        private Toggle isVotekickingEnabledToggle = default;

        /// <summary>
        /// Gets invoked when pinging host has been started
        /// </summary>
        [SerializeField]
        private UnityEvent<string> onPingStarted = default;

        /// <summary>
        /// Gets invoked when pinging host has failed
        /// </summary>
        [SerializeField]
        private UnityEvent onPingFailed = default;

        /// <summary>
        /// Gets invoked when pinging host was successful
        /// </summary>
        [SerializeField]
        private UnityEvent<float> onPingSucceeded = default;

        /// <summary>
        /// Test connection task
        /// </summary>
        private Task<bool> testConnectionTask;

        /// <summary>
        /// Elapsed test connection task time
        /// </summary>
        private float elapsedTestConnectionTaskTime;

        /// <summary>
        /// Maximal test connection task time
        /// </summary>
        public float MaximalTestConnectionTaskTime
        {
            get => maximalTestConnectionTaskTime;
            set => maximalTestConnectionTaskTime = Mathf.Max(value, 0.0f);
        }

        /// <summary>
        /// Host input field
        /// </summary>
        public TMP_InputField HostInputField
        {
            get => hostInputField;
            set => hostInputField = value;
        }

        /// <summary>
        /// Is using secure protocols toggle
        /// </summary>
        public Toggle IsUsingSecureProtocolsToggle
        {
            get => isUsingSecureProtocolsToggle;
            set => isUsingSecureProtocolsToggle = value;
        }

        /// <summary>
        /// Username input field
        /// </summary>
        public TMP_InputField UsernameInputField
        {
            get => usernameInputField;
            set => usernameInputField = value;
        }

        /// <summary>
        /// Lobby language dropdown controller
        /// </summary>
        public LobbyLanguageDropdownControllerScript LobbyLanguageDropdownController
        {
            get => lobbyLanguageDropdownController;
            set => lobbyLanguageDropdownController = value;
        }

        /// <summary>
        /// Drawing time slider
        /// </summary>
        public Slider DrawingTimeSlider
        {
            get => drawingTimeSlider;
            set => drawingTimeSlider = value;
        }

        /// <summary>
        /// Round count slider
        /// </summary>
        public Slider RoundCountSlider
        {
            get => roundCountSlider;
            set => roundCountSlider = value;
        }

        /// <summary>
        /// Maximal player count slider
        /// </summary>
        public Slider MaximalPlayerCountSlider
        {
            get => maximalPlayerCountSlider;
            set => maximalPlayerCountSlider = value;
        }

        /// <summary>
        /// Is lobby public toggle
        /// </summary>
        public Toggle IsLobbyPublicToggle
        {
            get => isLobbyPublicToggle;
            set => isLobbyPublicToggle = value;
        }

        /// <summary>
        /// Custom words input field
        /// </summary>
        public TMP_InputField CustomWordsInputField
        {
            get => customWordsInputField;
            set => customWordsInputField = value;
        }

        /// <summary>
        /// Custom words chance slider
        /// </summary>
        public Slider CustomWordsChanceSlider
        {
            get => customWordsChanceSlider;
            set => customWordsChanceSlider = value;
        }

        /// <summary>
        /// Players per IP limit slider
        /// </summary>
        public Slider PlayersPerIPLimitSlider
        {
            get => playersPerIPLimitSlider;
            set => playersPerIPLimitSlider = value;
        }

        /// <summary>
        /// Is votekicking enabled toggle
        /// </summary>
        public Toggle IsVotekickingEnabledToggle
        {
            get => isVotekickingEnabledToggle;
            set => isVotekickingEnabledToggle = value;
        }

        /// <summary>
        /// Gets invoked when pinging host has been started
        /// </summary>
        public event PingStartedDelegate OnPingStarted;

        /// <summary>
        /// Gets invoked when pinging host has failed
        /// </summary>
        public event PingFailedDelegate OnPingFailed;

        /// <summary>
        /// Gets invoked when pinging host was successful
        /// </summary>
        public event PingSucceededDelegate OnPingSucceeded;

        /// <summary>
        /// Tests connection
        /// </summary>
        public void TestConnection()
        {
            if (hostInputField && isUsingSecureProtocolsToggle)
            {
                if (testConnectionTask != null)
                {
                    testConnectionTask = null;
                    if (onPingFailed != null)
                    {
                        onPingFailed.Invoke();
                    }
                    OnPingFailed?.Invoke();
                }
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    string host = hostInputField.text.Trim();
                    string user_session_id = save_game.Data.UserSessionID;
                    bool is_using_secure_protocols = isUsingSecureProtocolsToggle.isOn;
                    testConnectionTask = string.IsNullOrWhiteSpace(host) ?
                        Task.FromResult(false) :
                        Task.Run
                        (
                            async () =>
                            {
                                bool result = false;
                                try
                                {
                                    using (IScribblersClient scribblers_client = Clients.Create(host, user_session_id, is_using_secure_protocols))
                                    {
                                        result = await scribblers_client.GetServerStatisticsAsync() != null;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError(e);
                                }
                                return result;
                            }
                        );
                    elapsedTestConnectionTaskTime = 0.0f;
                    if (onPingStarted != null)
                    {
                        onPingStarted.Invoke(host);
                    }
                    OnPingStarted?.Invoke(host);
                }
            }
        }

        /// <summary>
        /// Resets host to default
        /// </summary>
        public void ResetHostToDefault()
        {
            if (hostInputField && isUsingSecureProtocolsToggle)
            {
                hostInputField.SetTextWithoutNotify(ScribblersDefaultsObjectScript.defaultHost);
                isUsingSecureProtocolsToggle.SetIsOnWithoutNotify(true);
                TestConnection();
            }
        }

        /// <summary>
        /// Saves lobby settings
        /// </summary>
        public void SaveSettings()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                if (hostInputField)
                {
                    save_game.Data.ScribblersHost = hostInputField.text.Trim();
                }
                if (isUsingSecureProtocolsToggle)
                {
                    save_game.Data.IsUsingSecureProtocols = isUsingSecureProtocolsToggle.isOn;
                }
                if (usernameInputField)
                {
                    save_game.Data.Username = usernameInputField.text.Trim();
                }
                if (lobbyLanguageDropdownController)
                {
                    save_game.Data.LobbyLanguage = lobbyLanguageDropdownController.SelectedLanguage;
                }
                if (drawingTimeSlider)
                {
                    save_game.Data.DrawingTime = (uint)Mathf.RoundToInt(drawingTimeSlider.value);
                }
                if (roundCountSlider)
                {
                    save_game.Data.RoundCount = (uint)Mathf.RoundToInt(roundCountSlider.value);
                }
                if (maximalPlayerCountSlider)
                {
                    save_game.Data.MaximalPlayerCount = (uint)Mathf.RoundToInt(maximalPlayerCountSlider.value);
                }
                if (isLobbyPublicToggle)
                {
                    save_game.Data.IsLobbyPublic = isLobbyPublicToggle.isOn;
                }
                if (customWordsInputField)
                {
                    save_game.Data.CustomWords = customWordsInputField.text.Trim();
                }
                if (customWordsChanceSlider)
                {
                    save_game.Data.CustomWordsChance = (uint)Mathf.RoundToInt(customWordsChanceSlider.value);
                }
                if (playersPerIPLimitSlider)
                {
                    save_game.Data.PlayersPerIPLimit = (uint)Mathf.RoundToInt(playersPerIPLimitSlider.value);
                }
                if (isVotekickingEnabledToggle)
                {
                    save_game.Data.IsVotekickingEnabled = isVotekickingEnabledToggle.isOn;
                }
                save_game.Save();
            }
        }

        /// <summary>
        /// Gets invoked when script gets disabled
        /// </summary>
        private void OnDisable() => SaveSettings();

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                if (hostInputField)
                {
                    hostInputField.SetTextWithoutNotify(save_game.Data.ScribblersHost);
                }
                else
                {
                    Debug.LogError("Please assign a host input field to this component.", this);
                }
                if (isUsingSecureProtocolsToggle)
                {
                    isUsingSecureProtocolsToggle.SetIsOnWithoutNotify(save_game.Data.IsUsingSecureProtocols);
                }
                else
                {
                    Debug.LogError("Please assign an is using secure protocols toggle to this component.", this);
                }
                if (usernameInputField)
                {
                    usernameInputField.SetTextWithoutNotify(save_game.Data.Username);
                }
                else
                {
                    Debug.LogError("Please assign a username input field to this component.", this);
                }
                if (lobbyLanguageDropdownController)
                {
                    lobbyLanguageDropdownController.SelectedLanguage = save_game.Data.LobbyLanguage;
                }
                else
                {
                    Debug.LogError("Please assign a lobby language dropdown controller to this component.", this);
                }
                if (drawingTimeSlider)
                {
                    drawingTimeSlider.SetValueWithoutNotify(save_game.Data.DrawingTime);
                }
                else
                {
                    Debug.LogError("Please assign a drawing time slider to this component.", this);
                }
                if (roundCountSlider)
                {
                    roundCountSlider.SetValueWithoutNotify(save_game.Data.RoundCount);
                }
                else
                {
                    Debug.LogError("Please assign a round count slider to this component.", this);
                }
                if (maximalPlayerCountSlider)
                {
                    maximalPlayerCountSlider.SetValueWithoutNotify(save_game.Data.MaximalPlayerCount);
                }
                else
                {
                    Debug.LogError("Please assign a maximal player count slider to this component.", this);
                }
                if (isLobbyPublicToggle)
                {
                    isLobbyPublicToggle.SetIsOnWithoutNotify(save_game.Data.IsLobbyPublic);
                }
                else
                {
                    Debug.LogError("Please assign an is lobby public toggle to this component.", this);
                }
                if (customWordsInputField)
                {
                    customWordsInputField.SetTextWithoutNotify(save_game.Data.CustomWords);
                }
                else
                {
                    Debug.LogError("Please assign a custom words input field to this component.", this);
                }
                if (customWordsChanceSlider)
                {
                    customWordsChanceSlider.SetValueWithoutNotify(save_game.Data.CustomWordsChance);
                }
                else
                {
                    Debug.LogError("Please assign a custom words chance slider to this component.", this);
                }
                if (playersPerIPLimitSlider)
                {
                    playersPerIPLimitSlider.SetValueWithoutNotify(save_game.Data.PlayersPerIPLimit);
                }
                else
                {
                    Debug.LogError("Please assign a players per IP limit slider to this component.", this);
                }
                if (isVotekickingEnabledToggle)
                {
                    isVotekickingEnabledToggle.SetIsOnWithoutNotify(save_game.Data.IsVotekickingEnabled);
                }
                else
                {
                    Debug.LogError("Please assign an is votekicking enabled toggle to this component.", this);
                }
            }
            TestConnection();
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (testConnectionTask != null)
            {
                elapsedTestConnectionTaskTime += Time.deltaTime;
                switch (testConnectionTask.Status)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        testConnectionTask = null;
                        if (onPingFailed != null)
                        {
                            onPingFailed.Invoke();
                        }
                        OnPingFailed?.Invoke();
                        break;
                    case TaskStatus.RanToCompletion:
                        if (testConnectionTask.Result)
                        {
                            testConnectionTask = null;
                            if (onPingSucceeded != null)
                            {
                                onPingSucceeded.Invoke(elapsedTestConnectionTaskTime);
                            }
                            OnPingSucceeded?.Invoke(elapsedTestConnectionTaskTime);
                        }
                        else
                        {
                            testConnectionTask = null;
                            if (onPingFailed != null)
                            {
                                onPingFailed.Invoke();
                            }
                            OnPingFailed?.Invoke();
                        }
                        break;
                    default:
                        if (elapsedTestConnectionTaskTime >= maximalTestConnectionTaskTime)
                        {
                            testConnectionTask = null;
                            if (onPingFailed != null)
                            {
                                onPingFailed.Invoke();
                            }
                            OnPingFailed?.Invoke();
                        }
                        break;
                }
            }
        }
    }
}
