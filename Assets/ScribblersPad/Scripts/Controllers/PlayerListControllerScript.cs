using ScribblersSharp;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class taht describes a player list controller script
    /// </summary>
    public class PlayerListControllerScript : AScribblersClientControllerScript, IPlayerListController
    {
        /// <summary>
        /// Player list element asset
        /// </summary>
        [SerializeField]
        private GameObject playerListElementAsset = default;

        /// <summary>
        /// Own player list element asset
        /// </summary>
        [SerializeField]
        private GameObject ownPlayerListElementAsset = default;

        /// <summary>
        /// Player list element parent rectangle transform
        /// </summary>
        [SerializeField]
        private RectTransform playerListElementParentRectangleTransform = default;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNameChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived;

        /// <summary>
        /// Player list element controllers
        /// </summary>
        private readonly Dictionary<string, (RectTransform, PlayerListElementControllerScript)> playerListElementControllers = new Dictionary<string, (RectTransform, PlayerListElementControllerScript)>();

        /// <summary>
        /// Player list element asset
        /// </summary>
        public GameObject PlayerListElementAsset
        {
            get => playerListElementAsset;
            set => playerListElementAsset = value;
        }

        /// <summary>
        /// Own player list element asset
        /// </summary>
        public GameObject OwnPlayerListElementAsset
        {
            get => ownPlayerListElementAsset;
            set => ownPlayerListElementAsset = value;
        }

        /// <summary>
        /// Player list element parent rectangle transform
        /// </summary>
        public RectTransform PlayerListElementParentRectangleTransform
        {
            get => playerListElementParentRectangleTransform;
            set => playerListElementParentRectangleTransform = value;
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        public event NameChangeGameMessageReceivedDelegate OnNameChangeGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Updates player
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="lobby">Lobby</param>
        /// <returns>"true" if player has been successfully updated, otherwise "false"</returns>
        private bool UpdatePlayer(IPlayer player, ILobby lobby)
        {
            bool ret = false;
            if (player != null)
            {
                ret = true;
                if (playerListElementControllers.ContainsKey(player.ID))
                {
                    playerListElementControllers[player.ID].Item2.SetValues(player);
                }
                else
                {
                    GameObject game_object = Instantiate((player.ID == lobby.MyPlayer.ID) ? ownPlayerListElementAsset : playerListElementAsset);
                    if (game_object)
                    {
                        if (game_object.TryGetComponent(out RectTransform rectangle_transform) && game_object.TryGetComponent(out PlayerListElementControllerScript player_list_element_controller))
                        {
                            rectangle_transform.SetParent(playerListElementParentRectangleTransform, false);
                            player_list_element_controller.SetValues(player);
                            playerListElementControllers.Add(player.ID, (rectangle_transform, player_list_element_controller));
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to instantiate a player list element asset.", this);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Updates players
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void UpdatePlayers(ILobby lobby)
        {
            if (lobby == null)
            {
                throw new ArgumentNullException(nameof(lobby));
            }
            if (playerListElementParentRectangleTransform)
            {
                HashSet<string> remove_players = new HashSet<string>(playerListElementControllers.Keys);
                foreach (IPlayer player in lobby.Players.Values)
                {
                    if (UpdatePlayer(player, lobby))
                    {
                        remove_players.Remove(player.ID);
                    }
                }
                foreach (string remove_player in remove_players)
                {
                    playerListElementControllers.Remove(remove_player);
                }
                remove_players.Clear();
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            UpdatePlayers(ScribblersClientManager.Lobby);
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        private void ScribblersClientManagerNextTurnGameMessageReceivedEvent()
        {
            UpdatePlayers(ScribblersClientManager.Lobby);
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        private void ScribblersClientManagerNameChangeGameMessageReceivedEvent(IPlayer player)
        {
            UpdatePlayer(player, ScribblersClientManager.Lobby);
            if (onNameChangeGameMessageReceived != null)
            {
                onNameChangeGameMessageReceived.Invoke();
            }
            OnNameChangeGameMessageReceived?.Invoke(player);
        }

        /// <summary>
        /// Gets invoked when a "update-player" game message has been received
        /// </summary>
        /// <param name="players">Players</param>
        private void ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent(IReadOnlyDictionary<string, IPlayer> players)
        {
            UpdatePlayers(ScribblersClientManager.Lobby);
            if (onUpdatePlayersGameMessageReceived != null)
            {
                onUpdatePlayersGameMessageReceived.Invoke();
            }
            OnUpdatePlayersGameMessageReceived?.Invoke(players);
        }

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerCorrectGuessGameMessageReceivedEvent(IPlayer player)
        {
            UpdatePlayer(player, ScribblersClientManager.Lobby);
            if (onCorrectGuessGameMessageReceived != null)
            {
                onCorrectGuessGameMessageReceived.Invoke();
            }
            OnCorrectGuessGameMessageReceived?.Invoke(player);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived += ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnNameChangeGameMessageReceived += ScribblersClientManagerNameChangeGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdatePlayersGameMessageReceived += ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived += ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnNameChangeGameMessageReceived -= ScribblersClientManagerNameChangeGameMessageReceivedEvent;
            ScribblersClientManager.OnUpdatePlayersGameMessageReceived -= ScribblersClientManagerUpdatePlayersGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived -= ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            if (playerListElementAsset && !playerListElementAsset.TryGetComponent(out PlayerListElementControllerScript _))
            {
                playerListElementAsset = null;
            }
            if (ownPlayerListElementAsset && !ownPlayerListElementAsset.TryGetComponent(out PlayerListElementControllerScript _))
            {
                ownPlayerListElementAsset = null;
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (!playerListElementAsset)
            {
                Debug.LogError("Please assign a player list element asset reference to this component.", this);
            }
            if (!ownPlayerListElementAsset)
            {
                Debug.LogError("Please assign an own player list element asset reference to this component.", this);
            }
            if (!playerListElementParentRectangleTransform)
            {
                Debug.LogError("Please assign a player list element parent rectangle transform reference to this component.", this);
            }
        }
    }
}
