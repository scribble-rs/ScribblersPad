using ScribblersPad.Data;
using ScribblersSharp;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnitySaveGame;
using UnitySceneLoaderManager;

/// <summary>
/// Scribble.rs Pad managers namespace
/// </summary>
namespace ScribblersPad.Managers
{
    /// <summary>
    /// A class that describes a client manager script
    /// </summary>
    public class ScribblersClientManagerScript : ASingletonManagerScript<ScribblersClientManagerScript>, IScribblersClientManager
    {
        /// <summary>
        /// Empty players
        /// </summary>
        private static readonly IReadOnlyDictionary<string, IPlayer> emptyPlayers = new Dictionary<string, IPlayer>();

        /// <summary>
        /// Gets invoked when "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "next-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNextTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "update-players" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onUpdatePlayersGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "update-wordhint" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onUpdateWordhintGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "message" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onMessageGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "non-guessing-player-message" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNonGuessingPlayerMessageGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "system-message" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onSystemMessageGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "line" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onLineGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "fill" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onFillGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "clear-drawing-board" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onClearDrawingBoardGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "your-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onYourTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "drawing" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onDrawingGameMessageReceived = default;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onUnknownGameMessageReceived = default;

        /// <summary>
        /// Scribble.rs client
        /// </summary>
        public IScribblersClient ScribblersClient { get; private set; }

        /// <summary>
        /// Lobby
        /// </summary>
        public ILobby Lobby { get; private set; }

        /// <summary>
        /// Web socket state
        /// </summary>
        public WebSocketState WebSocketState => (Lobby == null) ? WebSocketState.None : Lobby.WebSocketState;

        /// <summary>
        /// Lobby ID
        /// </summary>
        public string LobbyID => (Lobby == null) ? string.Empty : Lobby.LobbyID;

        /// <summary>
        /// Drawing board base width
        /// </summary>
        public uint DrawingBoardBaseWidth => (Lobby == null) ? 0U : Lobby.DrawingBoardBaseWidth;

        /// <summary>
        /// Drawing board base height
        /// </summary>
        public uint DrawingBoardBaseHeight => (Lobby == null) ? 0U : Lobby.DrawingBoardBaseHeight;

        /// <summary>
        /// My player in lobby
        /// </summary>
        public IPlayer MyPlayer => Lobby?.MyPlayer;

        /// <summary>
        /// Is my player allowed to draw
        /// </summary>
        public bool IsPlayerAllowedToDraw => (Lobby != null) && Lobby.IsPlayerAllowedToDraw;

        /// <summary>
        /// Lobby owner
        /// </summary>
        public IPlayer Owner => Lobby?.Owner;

        /// <summary>
        /// Current round
        /// </summary>
        public uint Round => (Lobby == null) ? 0U : Lobby.Round;

        /// <summary>
        /// Maximal amount of rounds
        /// </summary>
        public uint MaximalRounds => (Lobby == null) ? 0U : Lobby.MaximalRounds;

        /// <summary>
        /// Round end time
        /// </summary>
        public long RoundEndTime => (Lobby == null) ? 0L : Lobby.RoundEndTime;

        /// <summary>
        /// Word hints
        /// </summary>
        public IReadOnlyList<IWordHint> WordHints => (Lobby == null) ? Array.Empty<IWordHint>() : Lobby.WordHints;

        /// <summary>
        /// Lobby players
        /// </summary>
        public IReadOnlyDictionary<string, IPlayer> Players => (Lobby == null) ? emptyPlayers : Lobby.Players;

        /// <summary>
        /// Current drawing
        /// </summary>
        public IReadOnlyList<ScribblersSharp.IDrawCommand> CurrentDrawing => (Lobby == null) ? Array.Empty<ScribblersSharp.IDrawCommand>() : Lobby.CurrentDrawing;

        /// <summary>
        /// Game state
        /// </summary>
        public EGameState GameState => (Lobby == null) ? EGameState.Unknown : Lobby.GameState;

        /// <summary>
        /// Gets invoked when "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when "update-players" game message has been received
        /// </summary>
        public event UpdatePlayersGameMessageReceivedDelegate OnUpdatePlayersGameMessageReceived;

