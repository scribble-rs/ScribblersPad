using ScribblersSharp;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scribble.rs Pad namespace
/// </summary>
namespace ScribblersPad
{
    /// <summary>
    /// An interface that represents a drawing board controller
    /// </summary>
    public interface IDrawingBoardController : IScribblersClientController
    {
        /// <summary>
        /// Is my player allowed to drawing right now
        /// </summary>
        bool IsPlayerDrawing { get; }

        /// <summary>
        /// Raw image
        /// </summary>
        RawImage RawImage { get; }

        /// <summary>
        /// Rectangle transform
        /// </summary>
        RectTransform RectangleTransform { get; }

        /// <summary>
        /// Gets invoked when a line has been drawn
        /// </summary>
        event LineDrawnDelegate OnLineDrawn;

        /// <summary>
        /// Gets invoked when drawing board has been filled
        /// </summary>
        event DrawingBoardFilledDelegate OnDrawingBoardFilled;

        /// <summary>
        /// Gets called when drawing board has been cleared
        /// </summary>
        event DrawingBoardClearedDelegate OnDrawingBoardCleared;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "line" game message has been received
        /// </summary>
        event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "fill" game message has been received
        /// </summary>
        event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received
        /// </summary>
        event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="from">Line start position</param>
        /// <param name="to">Line end position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        void DrawLine(Vector2 from, Vector2 to, Color32 color, float lineWidth);

        /// <summary>
        /// Flood fills drawing board
        /// </summary>
        /// <param name="position">Flood fill posiiton</param>
        /// <param name="color">Flood fill color</param>
        void Fill(Vector2 position, Color32 color);

        /// <summary>
        /// Clears drawing board
        /// </summary>
        void Clear();

        /// <summary>
        /// Processes next draw command in queue
        /// </summary>
        void ProcessNextDrawCommandInQueue();
    }
}
