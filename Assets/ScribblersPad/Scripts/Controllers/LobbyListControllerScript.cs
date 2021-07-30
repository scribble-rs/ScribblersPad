using ScribblersPad.Data;
using ScribblersPad.Objects;
using ScribblersSharp;
using System;
using System.Collections.Generic;
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
    [RequireComponent(typeof(RectTransform))]
    public class LobbyListControllerScript : MonoBehaviour, ILobbyListController
    {
        /// <summary>
        /// Lobby panel asset
        /// </summary>
        [SerializeField]
        private GameObject lobbyPanelAsset = default;

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
        /// Is allowed to use insecure protocols toggle
        /// </summary>
        [SerializeField]
        private Toggle isAllowedToUseInsecureProtocolsToggle = default;

        /// <summary>
        /// Gets invoked when listing lobbies has been requested
        /// </summary>
        [SerializeField]
        private UnityEvent onListLobbyRequested = default;

        /// <summary>
        /// Gets invoked when listing lobbies has failed
        /// </summary>
        [SerializeField]
        private UnityEvent onListLobbyRequestFailed = default;

        /// <summary>
        /// Gets invoked when listing lobbies has been finished
        /// </summary>
        [SerializeField]
        private UnityEvent onListLobbyRequestFinished = default;

        /// <summary>
        /// Gets invoked when listing lobbies has been finished from an insecure source
        /// </summary>
        [SerializeField]
        private UnityEvent onInsecureListLobbyRequestFinished = default;

        /// <summary>
        /// Lobby view panel controllers
        /// </summary>
        private readonly Dictionary<string, LobbyPanelControllerScript> lobbyViewPanelControllers = new Dictionary<string, LobbyPanelControllerScript>();

        /// <summary>
        /// Remove lobby view panel controller keys
        /// </summary>
        private readonly HashSet<string> removeLobbyViewPanelControllerKeys = new HashSet<string>();

        /// <summary>
        /// Scribble.rs client
        /// </summary>
        private IScribblersClient scribblersClient;

        /// <summary>
        /// List lobbies task
        /// </summary>
        private Task<ILobbyViews> listLobbiesTask;

        /// <summary>
        /// Lobby views
        /// </summary>
        private IEnumerable<ILobbyView> lobbyViews = Array.Empty<ILobbyView>();

        /// <summary>
        /// Lobby panel asset
        /// </summary>
        public GameObject LobbyPanelAsset
        {
            get => lobbyPanelAsset;
            set => lobbyPanelAsset = value;
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
        /// Is allowed to use insecure protocols toggle
        /// </summary>
        public Toggle IsAllowedToUseInsecureProtocolsToggle
        {
            get => isAllowedToUseInsecureProtocolsToggle;
            set => isAllowedToUseInsecureProtocolsToggle = value;
        }

        /// <summary>
        /// Gets invoked when listing lobbies has been requested
        /// </summary>
        public event ListLobbyRequestedDelegate OnListLobbyRequested;

        /// <summary>
        /// Gets invoked when listing lobbies has failed
        /// </summary>
        public event ListLobbyRequestFailedDelegate OnListLobbyRequestFailed;

        /// <summary>
        /// Gets invoked when listing lobbies has been finished
        /// </summary>
        public event ListLobbyRequestFinishedDelegate OnListLobbyRequestFinished;

        /// <summary>
        /// Gets invoked when listing lobbies has been finished from an insecure source
        /// </summary>
        public event InsecureListLobbyRequestFinishedDelegate OnInsecureListLobbyRequestFinished;

        /// <summary>
        /// Enables Scribble.rs client
        /// </summary>
        private void EnableScribblersClient()
        {
            if (scribblersClient == null)
            {
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    scribblersClient = Clients.Create(save_game.Data.ScribblersHost, save_game.Data.GetUserSessionID(save_game.Data.ScribblersHost), save_game.Data.IsUsingSecureProtocols, save_game.Data.IsAllowedToUseInsecureProtocols);
                }
            }
        }

        /// <summary>
        /// Disables Scribble.rs client
        /// </summary>
        private void DisableScribblersClient()
        {
            if (listLobbiesTask != null)
            {
                listLobbiesTask = null;
                if (onListLobbyRequestFailed != null)
                {
                    onListLobbyRequestFailed.Invoke();
                }
                OnListLobbyRequestFailed?.Invoke();
            }
            if (scribblersClient != null)
            {
                scribblersClient.Dispose();
                scribblersClient = null;
            }
        }

        /// <summary>
        /// Creates a new lobby panel
        /// </summary>
        /// <param name="lobbyView">Lobby view</param>
        private void CreateNewLobbyViewPanel(ILobbyView lobbyView)
        {
            GameObject lobby_view_game_object = Instantiate(lobbyPanelAsset, transform);
            if (lobby_view_game_object.TryGetComponent(out LobbyPanelControllerScript lobby_panel_controller))
            {
                lobby_panel_controller.SetValues(lobbyView);
                lobbyViewPanelControllers.Add(lobbyView.LobbyID, lobby_panel_controller);
            }
        }

        /// <summary>
        /// Updates visuals
        /// </summary>
        private void UpdateVisuals()
        {
            if (lobbyPanelAsset)
            {
                removeLobbyViewPanelControllerKeys.UnionWith(lobbyViewPanelControllers.Keys);
                foreach (ILobbyView lobby_view in lobbyViews)
                {
                    if (lobbyViewPanelControllers.ContainsKey(lobby_view.LobbyID))
                    {
                        LobbyPanelControllerScript lobby_panel_controller = lobbyViewPanelControllers[lobby_view.LobbyID];
                        if (lobby_panel_controller)
                        {
                            lobby_panel_controller.SetValues(lobby_view);
                        }
                        else
                        {
                            lobbyViewPanelControllers.Remove(lobby_view.LobbyID);
                            CreateNewLobbyViewPanel(lobby_view);
                        }
                        removeLobbyViewPanelControllerKeys.Remove(lobby_view.LobbyID);
                    }
                    else
                    {
                        CreateNewLobbyViewPanel(lobby_view);
                    }
                }
                foreach (string remove_lobby_view_panel_controller_key in removeLobbyViewPanelControllerKeys)
                {
                    LobbyPanelControllerScript lobby_panel_controller = lobbyViewPanelControllers[remove_lobby_view_panel_controller_key];
                    if (lobby_panel_controller)
                    {
                        Destroy(lobby_panel_controller.gameObject);
                    }
                    lobbyViewPanelControllers.Remove(remove_lobby_view_panel_controller_key);
                }
                removeLobbyViewPanelControllerKeys.Clear();
            }
        }

        /// <summary>
        /// Gets invoked when host input field editing has been ended
        /// </summary>
        /// <param name="newValue">New value</param>
        private void HostInputFieldEndEditEvent(string newValue)
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                DisableScribblersClient();
                listLobbiesTask = null;
                save_game.Data.ScribblersHost = newValue.Trim();
                EnableScribblersClient();
                ListLobbies();
            }
        }

        /// <summary>
        /// Gets invoked when is using secure protocols toggle value has been changed
        /// </summary>
        /// <param name="newValue">New value</param>
        private void IsUsingSecureProtocolsToggleValueChangedEvent(bool newValue)
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                DisableScribblersClient();
                listLobbiesTask = null;
                save_game.Data.IsUsingSecureProtocols = newValue;
                EnableScribblersClient();
                ListLobbies();
            }
        }

        /// <summary>
        /// Gets invoked when is allowed to use insecure protocols toggle value has been changed
        /// </summary>
        /// <param name="newValue">New value</param>
        private void IsAllowedToUseInsecureProtocolsToggleValueChangedEvent(bool newValue)
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game != null)
            {
                DisableScribblersClient();
                listLobbiesTask = null;
                save_game.Data.IsAllowedToUseInsecureProtocols = newValue;
                EnableScribblersClient();
                ListLobbies();
            }
        }

        /// <summary>
        /// Lists lobbies
        /// </summary>
        public void ListLobbies()
        {
            if ((scribblersClient != null) && (listLobbiesTask == null))
            {
                listLobbiesTask = scribblersClient.ListLobbiesAsync();
                if (onListLobbyRequested != null)
                {
                    onListLobbyRequested.Invoke();
                }
                OnListLobbyRequested?.Invoke();
            }
        }

        /// <summary>
        /// Resets host to default
        /// </summary>
        public void ResetHostToDefault()
        {
            if (hostInputField && isUsingSecureProtocolsToggle && isAllowedToUseInsecureProtocolsToggle)
            {
                hostInputField.SetTextWithoutNotify(ScribblersDefaultsObjectScript.defaultHost);
                isUsingSecureProtocolsToggle.SetIsOnWithoutNotify(true);
                isAllowedToUseInsecureProtocolsToggle.SetIsOnWithoutNotify(false);
                isAllowedToUseInsecureProtocolsToggle.interactable = true;
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    DisableScribblersClient();
                    listLobbiesTask = null;
                    save_game.Data.ScribblersHost = ScribblersDefaultsObjectScript.defaultHost;
                    save_game.Data.IsUsingSecureProtocols = true;
                    EnableScribblersClient();
                    ListLobbies();
                }
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (lobbyPanelAsset && !lobbyPanelAsset.GetComponent<LobbyPanelControllerScript>())
            {
                lobbyPanelAsset = null;
            }
        }

        /// <summary>
        /// Gets invoked when script gets enabled
        /// </summary>
        private void OnEnable()
        {
            EnableScribblersClient();
            if (hostInputField)
            {
                hostInputField.onEndEdit.AddListener(HostInputFieldEndEditEvent);
            }
            if (isUsingSecureProtocolsToggle)
            {
                isUsingSecureProtocolsToggle.onValueChanged.AddListener(IsUsingSecureProtocolsToggleValueChangedEvent);
            }
            if (isAllowedToUseInsecureProtocolsToggle)
            {
                isAllowedToUseInsecureProtocolsToggle.onValueChanged.AddListener(IsAllowedToUseInsecureProtocolsToggleValueChangedEvent);
            }
        }

        /// <summary>
        /// Gets invoked when script gets disabled
        /// </summary>
        private void OnDisable()
        {
            DisableScribblersClient();
            if (hostInputField)
            {
                hostInputField.onEndEdit.RemoveListener(HostInputFieldEndEditEvent);
            }
            if (isUsingSecureProtocolsToggle)
            {
                isUsingSecureProtocolsToggle.onValueChanged.RemoveListener(IsUsingSecureProtocolsToggleValueChangedEvent);
            }
            if (isAllowedToUseInsecureProtocolsToggle)
            {
                isAllowedToUseInsecureProtocolsToggle.onValueChanged.RemoveListener(IsAllowedToUseInsecureProtocolsToggleValueChangedEvent);
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (!lobbyPanelAsset)
            {
                Debug.LogError("Please assign a lobby panel asset to this component.", this);
            }
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
                if (isAllowedToUseInsecureProtocolsToggle)
                {
                    isAllowedToUseInsecureProtocolsToggle.SetIsOnWithoutNotify(save_game.Data.IsAllowedToUseInsecureProtocols);
                    isAllowedToUseInsecureProtocolsToggle.interactable = save_game.Data.IsUsingSecureProtocols;
                }
                else
                {
                    Debug.LogError("Please assign an is allowed to use insecure protocols toggle to this component.", this);
                }
            }
            ListLobbies();
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (listLobbiesTask != null)
            {
                switch (listLobbiesTask.Status)
                {
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        listLobbiesTask = null;
                        if (onListLobbyRequestFailed != null)
                        {
                            onListLobbyRequestFailed.Invoke();
                        }
                        OnListLobbyRequestFailed?.Invoke();
                        break;
                    case TaskStatus.RanToCompletion:
                        ILobbyViews lobby_views = listLobbiesTask.Result;
                        if (lobby_views == null)
                        {
                            listLobbiesTask = null;
                            if (onListLobbyRequestFailed != null)
                            {
                                onListLobbyRequestFailed.Invoke();
                            }
                            OnListLobbyRequestFailed?.Invoke();
                        }
                        else
                        {
                            listLobbiesTask = null;
                            lobbyViews = lobby_views;
                            UpdateVisuals();
                            if (lobby_views.IsConnectionSecure)
                            {
                                if (onListLobbyRequestFinished != null)
                                {
                                    onListLobbyRequestFinished.Invoke();
                                }
                                OnListLobbyRequestFinished?.Invoke(lobbyViews);
                            }
                            else
                            {
                                if (onInsecureListLobbyRequestFinished != null)
                                {
                                    onInsecureListLobbyRequestFinished.Invoke();
                                }
                                OnInsecureListLobbyRequestFinished?.Invoke(lobbyViews);
                            }
                        }
                        break;
                }
            }
        }
    }
}