        /// <summary>
        /// Gets invoked when "update-wordhint" game message has been received
        /// </summary>
        public event UpdateWordhintGameMessageReceivedDelegate OnUpdateWordhintGameMessageReceived;

        /// <summary>
        /// Gets invoked when "message" game message has been received
        /// </summary>
        public event MessageGameMessageReceivedDelegate OnMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "non-guessing-player-message" game message has been received
        /// </summary>
        public event NonGuessingPlayerMessageGameMessageReceivedDelegate OnNonGuessingPlayerMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "system-message" game message has been received
        /// </summary>
        public event SystemMessageGameMessageReceivedDelegate OnSystemMessageGameMessageReceived;

        /// <summary>
        /// Gets invoked when "line" game message has been received
        /// </summary>
        public event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when "fill" game message has been received
        /// </summary>
        public event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when "clear-drawing-board" game message has been received
        /// </summary>
        public event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when "your-turn" game message has been received
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when "drawing" game message has been received
        /// </summary>
        public event DrawingGameMessageReceivedDelegate OnDrawingGameMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful game message has been received.
        /// </summary>
        public event UnknownGameMessageReceivedDelegate OnUnknownGameMessageReceived;

        /// <summary>
        /// Subcribes to lobby events
        /// </summary>
        private void SubscribeLobbyEvents()
        {
            if (Lobby != null)
            {
                Lobby.OnReadyGameMessageReceived += LobbyReadyGameMessageReceivedEvent;
                Lobby.OnNextTurnGameMessageReceived += LobbyNextTurnGameMessageReceivedEvent;
                Lobby.OnUpdatePlayersGameMessageReceived += LobbyUpdatePlayersGameMessageReceivedEvent;
                Lobby.OnUpdateWordhintGameMessageReceived += LobbyUpdateWordhintGameMessageReceivedEvent;
                Lobby.OnMessageGameMessageReceived += LobbyMessageGameMessageReceivedEvent;
                Lobby.OnNonGuessingPlayerMessageGameMessageReceived += LobbyNonGuessingPlayerMessageGameMessageReceivedEvent;
                Lobby.OnSystemMessageGameMessageReceived += LobbySystemMessageGameMessageReceivedEvent;
                Lobby.OnLineGameMessageReceived += LobbyLineGameMessageReceivedEvent;
                Lobby.OnFillGameMessageReceived += LobbyFillGameMessageReceivedEvent;
                Lobby.OnClearDrawingBoardGameMessageReceived += LobbyClearDrawingBoardGameMessageReceivedEvent;
                Lobby.OnYourTurnGameMessageReceived += LobbyYourTurnGameMessageReceivedEvent;
                Lobby.OnCorrectGuessGameMessageReceived += LobbyCorrectGuessGameMessageReceivedEvent;
                Lobby.OnDrawingGameMessageReceived += LobbyDrawingGameMessageReceivedEvent;
                Lobby.OnUnknownGameMessageReceived += LobbyUnknownGameMessageReceivedEvent;
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void LobbyReadyGameMessageReceivedEvent(ILobby lobby)
        {
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        /// <param name="lobby">Lobby</param>
        private void LobbyNextTurnGameMessageReceivedEvent(ILobby lobby)
        {
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke(lobby);
        }

        /// <summary>
        /// Gets invoked when a "update-players" game message has been received
        /// </summary>
        /// <param name="players">Players</param>
        private void LobbyUpdatePlayersGameMessageReceivedEvent(IReadOnlyDictionary<string, IPlayer> players)
        {
            if (onUpdatePlayersGameMessageReceived != null)
            {
                onUpdatePlayersGameMessageReceived.Invoke();
            }
            OnUpdatePlayersGameMessageReceived?.Invoke(players);
        }

        /// <summary>
        /// Gets invoked when a "update-wordhint" game message has been received
        /// </summary>
        /// <param name="wordHints">Word hints</param>
        private void LobbyUpdateWordhintGameMessageReceivedEvent(IReadOnlyList<IWordHint> wordHints)
        {
            if (onUpdateWordhintGameMessageReceived != null)
            {
                onUpdateWordhintGameMessageReceived.Invoke();
            }
            OnUpdateWordhintGameMessageReceived?.Invoke(wordHints);
        }

        /// <summary>
        /// Gets invoked when a "message" game message has been received
        /// </summary>
        /// <param name="author">Message author</param>
        /// <param name="content">Message content</param>
        private void LobbyMessageGameMessageReceivedEvent(IPlayer author, string content)
        {
            if (onMessageGameMessageReceived != null)
            {
                onMessageGameMessageReceived.Invoke();
            }
            OnMessageGameMessageReceived?.Invoke(author, content);
        }

        /// <summary>
        /// Gets invoked when a "non-guessing-player-message" game message has been received
        /// </summary>
        /// <param name="author">Message author</param>
        /// <param name="content">Message content</param>
        private void LobbyNonGuessingPlayerMessageGameMessageReceivedEvent(IPlayer author, string content)
        {
            if (onNonGuessingPlayerMessageGameMessageReceived != null)
            {
                onNonGuessingPlayerMessageGameMessageReceived.Invoke();
            }
            OnNonGuessingPlayerMessageGameMessageReceived?.Invoke(author, content);
        }

        /// <summary>
        /// Gets invoked when a "system-message" game message has been received
        /// </summary>
        /// <param name="content">Message content</param>
        private void LobbySystemMessageGameMessageReceivedEvent(string content)
        {
            if (onSystemMessageGameMessageReceived != null)
            {
                onSystemMessageGameMessageReceived.Invoke();
            }
            OnSystemMessageGameMessageReceived?.Invoke(content);
        }

        /// <summary>
        /// Gets invoked when a "line" game message has been received
        /// </summary>
        /// <param name="fromX">X component of start line position</param>
        /// <param name="fromY">Y component of start line position</param>
        /// <param name="toX">X component of end line position</param>
        /// <param name="toY">Y component of end line position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        private void LobbyLineGameMessageReceivedEvent(float fromX, float fromY, float toX, float toY, System.Drawing.Color color, float lineWidth)
        {
            if (onLineGameMessageReceived != null)
            {
                onLineGameMessageReceived.Invoke();
            }
            OnLineGameMessageReceived?.Invoke(fromX, fromY, toX, toY, color, lineWidth);
        }

        /// <summary>
        /// Gets invoked when a "fill" game message has been received
        /// </summary>
        /// <param name="positionX">X component of fill position</param>
        /// <param name="positionY">Y component of fill position</param>
        /// <param name="color">Color</param>
        private void LobbyFillGameMessageReceivedEvent(float positionX, float positionY, System.Drawing.Color color)
        {
            if (onFillGameMessageReceived != null)
            {
                onFillGameMessageReceived.Invoke();
            }
            OnFillGameMessageReceived?.Invoke(positionX, positionY, color);
        }

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received
        /// </summary>
        private void LobbyClearDrawingBoardGameMessageReceivedEvent()
        {
            if (onClearDrawingBoardGameMessageReceived != null)
            {
                onClearDrawingBoardGameMessageReceived.Invoke();
            }
            OnClearDrawingBoardGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when "your-turn" game message has been received
        /// </summary>
        private void LobbyYourTurnGameMessageReceivedEvent(IReadOnlyList<string> words)
        {
            if (onYourTurnGameMessageReceived != null)
            {
                onYourTurnGameMessageReceived.Invoke();
            }
            OnYourTurnGameMessageReceived?.Invoke(words);
        }

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        private void LobbyCorrectGuessGameMessageReceivedEvent(IPlayer player)
        {
            if (onCorrectGuessGameMessageReceived != null)
            {
                onCorrectGuessGameMessageReceived.Invoke();
            }
            OnCorrectGuessGameMessageReceived?.Invoke(player);
        }

        /// <summary>
        /// Gets invoked when "drawing" game message has been received
        /// </summary>
        private void LobbyDrawingGameMessageReceivedEvent(IReadOnlyList<ScribblersSharp.IDrawCommand> drawCommands)
        {
            if (onDrawingGameMessageReceived != null)
            {
                onDrawingGameMessageReceived.Invoke();
            }
            OnDrawingGameMessageReceived?.Invoke(drawCommands);
        }

        /// <summary>
        /// Gets invoked when a non-meaningful game message has been received.
        /// </summary>
        private void LobbyUnknownGameMessageReceivedEvent(IBaseGameMessageData gameMessage, string json)
        {
            if (onUnknownGameMessageReceived != null)
            {
                onUnknownGameMessageReceived.Invoke();
            }
            OnUnknownGameMessageReceived?.Invoke(gameMessage, json);
        }

        /// <summary>
        /// Creates a new client
        /// </summary>
        public void CreateClient()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                DestroyClient();
                ScribblersClient = Clients.Create(save_game.Data.ScribblersHost);
            }
        }

        /// <summary>
        /// Destroys current client
        /// </summary>
        public void DestroyClient()
        {
            LeaveLobby();
            if (ScribblersClient != null)
            {
                ScribblersClient.Dispose();
                ScribblersClient = null;
            }
        }

        /// <summary>
        /// Leaves lobby
        /// </summary>
        public void LeaveLobby()
        {
            if (Lobby != null)
            {
                Lobby.Dispose();
                Lobby.OnReadyGameMessageReceived -= LobbyReadyGameMessageReceivedEvent;
                Lobby.OnNextTurnGameMessageReceived -= LobbyNextTurnGameMessageReceivedEvent;
                Lobby.OnUpdatePlayersGameMessageReceived -= LobbyUpdatePlayersGameMessageReceivedEvent;
                Lobby.OnUpdateWordhintGameMessageReceived -= LobbyUpdateWordhintGameMessageReceivedEvent;
                Lobby.OnMessageGameMessageReceived -= LobbyMessageGameMessageReceivedEvent;
                Lobby.OnNonGuessingPlayerMessageGameMessageReceived -= LobbyNonGuessingPlayerMessageGameMessageReceivedEvent;
                Lobby.OnSystemMessageGameMessageReceived -= LobbySystemMessageGameMessageReceivedEvent;
                Lobby.OnLineGameMessageReceived -= LobbyLineGameMessageReceivedEvent;
                Lobby.OnFillGameMessageReceived -= LobbyFillGameMessageReceivedEvent;
                Lobby.OnClearDrawingBoardGameMessageReceived -= LobbyClearDrawingBoardGameMessageReceivedEvent;
                Lobby.OnYourTurnGameMessageReceived -= LobbyYourTurnGameMessageReceivedEvent;
                Lobby.OnCorrectGuessGameMessageReceived -= LobbyCorrectGuessGameMessageReceivedEvent;
                Lobby.OnDrawingGameMessageReceived -= LobbyDrawingGameMessageReceivedEvent;
                Lobby.OnUnknownGameMessageReceived -= LobbyUnknownGameMessageReceivedEvent;
                Lobby = null;
            }
        }

        /// <summary>
        /// Enters a lobby asynchronously
        /// </summary>
        /// <param name="lobbyID">Lobby ID</param>
        /// <param name="username">Username</param>
        /// <returns>"true" if successful, otherwise "false" as a task</returns>
        public async Task<bool> EnterLobbyAsync(string lobbyID, string username)
        {
            bool ret = false;
            if (ScribblersClient != null)
            {
                LeaveLobby();
                Lobby = await ScribblersClient.EnterLobbyAsync(lobbyID, username);
                ret = Lobby != null;
                SubscribeLobbyEvents();
            }
            return ret;
        }

        /// <summary>
        /// Creates a lobby asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is public</param>
        /// <param name="maximalPlayers">Maximal amount of players allowed</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="rounds">Maximal amout of rounds</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom word chance</param>
        /// <param name="isVotekickEnabled">Is votekick enabled</param>
        /// <param name="clientsPerIPLimit">Allowed clients per IP limit</param>
        /// <returns>"true" if successful, otherwise "false" as a task</returns>
        public async Task<bool> CreateLobbyAsync(string username, ELanguage language, bool isPublic, uint maximalPlayers, ulong drawingTime, uint rounds, IReadOnlyList<string> customWords, uint customWordsChance, bool isVotekickEnabled, uint clientsPerIPLimit)
        {
            bool ret = false;
            if (ScribblersClient != null)
            {
                LeaveLobby();
                Lobby = await ScribblersClient.CreateLobbyAsync(username, language, isPublic, maximalPlayers, drawingTime, rounds, customWords, customWordsChance, isVotekickEnabled, clientsPerIPLimit);
                ret = Lobby != null;
                SubscribeLobbyEvents();
            }
            return ret;
        }
        
        /// <summary>
        /// Sends a "start-game" message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendStartGameMessageAsync() => (Lobby == null) ? Task.CompletedTask : Lobby.SendStartGameMessageAsync();

        /// <summary>
        /// Sends a "clear-drawing-board" game message asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public Task SendClearDrawingBoardGameMessageAsync() => (Lobby == null) ? Task.CompletedTask : Lobby.SendClearDrawingBoardGameMessageAsync();

        /// <summary>
        /// Sends a "fill" game message asynchronously
        /// </summary>
        /// <param name="positionX">X component of fill start posiiton</param>
        /// <param name="positionY">Y component of fill start position</param>
        /// <param name="color"></param>
        /// <returns>Task</returns>
        public Task SendFillGameMessageAsync(float positionX, float positionY, System.Drawing.Color color) => (Lobby == null) ? Task.CompletedTask : Lobby.SendFillGameMessageAsync(positionX, positionY, color);

        /// <summary>
        /// Sends a "line" game message asynchronously
        /// </summary>
        /// <param name="fromX">X component of start line position</param>
        /// <param name="fromY">Y component of start line position</param>
        /// <param name="toX">X component of end line position</param>
        /// <param name="toY">Y component of end line position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width in pixels</param>
        /// <returns>Task</returns>
        public Task SendLineGameMessageAsync(float fromX, float fromY, float toX, float toY, System.Drawing.Color color, float lineWidth) => (Lobby == null) ? Task.CompletedTask : Lobby.SendLineGameMessageAsync(fromX, fromY, toX, toY, color, lineWidth);

        /// <summary>
        /// Sends a "choose-word" game message asynchronously
        /// </summary>
        /// <param name="index">Word index</param>
        /// <returns>Task</returns>
        public Task SendChooseWordGameMessageAsync(uint index) => (Lobby == null) ? Task.CompletedTask : Lobby.SendChooseWordGameMessageAsync(index);

        /// <summary>
        /// Sends a "message" game message asynchronously
        /// </summary>
        /// <param name="content">Message content</param>
        /// <returns>Task</returns>
        public Task SendMessageGameMessageAsync(string content) => (Lobby == null) ? Task.CompletedTask : Lobby.SendMessageGameMessageAsync(content);

        /// <summary>
        /// Gets invoked when script gets enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            if (Instance == this)
            {
                CreateClient();
            }
        }

        /// <summary>
        /// Gets invoked when script gets disabled
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            DestroyClient();
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private async void Start()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game == null)
            {
                SceneLoaderManager.LoadScene("MainMenuScene");
            }
            else
            {
                string username = save_game.Data.ScribblersUsername.Trim();
                if (username.Length > Rules.maximalUsernameLength)
                {
                    username = username.Substring(0, (int)Rules.maximalUsernameLength);
                }
                if (string.IsNullOrWhiteSpace(save_game.Data.ScribblersLobbyID.Trim()))
                {
                    if (!await CreateLobbyAsync(username, ELanguage.English, true, Rules.maximalPlayers, Rules.maximalDrawingTime, Rules.maximalRounds, Array.Empty<string>(), Rules.minimalCustomWordsChance, false, Rules.maximalClientsPerIPLimit))
                    {
                        SceneLoaderManager.LoadScene("MainMenuScene");
                    }
                }
                else if (!await EnterLobbyAsync(save_game.Data.ScribblersLobbyID.Trim(), username))
                {
                    SceneLoaderManager.LoadScene("MainMenuScene");
                }
            }
        }

        /// <summary>
        /// Gets invoked when script perfoms a fixed update
        /// </summary>
        private void FixedUpdate() => Lobby?.ProcessEvents();
    }
}
