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
        /// Gets invoked when a "name-change" game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onNameChangeGameMessageReceived = default;

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
        /// Gets invoked when a "close-guess" game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onCloseGuessGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "kick-vote" game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onKickVoteGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "drawer-kicked" game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onDrawerKickedGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "owner-change" game message has been received.
        /// </summary>
        [SerializeField]
        private UnityEvent onOwnerChangeGameMessageReceived = default;

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
        /// Lobby limits
        /// </summary>
        public ILobbyLimits Limits => Lobby?.Limits;

        /// <summary>
        /// Maximal player count
        /// </summary>
        public uint MaximalPlayerCount => (Lobby == null) ? 0U : Lobby.MaximalPlayerCount;

        /// <summary>
        /// Is lobby public
        /// </summary>
        public bool IsLobbyPublic => (Lobby != null) && Lobby.IsLobbyPublic;

        /// <summary>
        /// Is votekicking enabled
        /// </summary>
        public bool IsVotekickingEnabled => (Lobby != null) && Lobby.IsVotekickingEnabled;

        /// <summary>
        /// Custom words chance
        /// </summary>
        public uint CustomWordsChance => (Lobby == null) ? 0U : Lobby.CustomWordsChance;

        /// <summary>
        /// Allowed clients per IP
        /// </summary>
        public uint AllowedClientsPerIPCount => (Lobby == null) ? 0U : Lobby.AllowedClientsPerIPCount;

        /// <summary>
        /// Drawing board base width
        /// </summary>
        public uint DrawingBoardBaseWidth => (Lobby == null) ? 0U : Lobby.DrawingBoardBaseWidth;

        /// <summary>
        /// Drawing board base height
        /// </summary>
        public uint DrawingBoardBaseHeight => (Lobby == null) ? 0U : Lobby.DrawingBoardBaseHeight;

        /// <summary>
        /// Suggested brush sizes
        /// </summary>
        public IEnumerable<uint> SuggestedBrushSizes => (Lobby == null) ? Array.Empty<uint>() : Lobby.SuggestedBrushSizes;

        /// <summary>
        /// Canvas color
        /// </summary>
        public Color32 CanvasColor { get; private set; } = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

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
        public uint CurrentRound => (Lobby == null) ? 0U : Lobby.CurrentRound;

        /// <summary>
        /// Current maximal round count
        /// </summary>
        public uint CurrentMaximalRoundCount => (Lobby == null) ? 0U : Lobby.CurrentMaximalRoundCount;

        /// <summary>
        /// Current drawing time in milliseconds
        /// </summary>
        public long CurrentDrawingTime => (Lobby == null) ? 0L : Lobby.CurrentDrawingTime;

        /// <summary>
        /// Previous word
        /// </summary>
        public string PreviousWord => (Lobby == null) ? string.Empty : Lobby.PreviousWord;

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
        public EGameState GameState => (Lobby == null) ? EGameState.Invalid : Lobby.GameState;

        /// <summary>
        /// Gets invoked when "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        public event NameChangeGameMessageReceivedDelegate OnNameChangeGameMessageReceived;

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
        /// Gets invoked when a "close-guess" game message has been received
        /// </summary>
        public event CloseGuessGameMessageReceivedDelegate OnCloseGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "kick-vote" game message has been received.
        /// </summary>
        public event KickVoteGameMessageReceivedDelegate OnKickVoteGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "drawer-kicked" game message has been received.
        /// </summary>
        public event DrawerKickedGameMessageReceivedDelegate OnDrawerKickedGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "owner-change" game message has been received.
        /// </summary>
        public event OwnerChangeGameMessageReceivedDelegate OnOwnerChangeGameMessageReceived;

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
                Lobby.OnNameChangeGameMessageReceived += LobbyNameChangeMessageReceivedEvent;
                Lobby.OnUpdatePlayersGameMessageReceived += LobbyUpdatePlayersGameMessageReceivedEvent;
                Lobby.OnUpdateWordhintGameMessageReceived += LobbyUpdateWordhintGameMessageReceivedEvent;
                Lobby.OnMessageGameMessageReceived += LobbyMessageGameMessageReceivedEvent;
                Lobby.OnNonGuessingPlayerMessageGameMessageReceived += LobbyNonGuessingPlayerMessageGameMessageReceivedEvent;
                Lobby.OnSystemMessageGameMessageReceived += LobbySystemMessageGameMessageReceivedEvent;
                Lobby.OnLineGameMessageReceived += LobbyLineGameMessageReceivedEvent;
                Lobby.OnFillGameMessageReceived += LobbyFillGameMessageReceivedEvent;
                Lobby.OnClearDrawingBoardGameMessageReceived += LobbyClearDrawingBoardGameMessageReceivedEvent;
                Lobby.OnYourTurnGameMessageReceived += LobbyYourTurnGameMessageReceivedEvent;
                Lobby.OnCloseGuessGameMessageReceived += LobbyCloseGuessGameMessageReceivedEvent;
                Lobby.OnCorrectGuessGameMessageReceived += LobbyCorrectGuessGameMessageReceivedEvent;
                Lobby.OnKickVoteGameMessageReceived += LobbyKickVoteGameMessageReceivedEvent;
                Lobby.OnDrawerKickedGameMessageReceived += LobbyDrawerKickedGameMessageReceivedEvent;
                Lobby.OnOwnerChangeGameMessageReceived += LobbyOwnerChangeGameMessageReceivedEvent;
                Lobby.OnDrawingGameMessageReceived += LobbyDrawingGameMessageReceivedEvent;
                Lobby.OnUnknownGameMessageReceived += LobbyUnknownGameMessageReceivedEvent;
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void LobbyReadyGameMessageReceivedEvent()
        {
            CanvasColor = new Color32(Lobby.CanvasColor.Red, Lobby.CanvasColor.Green, Lobby.CanvasColor.Blue, 0xFF);
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void LobbyNextTurnGameMessageReceivedEvent()
        {
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "name-change" game message has been received
        /// </summary>
        private void LobbyNameChangeMessageReceivedEvent(IPlayer player)
        {
            if (onNameChangeGameMessageReceived != null)
            {
                onNameChangeGameMessageReceived.Invoke();
            }
            OnNameChangeGameMessageReceived?.Invoke(player);
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
        private void LobbyLineGameMessageReceivedEvent(float fromX, float fromY, float toX, float toY, IColor color, float lineWidth)
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
        private void LobbyFillGameMessageReceivedEvent(float positionX, float positionY, IColor color)
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
        /// Gets invoked when "close-guess" game message has been received
        /// </summary>
        private void LobbyCloseGuessGameMessageReceivedEvent(string closelyGuessedWord)
        {
            if (onCloseGuessGameMessageReceived != null)
            {
                onCloseGuessGameMessageReceived.Invoke();
            }
            OnCloseGuessGameMessageReceived?.Invoke(closelyGuessedWord);
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
        /// Gets invoked when "kick-vote" game message has been received
        /// </summary>
        private void LobbyKickVoteGameMessageReceivedEvent(IPlayer player, uint voteCount, uint requiredVoteCount)
        {
            if (onKickVoteGameMessageReceived != null)
            {
                onKickVoteGameMessageReceived.Invoke();
            }
            OnKickVoteGameMessageReceived?.Invoke(player, voteCount, requiredVoteCount);
        }

        /// <summary>
        /// Gets invoked when "drawer-kicked" game message has been received
        /// </summary>
        private void LobbyDrawerKickedGameMessageReceivedEvent()
        {
            if (onDrawerKickedGameMessageReceived != null)
            {
                onDrawerKickedGameMessageReceived.Invoke();
            }
            OnDrawerKickedGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when "owner-changed" game message has been received
        /// </summary>
        private void LobbyOwnerChangeGameMessageReceivedEvent(IPlayer player)
        {
            if (onOwnerChangeGameMessageReceived != null)
            {
                onOwnerChangeGameMessageReceived.Invoke();
            }
            OnOwnerChangeGameMessageReceived?.Invoke(player);
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
                ScribblersClient = Clients.Create(save_game.Data.ScribblersHost, save_game.Data.GetUserSessionID(save_game.Data.ScribblersHost), save_game.Data.IsUsingSecureProtocols, save_game.Data.IsAllowedToUseInsecureProtocols);
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
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    save_game.Data.SetUserSessionID(ScribblersClient.Host, ScribblersClient.UserSessionID);
                    save_game.Save();
                }
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
                Lobby.OnNameChangeGameMessageReceived -= LobbyNameChangeMessageReceivedEvent;
                Lobby.OnUpdatePlayersGameMessageReceived -= LobbyUpdatePlayersGameMessageReceivedEvent;
                Lobby.OnUpdateWordhintGameMessageReceived -= LobbyUpdateWordhintGameMessageReceivedEvent;
                Lobby.OnMessageGameMessageReceived -= LobbyMessageGameMessageReceivedEvent;
                Lobby.OnNonGuessingPlayerMessageGameMessageReceived -= LobbyNonGuessingPlayerMessageGameMessageReceivedEvent;
                Lobby.OnSystemMessageGameMessageReceived -= LobbySystemMessageGameMessageReceivedEvent;
                Lobby.OnLineGameMessageReceived -= LobbyLineGameMessageReceivedEvent;
                Lobby.OnFillGameMessageReceived -= LobbyFillGameMessageReceivedEvent;
                Lobby.OnClearDrawingBoardGameMessageReceived -= LobbyClearDrawingBoardGameMessageReceivedEvent;
                Lobby.OnYourTurnGameMessageReceived -= LobbyYourTurnGameMessageReceivedEvent;
                Lobby.OnCloseGuessGameMessageReceived -= LobbyCloseGuessGameMessageReceivedEvent;
                Lobby.OnCorrectGuessGameMessageReceived -= LobbyCorrectGuessGameMessageReceivedEvent;
                Lobby.OnKickVoteGameMessageReceived -= LobbyKickVoteGameMessageReceivedEvent;
                Lobby.OnDrawerKickedGameMessageReceived -= LobbyDrawerKickedGameMessageReceivedEvent;
                Lobby.OnOwnerChangeGameMessageReceived -= LobbyOwnerChangeGameMessageReceivedEvent;
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
        /// Gets server statistics
        /// </summary>
        /// <returns>Server statistics task</returns>
        public Task<IServerStatistics> GetServerStatisticsAsync() => (ScribblersClient == null) ? Task.FromResult<IServerStatistics>(new ServerStatistics()) : ScribblersClient.GetServerStatisticsAsync();

        /// <summary>
        /// Lists all public lobbies
        /// </summary>
        /// <returns>Lobby views task</returns>
        public Task<ILobbyViews> ListLobbiesAsync() => (ScribblersClient == null) ? Task.FromResult<ILobbyViews>(null) : ScribblersClient.ListLobbiesAsync();

        /// <summary>
        /// Changes lobby rules
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="isPublic">Is lobby public</param>
        /// <param name="maximalPlayerCount">Maximal player count</param>
        /// <param name="drawingTime">Drawing time</param>
        /// <param name="roundCount">Round count</param>
        /// <param name="customWords">Custom words</param>
        /// <param name="customWordsChance">Custom words chance</param>
        /// <param name="isVotekickingEnabled">Is votekicking enabled</param>
        /// <param name="clientsPerIPLimit">Clients per IP limit</param>
        /// <returns>Task</returns>
        public Task ChangeLobbyRulesAsync(ELanguage? language = null, bool? isPublic = null, uint? maximalPlayerCount = null, ulong? drawingTime = null, uint? roundCount = null, IReadOnlyList<string> customWords = null, uint? customWordsChance = null, bool? isVotekickingEnabled = null, uint? clientsPerIPLimit = null) => (ScribblersClient == null) ? Task.CompletedTask : ScribblersClient.ChangeLobbyRulesAsync(language, isPublic, maximalPlayerCount, drawingTime, roundCount, customWords, customWordsChance, isVotekickingEnabled, clientsPerIPLimit);

        /// <summary>
        /// Sends a "start-game" message
        /// </summary>
        public void SendStartGameMessage() => Lobby?.SendStartGameMessage();

        /// <summary>
        /// Sends a "line" game message
        /// </summary>
        /// <param name="fromX">X component of start line position</param>
        /// <param name="fromY">Y component of start line position</param>
        /// <param name="toX">X component of end line position</param>
        /// <param name="toY">Y component of end line position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width in pixels</param>
        public void SendLineGameMessage(float fromX, float fromY, float toX, float toY, ScribblersSharp.Color color, float lineWidth) => Lobby?.SendLineGameMessage(fromX, fromY, toX, toY, color, lineWidth);

        /// <summary>
        /// Sends a "fill" game message
        /// </summary>
        /// <param name="positionX">X component of fill start posiiton</param>
        /// <param name="positionY">Y component of fill start position</param>
        /// <param name="color"></param>
        public void SendFillGameMessage(float positionX, float positionY, ScribblersSharp.Color color) => Lobby?.SendFillGameMessage(positionX, positionY, color);

        /// <summary>
        /// Sends a "clear-drawing-board" game message
        /// </summary>
        public void SendClearDrawingBoardGameMessage() => Lobby?.SendClearDrawingBoardGameMessage();

        /// <summary>
        /// Sends a "message" game message
        /// </summary>
        /// <param name="content">Message content</param>
        public void SendMessageGameMessage(string content) => Lobby?.SendMessageGameMessage(content);

        /// <summary>
        /// Sends a "choose-word" game message
        /// </summary>
        /// <param name="index">Word index</param>
        public void SendChooseWordGameMessage(uint index) => Lobby?.SendChooseWordGameMessage(index);

        /// <summary>
        /// Sends a "name-change" game message
        /// </summary>
        /// <param name="newUsername">New username</param>
        public void SendNameChangeGameMessage(string newUsername) => Lobby?.SendNameChangeGameMessage(newUsername);

        /// <summary>
        /// Sends a "request-drawing" game message
        /// </summary>
        public void SendRequestDrawingGameMessage() => Lobby?.SendRequestDrawingGameMessage();

        /// <summary>
        /// Sends a "kick-vote" game message
        /// </summary>
        /// <param name="toKickPlayer">To kick player</param>
        public void SendKickVoteGameMessage(IPlayer toKickPlayer) => Lobby?.SendKickVoteGameMessage(toKickPlayer);

        /// <summary>
        /// Sends a "keep-alive" game message
        /// </summary>
        public void SendKeepAliveGameMessage() => Lobby?.SendKeepAliveGameMessage();

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
        private void Start()
        {
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game == null)
            {
                SceneLoaderManager.LoadScene("MainMenuScene");
            }
            else
            {
                string username = save_game.Data.Username.Trim();
                if (username.Length > Rules.maximalUsernameLength)
                {
                    username = username.Substring(0, (int)Rules.maximalUsernameLength);
                }
                if (string.IsNullOrWhiteSpace(save_game.Data.LobbyID.Trim()))
                {
                    _ = CreateLobbyAsync(username, save_game.Data.LobbyLanguage, save_game.Data.IsLobbyPublic, save_game.Data.MaximalPlayerCount, save_game.Data.DrawingTime, save_game.Data.RoundCount, save_game.Data.CustomWords.Split(','), save_game.Data.CustomWordsChance, save_game.Data.IsVotekickingEnabled, save_game.Data.PlayersPerIPLimit);
                }
                else
                {
                    _ = EnterLobbyAsync(save_game.Data.LobbyID.Trim(), username);
                }
            }
        }

        /// <summary>
        /// Gets invoked when script perfoms a fixed update
        /// </summary>
        private void FixedUpdate() => Lobby?.ProcessEvents();
    }
}
