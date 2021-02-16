using ScribblersPad.Controllers;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a word selection button controller
    /// </summary>
    public interface IWordSelectionButtonController : IScribblersClientController
    {
        /// <summary>
        /// Word text
        /// </summary>
        public TextMeshProUGUI WordText { get; set; }

        /// <summary>
        /// Word
        /// </summary>
        public string Word { get; }

        /// <summary>
        /// Index
        /// </summary>
        public uint Index { get; }

        /// <summary>
        /// Parent
        /// </summary>
        public WordSelectionControllerScript Parent { get; }

        /// <summary>
        /// Button
        /// </summary>
        public Button Button { get; }

        /// <summary>
        /// Sets values in component
        /// </summary>
        /// <param name="word">Word</param>
        /// <param name="index">Index</param>
        /// <param name="parent">Parent</param>
        void SetValues(string word, uint index, WordSelectionControllerScript parent);
    }
}
