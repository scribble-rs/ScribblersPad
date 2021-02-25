using ScribblersSharp;
using System;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad data namespace
/// </summary>
namespace ScribblersPad.Data
{
    /// <summary>
    /// A structure that describes language sprite data
    /// </summary>
    [Serializable]
    public struct LanguageSpriteData : ILanguageSpriteData
    {
        /// <summary>
        /// Language
        /// </summary>
        [SerializeField]
        private ELanguage language;

        /// <summary>
        /// Language sprite
        /// </summary>
        [SerializeField]
        private Sprite sprite;

        /// <summary>
        /// Language
        /// </summary>
        public ELanguage Language
        {
            get => language;
            set => language = value;
        }

        /// <summary>
        /// Language sprite
        /// </summary>
        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }
    }
}
