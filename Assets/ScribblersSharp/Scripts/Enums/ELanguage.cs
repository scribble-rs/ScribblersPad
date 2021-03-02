﻿using Newtonsoft.Json;
using ScribblersSharp.JSONConverters;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// Language enumerator
    /// </summary>
    [JsonConverter(typeof(LanguageJSONConverter))]
    public enum ELanguage
    {
        /// <summary>
        /// Invalid language
        /// </summary>
        Invalid,

        /// <summary>
        /// Dutch
        /// </summary>
        Dutch,

        /// <summary>
        /// English (GB)
        /// </summary>
        EnglishGB,

        /// <summary>
        /// English (US)
        /// </summary>
        EnglishUS,

        /// <summary>
        /// French
        /// </summary>
        French,

        /// <summary>
        /// German
        /// </summary>
        German,

        /// <summary>
        /// Italian
        /// </summary>
        Italian
    }
}
