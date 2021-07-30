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
    /// A class that describes a chat box controller script
    /// </summary>
    public class ChatBoxControllerScript : AScribblersClientControllerScript, IChatBoxController
    {
        /// <summary>
        /// Chat box element limit count
        /// </summary>
        [SerializeField]
        private uint chatBoxElementLimitCount;

        /// <summary>
        /// Message chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject messageChatBoxElementAsset = default;

        /// <summary>
        /// Own message chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject ownMessageChatBoxElementAsset = default;

        /// <summary>
        /// Non guessing player message chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject nonGuessingPlayerMessageChatBoxElementAsset = default;

        /// <summary>
        /// Own non-guessing player message chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject ownNonGuessingPlayerMessageChatBoxElementAsset = default;

        /// <summary>
        /// Closely guessed chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject closelyGuessedChatBoxElementAsset = default;

        /// <summary>
        /// Correctly guessed chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject correctlyGuessedChatBoxElementAsset = default;

        /// <summary>
        /// Own correctly guessed chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject ownCorrectlyGuessedChatBoxElementAsset = default;

        /// <summary>
        /// System message chat box element asset
        /// </summary>
        [SerializeField]
        private GameObject systemMessageChatBoxElementAsset = default;

        [SerializeField]
        private GameObject chatPreviewMessageChatBoxElementAsset = default;

        /// <summary>
        /// Chat box element parent rectangle transform
        /// </summary>
        [SerializeField]
        private RectTransform chatBoxElementParentRectangleTransform = default;

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
        /// Gets invoked when "close-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCloseGuessGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onCorrectGuessGameMessageReceived = default;

        /// <summary>
        /// Chat box element controllers
        /// </summary>
        private readonly List<RectTransform> chatBoxElementControllers = new List<RectTransform>();

        /// <summary>
        /// Scroll value
        /// </summary>
        private Vector2 scrollValue;

        /// <summary>
        /// Chat box element limit count
        /// </summary>
        public uint ChatBoxElementLimitCount
        {
            get => chatBoxElementLimitCount;
            set => chatBoxElementLimitCount = value;
        }

        /// <summary>
        /// Message chat box element asset
        /// </summary>
        public GameObject MessageChatBoxElementAsset
        {
            get => messageChatBoxElementAsset;
            set => messageChatBoxElementAsset = value;
        }

        /// <summary>
        /// Own message chat box element asset
        /// </summary>
        public GameObject OwnMessageChatBoxElementAsset
        {
            get => ownMessageChatBoxElementAsset;
            set => ownMessageChatBoxElementAsset = value;
        }

        /// <summary>
        /// Non guessing player message chat box element asset
        /// </summary>
        public GameObject NonGuessingPlayerMessageChatBoxElementAsset
        {
            get => nonGuessingPlayerMessageChatBoxElementAsset;
            set => nonGuessingPlayerMessageChatBoxElementAsset = value;
        }

        /// <summary>
        /// Own non-guessing player message chat box element asset
        /// </summary>
        public GameObject OwnNonGuessingPlayerMessageChatBoxElementAsset
        {
            get => ownNonGuessingPlayerMessageChatBoxElementAsset;
            set => ownNonGuessingPlayerMessageChatBoxElementAsset = value;
        }

        /// <summary>
        /// Closely guessed chat box element asset
        /// </summary>
        public GameObject CloselyGuessedChatBoxElementAsset
        {
            get => closelyGuessedChatBoxElementAsset;
            set => closelyGuessedChatBoxElementAsset = value;
        }

        /// <summary>
        /// Correctly guessed chat box element asset
        /// </summary>
        public GameObject CorrectlyGuessedChatBoxElementAsset
        {
            get => correctlyGuessedChatBoxElementAsset;
            set => correctlyGuessedChatBoxElementAsset = value;
        }

        /// <summary>
        /// Own correctly guessed chat box element asset
        /// </summary>
        public GameObject OwnCorrectlyGuessedChatBoxElementAsset
        {
            get => ownCorrectlyGuessedChatBoxElementAsset;
            set => ownCorrectlyGuessedChatBoxElementAsset = value;
        }

        /// <summary>
        /// System message chat box element asset
        /// </summary>
        public GameObject SystemMessageChatBoxElementAsset
        {
            get => systemMessageChatBoxElementAsset;
            set => systemMessageChatBoxElementAsset = value;
        }

        /// <summary>
        /// Chat box element parent rectangle transform
        /// </summary>
        public RectTransform ChatBoxElementParentRectangleTransform
        {
            get => chatBoxElementParentRectangleTransform;
            set => chatBoxElementParentRectangleTransform = value;
        }

        /// <summary>
        /// Scroll value
        /// </summary>
        public Vector2 ScrollValue
        {
            get => scrollValue;
            set => scrollValue = value;
        }

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
        /// Gets invoked when "close-guess" game message has been received
        /// </summary>
        public event CloseGuessGameMessageReceivedDelegate OnCloseGuessGameMessageReceived;

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        public event CorrectGuessGameMessageReceivedDelegate OnCorrectGuessGameMessageReceived;

        /// <summary>
        /// Validates the specified chat box element
        /// </summary>
        /// <param name="asset">Chat box element asset</param>
        private void ValidateChatBoxElement(ref GameObject asset)
        {
            if (asset && (!asset.TryGetComponent(out RectTransform _) || !asset.TryGetComponent(out AChatBoxElementControllerScript _)))
            {
                asset = null;
            }
        }

        /// <summary>
        /// Notifies in console if reference is missing
        /// </summary>
        /// <typeparam name="T">Reference type</typeparam>
        /// <param name="reference">Reference</param>
        /// <param name="fieldName">Field name</param>
        private void NotifyIfReferenceIsMissing<T>(T reference, string fieldName) where T : UnityEngine.Object
        {
            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            if (!reference)
            {
                Debug.LogError($"Please assign a \"{ fieldName }\" of type \"{ typeof(T).Name }\" to this component.", this);
            }
        }

        /// <summary>
        /// Asserts the specified asset to be a chat box element asset
        /// </summary>
        /// <param name="asset">Asset</param>
        private void AssertChatBoxElement(GameObject asset)
        {
            if (!gameObject)
            {
                throw new ArgumentException("Not all chat box element asset references has been assigned yet.", nameof(asset));
            }
            if (!asset.TryGetComponent(out RectTransform _) || !asset.TryGetComponent(out AChatBoxElementControllerScript _))
            {
                throw new ArgumentException("Not all chat box element asset references are chat box asset references.", nameof(asset));
            }
        }

        /// <summary>
        /// Adds a chat box element
        /// </summary>
        /// <param name="asset">Chat box element asset</param>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void AddChatBoxElement(GameObject asset, string author, string content)
        {
            AssertChatBoxElement(asset);
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (chatBoxElementParentRectangleTransform)
            {
                GameObject game_object = Instantiate(asset);
                if (game_object)
                {
                    if (game_object.TryGetComponent(out RectTransform rectangle_transform) && game_object.TryGetComponent(out AChatBoxElementControllerScript chat_box_element_controller))
                    {
                        rectangle_transform.SetParent(chatBoxElementParentRectangleTransform, false);
                        chat_box_element_controller.SetValues(author, content);
                        chatBoxElementControllers.Add(rectangle_transform);
                        if (chatBoxElementLimitCount > 0U)
                        {
                            while (chatBoxElementControllers.Count > chatBoxElementLimitCount)
                            {
                                RectTransform chat_box_element_rectangle_transform = chatBoxElementControllers[0];
                                if (chat_box_element_rectangle_transform && chat_box_element_rectangle_transform.gameObject)
                                {
                                    Destroy(chat_box_element_rectangle_transform.gameObject);
                                }
                                chatBoxElementControllers.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        Destroy(game_object);
                        Debug.LogError("Reference for a chat box element asset is not a real chat box asset reference.", this);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate chat box element asset.", this);
                }
            }
            else
            {
                Debug.LogError("Please assign a vertical layout group to this component.", this);
            }
        }

        /// <summary>
        /// Gets invoked when "message" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerMessageGameMessageReceivedEvent(IPlayer author, string content)
        {
            AddChatBoxElement((ScribblersClientManager.MyPlayer.ID == author.ID) ? ownMessageChatBoxElementAsset : messageChatBoxElementAsset, author.Name, content);
            if (onMessageGameMessageReceived != null)
            {
                onMessageGameMessageReceived.Invoke();
            }
            OnMessageGameMessageReceived?.Invoke(author, content);
        }

        /// <summary>
        /// Gets invoked when "non-guessing-player-message" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerNonGuessingPlayerMessageGameMessageReceivedEvent(IPlayer author, string content)
        {
            AddChatBoxElement((ScribblersClientManager.MyPlayer.ID == author.ID) ? ownNonGuessingPlayerMessageChatBoxElementAsset : nonGuessingPlayerMessageChatBoxElementAsset, author.Name, content);
            if (onNonGuessingPlayerMessageGameMessageReceived != null)
            {
                onNonGuessingPlayerMessageGameMessageReceived.Invoke();
            }
            OnNonGuessingPlayerMessageGameMessageReceived?.Invoke(author, content);
        }

        /// <summary>
        /// Gets invoked when "system-message" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerSystemMessageGameMessageReceivedEvent(string content)
        {
            AddChatBoxElement(systemMessageChatBoxElementAsset, string.Empty, content);
            if (onSystemMessageGameMessageReceived != null)
            {
                onSystemMessageGameMessageReceived.Invoke();
            }
            OnSystemMessageGameMessageReceived?.Invoke(content);
        }

        /// <summary>
        /// Gets invoked when "close-guess" game message has been received
        /// </summary>
        /// <param name="closelyGuessedWord">Closely guessed word</param>
        private void ScribblersClientManagerCloseGuessGameMessageReceivedEvent(string closelyGuessedWord)
        {
            AddChatBoxElement(closelyGuessedChatBoxElementAsset, string.Empty, closelyGuessedWord);
            if (onCloseGuessGameMessageReceived != null)
            {
                onCloseGuessGameMessageReceived.Invoke();
            }
            OnCloseGuessGameMessageReceived?.Invoke(closelyGuessedWord);
        }

        /// <summary>
        /// Gets invoked when "correct-guess" game message has been received
        /// </summary>
        /// <param name="author">Author</param>
        /// <param name="content">Content</param>
        private void ScribblersClientManagerCorrectGuessGameMessageReceivedEvent(IPlayer player)
        {
            AddChatBoxElement((ScribblersClientManager.MyPlayer.ID == player.ID) ? ownCorrectlyGuessedChatBoxElementAsset : correctlyGuessedChatBoxElementAsset, player.Name, string.Empty);
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
            ScribblersClientManager.OnMessageGameMessageReceived += ScribblersClientManagerMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnNonGuessingPlayerMessageGameMessageReceived += ScribblersClientManagerNonGuessingPlayerMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnSystemMessageGameMessageReceived += ScribblersClientManagerSystemMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnCloseGuessGameMessageReceived += ScribblersClientManagerCloseGuessGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived += ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnMessageGameMessageReceived -= ScribblersClientManagerMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnNonGuessingPlayerMessageGameMessageReceived -= ScribblersClientManagerNonGuessingPlayerMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnSystemMessageGameMessageReceived -= ScribblersClientManagerSystemMessageGameMessageReceivedEvent;
            ScribblersClientManager.OnCloseGuessGameMessageReceived -= ScribblersClientManagerCloseGuessGameMessageReceivedEvent;
            ScribblersClientManager.OnCorrectGuessGameMessageReceived -= ScribblersClientManagerCorrectGuessGameMessageReceivedEvent;
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate()
        {
            ValidateChatBoxElement(ref messageChatBoxElementAsset);
            ValidateChatBoxElement(ref ownMessageChatBoxElementAsset);
            ValidateChatBoxElement(ref nonGuessingPlayerMessageChatBoxElementAsset);
            ValidateChatBoxElement(ref ownNonGuessingPlayerMessageChatBoxElementAsset);
            ValidateChatBoxElement(ref closelyGuessedChatBoxElementAsset);
            ValidateChatBoxElement(ref correctlyGuessedChatBoxElementAsset);
            ValidateChatBoxElement(ref ownCorrectlyGuessedChatBoxElementAsset);
            ValidateChatBoxElement(ref systemMessageChatBoxElementAsset);
            ValidateChatBoxElement(ref chatPreviewMessageChatBoxElementAsset);
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            NotifyIfReferenceIsMissing(messageChatBoxElementAsset, nameof(messageChatBoxElementAsset));
            NotifyIfReferenceIsMissing(ownMessageChatBoxElementAsset, nameof(ownMessageChatBoxElementAsset));
            NotifyIfReferenceIsMissing(nonGuessingPlayerMessageChatBoxElementAsset, nameof(nonGuessingPlayerMessageChatBoxElementAsset));
            NotifyIfReferenceIsMissing(ownNonGuessingPlayerMessageChatBoxElementAsset, nameof(ownNonGuessingPlayerMessageChatBoxElementAsset));
            NotifyIfReferenceIsMissing(closelyGuessedChatBoxElementAsset, nameof(closelyGuessedChatBoxElementAsset));
            NotifyIfReferenceIsMissing(correctlyGuessedChatBoxElementAsset, nameof(correctlyGuessedChatBoxElementAsset));
            NotifyIfReferenceIsMissing(ownCorrectlyGuessedChatBoxElementAsset, nameof(ownCorrectlyGuessedChatBoxElementAsset));
            NotifyIfReferenceIsMissing(systemMessageChatBoxElementAsset, nameof(systemMessageChatBoxElementAsset));
            if (!chatBoxElementParentRectangleTransform)
            {
                Debug.LogError("Please assign a chat box element parent rectangle transform to this component.", this);
            }
            if (chatPreviewMessageChatBoxElementAsset)
            {
                AddChatBoxElement(chatPreviewMessageChatBoxElementAsset, string.Empty, string.Empty);
            }
        }
    }
}
