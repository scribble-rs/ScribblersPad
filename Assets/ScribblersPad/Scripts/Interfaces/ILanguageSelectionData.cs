using ScribblersSharp;
using UnityEngine;
using UnityTranslator.Objects;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// AN interface that represents language selection data
    /// </summary>
    public interface ILanguageSelectionData
    {
        /// <summary>
        /// Language
        /// </summary>
        ELanguage Language { get; set; }

        /// <summary>
        /// Language string translation
        /// </summary>
        StringTranslationObjectScript LangugageStringTranslation { get; set; }

        /// <summary>
        /// Language sprite
        /// </summary>
        Sprite LanguageSprite { get; set; }
    }
}
