using ScribblersSharp;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents language sprite data
    /// </summary>
    public interface ILanguageSpriteData
    {
        /// <summary>
        /// Language
        /// </summary>
        ELanguage Language { get; set; }

        /// <summary>
        /// Language sprite
        /// </summary>
        Sprite Sprite { get; set; }
    }
}
