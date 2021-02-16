using ScribblersSharp;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a player list element controller script
    /// </summary>
    public class PlayerListElementControllerScript : MonoBehaviour, IPlayerListElementController
    {
        /// <summary>
        /// Name text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI nameText = default;

        /// <summary>
        /// Score text
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI scoreText = default;

        /// <summary>
        /// Name text
        /// </summary>
        public TextMeshProUGUI NameText
        {
            get => nameText;
            set => nameText = value;
        }

        /// <summary>
        /// Score text
        /// </summary>
        public TextMeshProUGUI ScoreText
        {
            get => scoreText;
            set => scoreText = value;
        }

        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="player">Player</param>
        public void SetValues(IPlayer player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            if (nameText)
            {
                nameText.text = player.Name;
            }
            if (scoreText)
            {
                scoreText.text = player.Score.ToString();
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (!scoreText)
            {
                Debug.LogError("Please assign a score text reference to this component.", this);
            }
        }
    }
}
