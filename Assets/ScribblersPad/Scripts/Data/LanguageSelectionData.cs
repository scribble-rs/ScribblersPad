using ScribblersSharp;
using System;
using UnityEngine;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad data namespace
/// </summary>
namespace ScribblersPad.Data
{
    /// <summary>
    /// A structure that describes language selection data
    /// </summary>
    [Serializable]
    public struct LanguageSelectionData : ILanguageSelectionData
    {
        /// <summary>
        /// Language
        /// </summary>
        [SerializeField]
        private ELanguage language;

        /// <summary>
        /// Language string translation
        /// </summary>
        [SerializeField]
        private StringTranslationObjectScript langugageStringTranslation;

        /// <summary>
        /// Language sprite
        /// </summary>
        [SerializeField]
        private Sprite languageSprite;

        /// <summary>
        /// Language
        /// </summary>
        public ELanguage Language
        {
            get => language;
            set => language = value;
        }

        /// <summary>
        /// Language string translation
        /// </summary>
        public StringTranslationObjectScript LangugageStringTranslation
        {
            get => langugageStringTranslation;
            set => langugageStringTranslation = value;
        }

        /// <summary>
        /// Language sprite
        /// </summary>
        public Sprite LanguageSprite
        {
            get => languageSprite;
            set => languageSprite = value;
        }
    }
}
