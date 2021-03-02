using ScribblersPad.Controllers;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// AN interface that represents a lobby settings controller
    /// </summary>
    public interface ILobbySettingsController : IBehaviour
    {
        /// <summary>
        /// Maximal test connection task time
        /// </summary>
        float MaximalTestConnectionTaskTime { get; set; }

        /// <summary>
        /// Host input field
        /// </summary>
        TMP_InputField HostInputField { get; set; }

        /// <summary>
        /// Is using secure protocols toggle
        /// </summary>
        Toggle IsUsingSecureProtocolsToggle { get; set; }

        /// <summary>
        /// Username input field
        /// </summary>
        TMP_InputField UsernameInputField { get; set; }

        /// <summary>
        /// Lobby language dropdown controller
        /// </summary>
        LobbyLanguageDropdownControllerScript LobbyLanguageDropdownController { get; set; }

        /// <summary>
        /// Drawing time slider
        /// </summary>
        Slider DrawingTimeSlider { get; set; }

        /// <summary>
        /// Round count slider
        /// </summary>
        Slider RoundCountSlider { get; set; }

        /// <summary>
        /// Maximal player count slider
        /// </summary>
        Slider MaximalPlayerCountSlider { get; set; }

        /// <summary>
        /// Is lobby public toggle
        /// </summary>
        Toggle IsLobbyPublicToggle { get; set; }

        /// <summary>
        /// Custom words input field
        /// </summary>
        TMP_InputField CustomWordsInputField { get; set; }

        /// <summary>
        /// Custom words chance slider
        /// </summary>
        Slider CustomWordsChanceSlider { get; set; }

        /// <summary>
        /// Players per IP limit slider
        /// </summary>
        Slider PlayersPerIPLimitSlider { get; set; }

        /// <summary>
        /// Is votekicking enabled toggle
        /// </summary>
        Toggle IsVotekickingEnabledToggle { get; set; }

        /// <summary>
        /// Gets invoked when pinging host has been started
        /// </summary>
        event PingStartedDelegate OnPingStarted;

        /// <summary>
        /// Gets invoked when pinging host has failed
        /// </summary>
        event PingFailedDelegate OnPingFailed;

        /// <summary>
        /// Gets invoked when pinging host was successful
        /// </summary>
        event PingSucceededDelegate OnPingSucceeded;

        /// <summary>
        /// Tests connection
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Resets host to default
        /// </summary>
        void ResetHostToDefault();

        /// <summary>
        /// Saves lobby settings
        /// </summary>
        void SaveSettings();
    }
}
