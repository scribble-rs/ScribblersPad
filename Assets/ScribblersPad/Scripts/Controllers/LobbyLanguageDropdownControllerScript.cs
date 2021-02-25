using ScribblersPad.Data;
using ScribblersSharp;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a lobby labuage dropdown controller script
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    [ExecuteInEditMode]
    public class LobbyLanguageDropdownControllerScript : MonoBehaviour, ILobbyLanguageDropdownController
    {
        /// <summary>
        /// Selected lobby language
        /// </summary>
        [SerializeField]
        private ELanguage selectedLanguage = ELanguage.EnglishUS;

        /// <summary>
        /// Language selections
        /// </summary>
        [SerializeField]
        private LanguageSelectionData[] languageSelections = Array.Empty<LanguageSelectionData>();

        /// <summary>
        /// Language to dropdown element index lookup
        /// </summary>
        private readonly Dictionary<ELanguage, int> languageToDropdownElementIndexLookup = new Dictionary<ELanguage, int>();

        /// <summary>
        /// Language list
        /// </summary>
        private readonly List<ELanguage> languageList = new List<ELanguage>();

        /// <summary>
        /// Last selected lobby language
        /// </summary>
        private ELanguage lastSelectedLanguage = ELanguage.Invalid;

        /// <summary>
        /// Dropdown
        /// </summary>
        public TMP_Dropdown Dropdown { get; private set; }

        /// <summary>
        /// Selected lobby language
        /// </summary>
        public ELanguage SelectedLanguage
        {
            get => selectedLanguage;
            set => selectedLanguage = value;
        }

        /// <summary>
        /// Updates visuals
        /// </summary>
        private void UpdateVisuals()
        {
            if (Dropdown && (languageSelections != null))
            {
                HashSet<ELanguage> loaded_languages = new HashSet<ELanguage>();
                List<TMP_Dropdown.OptionData> dropdown_options = new List<TMP_Dropdown.OptionData>();
                foreach (LanguageSelectionData language_selection in languageSelections)
                {
                    if (language_selection.Language == ELanguage.Invalid)
                    {
                        Debug.LogError("Language can't be invalid.", this);
                    }
                    else if (loaded_languages.Contains(language_selection.Language))
                    {
                        Debug.LogWarning($"Skipping duplicate language selection entry \"{ language_selection.Language }\"...");
                    }
                    else
                    {
                        languageToDropdownElementIndexLookup.Add(language_selection.Language, dropdown_options.Count);
                        languageList.Add(language_selection.Language);
                        dropdown_options.Add(new TMP_Dropdown.OptionData(language_selection.LangugageStringTranslation ? language_selection.LangugageStringTranslation.ToString() : string.Empty, language_selection.LanguageSprite));
                    }
                }
                Dropdown.options.Clear();
                Dropdown.AddOptions(dropdown_options);
                dropdown_options.Clear();
                loaded_languages.Clear();
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (TryGetComponent(out TMP_Dropdown dropdown))
            {
                Dropdown = dropdown;
                UpdateVisuals();
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(TMP_Dropdown) }\" component to this game object.", this);
            }
        }

        /// <summary>
        /// Gets invoked when script gets validated
        /// </summary>
        private void OnValidate() => selectedLanguage = (selectedLanguage == ELanguage.Invalid) ? ELanguage.EnglishUS : selectedLanguage;

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update()
        {
            if (!Dropdown && TryGetComponent(out TMP_Dropdown dropdown))
            {
                Dropdown = dropdown;
                UpdateVisuals();
            }
            if (Dropdown)
            {
                if (lastSelectedLanguage != selectedLanguage)
                {
                    lastSelectedLanguage = selectedLanguage;
                    Dropdown.value = languageToDropdownElementIndexLookup.ContainsKey(selectedLanguage) ? languageToDropdownElementIndexLookup[selectedLanguage] : -1;
                }
                else if (Dropdown.value < languageList.Count)
                {
                    selectedLanguage = languageList[Dropdown.value];
                    lastSelectedLanguage = selectedLanguage;
                }
            }
        }
    }
}
