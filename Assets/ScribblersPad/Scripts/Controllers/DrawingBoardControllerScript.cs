using ScribblersSharp;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityParallel;

/// <summary>
/// Scribble.rs Pad controllers namespace
/// </summary>
namespace ScribblersPad.Controllers
{
    /// <summary>
    /// A class that describes a drawing board controller script
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class DrawingBoardControllerScript : AScribblersClientControllerScript, IDrawingBoardController
    {
        /// <summary>
        /// Gets invoked when a line has been drawn
        /// </summary>
        [SerializeField]
        private UnityEvent onLineDrawn = default;

        /// <summary>
        /// Gets invoked when drawing board has been filled
        /// </summary>
        [SerializeField]
        private UnityEvent onDrawingBoardFilled = default;

        /// <summary>
        /// Gets called when drawing board has been cleared
        /// </summary>
        [SerializeField]
        private UnityEvent onDrawingBoardCleared = default;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onReadyGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onNextTurnGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "line" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onLineGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "fill" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onFillGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onClearDrawingBoardGameMessageReceived = default;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        [SerializeField]
        private UnityEvent onYourTurnGameMessageReceived = default;

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Texture size
        /// </summary>
        private Vector2Int textureSize;

        /// <summary>
        /// Bitmap
        /// </summary>
        private NativeArray<Color32> bitmap;

        /// <summary>
        /// Draw command queue
        /// </summary>
        private ConcurrentQueue<DrawCommand> drawCommandQueue = new ConcurrentQueue<DrawCommand>();

        /// <summary>
        /// Draw thread
        /// </summary>
        private Thread drawThread;

        /// <summary>
        /// Is keeping draw thread running
        /// </summary>
        private bool isKeepingDrawThreadRunning = true;

        /// <summary>
        /// Is texture ready for update
        /// </summary>
        private bool isTextureReadyForUpdate;

        /// <summary>
        /// Relative frame count
        /// </summary>
        private uint relativeFrameCount;

        /// <summary>
        /// Is my player allowed to drawing right now
        /// </summary>
        public bool IsPlayerDrawing => ScribblersClientManager && ScribblersClientManager.IsPlayerAllowedToDraw;

        /// <summary>
        /// Raw image
        /// </summary>
        public RawImage RawImage { get; private set; }

        /// <summary>
        /// Rectangle transform
        /// </summary>
        public RectTransform RectangleTransform { get; private set; }

        /// <summary>
        /// Gets invoked when a line has been drawn
        /// </summary>
        public event LineDrawnDelegate OnLineDrawn;

        /// <summary>
        /// Gets invoked when drawing board has been filled
        /// </summary>
        public event DrawingBoardFilledDelegate OnDrawingBoardFilled;

        /// <summary>
        /// Gets called when drawing board has been cleared
        /// </summary>
        public event DrawingBoardClearedDelegate OnDrawingBoardCleared;

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        public event ReadyGameMessageReceivedDelegate OnReadyGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        public event NextTurnGameMessageReceivedDelegate OnNextTurnGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "line" game message has been received
        /// </summary>
        public event LineGameMessageReceivedDelegate OnLineGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "fill" game message has been received
        /// </summary>
        public event FillGameMessageReceivedDelegate OnFillGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "clear-drawing-board" game message has been received
        /// </summary>
        public event ClearDrawingBoardGameMessageReceivedDelegate OnClearDrawingBoardGameMessageReceived;

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        public event YourTurnGameMessageReceivedDelegate OnYourTurnGameMessageReceived;

        /// <summary>
        /// Pushes position to flood fill stack if color is targeted
        /// </summary>
        /// <param name="positionX">X component of flood fill posiiton</param>
        /// <param name="positionY">Y component of flood fill posiiton</param>
        /// <param name="targetColor">Target color</param>
        /// <param name="textureWidth">Texture width</param>
        /// <param name="stack">Flood fill stack</param>
        private void PushPositionIfColorIsTargeted(int positionX, int positionY, ref Color32 targetColor, int textureWidth, Stack<Vector2Int> stack)
        {
            Color32 current_color = bitmap[positionX + (positionY * textureWidth)];
            if ((current_color.r == targetColor.r) && (current_color.g == targetColor.g) && (current_color.b == targetColor.b))
            {
                stack.Push(new Vector2Int(positionX, positionY));
            }
        }

        /// <summary>
        /// Sets color if color is targeted
        /// </summary>
        /// <param name="positionX">X component of flood fill posiiton</param>
        /// <param name="positionY">Y component of flood fill posiiton</param>
        /// <param name="targetColor">Target color</param>
        /// <param name="newColor">New color</param>
        /// <param name="textureWidth">Texture width</param>
        /// <returns>"true" if color is targeted, otherwise "false"</returns>
        private bool SetColorIfColorIsTargeted(int positionX, int positionY, ref Color32 targetColor, ref Color32 newColor, int textureWidth)
        {
            bool ret = false;
            int bitmap_index = positionX + (positionY * textureWidth);
            Color32 current_color = bitmap[bitmap_index];
            if ((current_color.r == targetColor.r) && (current_color.g == targetColor.g) && (current_color.b == targetColor.b))
            {
                bitmap[bitmap_index] = newColor;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Draws line internally
        /// </summary>
        /// <param name="from">Line start position</param>
        /// <param name="to">Line end posiiton</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        private void DrawLineInternally(Vector2 from, Vector2 to, IColor color, float lineWidth)
        {
            float half_line_width = lineWidth * 0.5f;
            Vector2Int origin = new Vector2Int(
                Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(to.x, from.x) - half_line_width), 0, textureSize.x),
                Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(to.y, from.y) - half_line_width), 0, textureSize.y));
            Vector2Int size = new Vector2Int(
                Mathf.Clamp(Mathf.CeilToInt(Mathf.Abs(to.x - from.x) + lineWidth) + 1, 0, textureSize.x - origin.x),
                Mathf.Clamp(Mathf.CeilToInt(Mathf.Abs(to.y - from.y) + lineWidth) + 1, 0, textureSize.y - origin.y));
            Color32 draw_color = new Color32(color.Red, color.Green, color.Blue, 0xFF);
            for (int index = 0, length = size.x * size.y; index < length; index++)
            {
                Vector2Int position = new Vector2Int(origin.x + (index % size.x), origin.y + (index / size.x));
                Vector2 va = to - from;
                Vector2 vb = from - position;
                float a_squared = (va.x * va.x) + (va.y * va.y);
                float b_squared = (vb.x * vb.x) + (vb.y * vb.y);
                float distance_squared;
                if (a_squared > float.Epsilon)
                {
                    Vector2 vc = position - to;
                    float c_squared = (vc.x * vc.x) + (vc.y * vc.y);
                    float a = Mathf.Sqrt(a_squared);
                    float b = Mathf.Sqrt(b_squared);
                    float c = Mathf.Sqrt(c_squared);
                    float s = (a + b + c) * 0.5f;
                    float height_squared = Mathf.Abs(s * (s - a) * (s - b) * (s - c) / (a_squared * 0.25f));
                    if (c_squared > (a_squared + height_squared))
                    {
                        distance_squared = b_squared;
                    }
                    else if (b_squared > (a_squared + height_squared))
                    {
                        distance_squared = c_squared;
                    }
                    else
                    {
                        distance_squared = height_squared;
                    }
                }
                else
                {
                    distance_squared = b_squared;
                }
                if (distance_squared <= (lineWidth * lineWidth * 0.25f))
                {
                    bitmap[position.x + (textureSize.x * position.y)] = draw_color;
                }
            }
        }

        /// <summary>
        /// Flood fills internally
        /// </summary>
        /// <param name="position">Flood fill position</param>
        /// <param name="color">Flood fill color</param>
        private void FillInternally(Vector2 position, IColor color)
        {
            Vector2Int integer_position = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            if ((integer_position.x >= 0) && (integer_position.y >= 0) && (integer_position.x < textureSize.x) && (integer_position.y < textureSize.y))
            {
                int target_index = integer_position.x + (integer_position.y * textureSize.x);
                if (target_index < bitmap.Length)
                {
                    Color32 target_color = bitmap[target_index];
                    if ((color.Red != target_color.r) || (color.Green != target_color.g) || (color.Blue != target_color.b))
                    {
                        Color32 draw_color = new Color32(color.Red, color.Green, color.Blue, 0xFF);
                        Stack<Vector2Int> stack = new Stack<Vector2Int>();
                        stack.Push(integer_position);
                        while (stack.Count > 0)
                        {
                            Vector2Int current_position = stack.Pop();
                            int top_bound;
                            int bottom_bound;
                            int left_bound;
                            int right_bound;
                            bitmap[current_position.x + (current_position.y * textureSize.x)] = draw_color;
                            for (top_bound = current_position.y + 1; (top_bound < textureSize.y) && SetColorIfColorIsTargeted(current_position.x, top_bound, ref target_color, ref draw_color, textureSize.x); top_bound++)
                            {
                                // ...
                            }
                            top_bound--;
                            for (bottom_bound = current_position.y - 1; (bottom_bound >= 0) && SetColorIfColorIsTargeted(current_position.x, bottom_bound, ref target_color, ref draw_color, textureSize.x); bottom_bound--)
                            {
                                // ...
                            }
                            bottom_bound++;
                            for (left_bound = current_position.x - 1; (left_bound >= 0) && SetColorIfColorIsTargeted(left_bound, current_position.y, ref target_color, ref draw_color, textureSize.x); left_bound--)
                            {
                                // ...
                            }
                            left_bound++;
                            for (right_bound = current_position.x + 1; (right_bound < textureSize.x) && SetColorIfColorIsTargeted(right_bound, current_position.y, ref target_color, ref draw_color, textureSize.x); right_bound++)
                            {
                                // ...
                            }
                            right_bound--;
                            for (int position_x = left_bound; position_x <= right_bound; position_x++)
                            {
                                if (current_position.x != position_x)
                                {
                                    if (current_position.y > 0)
                                    {
                                        PushPositionIfColorIsTargeted(position_x, current_position.y - 1, ref target_color, textureSize.x, stack);
                                    }
                                    if ((current_position.y + 1) < textureSize.y)
                                    {
                                        PushPositionIfColorIsTargeted(position_x, current_position.y + 1, ref target_color, textureSize.x, stack);
                                    }
                                }
                            }
                            for (int position_y = bottom_bound; position_y <= top_bound; position_y++)
                            {
                                if (Mathf.Abs(current_position.y - position_y) > 1)
                                {
                                    if (current_position.x > 0)
                                    {
                                        PushPositionIfColorIsTargeted(current_position.x - 1, position_y, ref target_color, textureSize.x, stack);
                                    }
                                    if ((current_position.x + 1) < textureSize.x)
                                    {
                                        PushPositionIfColorIsTargeted(current_position.x + 1, position_y, ref target_color, textureSize.x, stack);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears internally
        /// </summary>
        private void ClearInternally()
        {
            if (texture)
            {
                IColor canvas_color = ScribblersClientManager.Lobby.CanvasColor;
                Color32 clear_color = new Color32(canvas_color.Red, canvas_color.Green, canvas_color.Blue, 0xFF);
                Parallel.For(0, bitmap.Length, (index) => bitmap[index] = clear_color);
                isTextureReadyForUpdate = false;
                texture.SetPixelData(bitmap, 0);
                texture.Apply(false);
            }
            if (onDrawingBoardCleared != null)
            {
                onDrawingBoardCleared.Invoke();
            }
            OnDrawingBoardCleared?.Invoke();
        }

        /// <summary>
        /// Updates texture
        /// </summary>
        private void UpdateTexture()
        {
            if (RawImage && (ScribblersClientManager.Lobby != null))
            {
                ILobby lobby = ScribblersClientManager.Lobby;
                if (!texture)
                {
                    texture = new Texture2D((int)lobby.DrawingBoardBaseWidth, (int)lobby.DrawingBoardBaseHeight, TextureFormat.RGBA32, false, true);
                }
                else if ((texture.width != lobby.DrawingBoardBaseWidth) || (texture.height != lobby.DrawingBoardBaseHeight))
                {
                    texture = new Texture2D((int)lobby.DrawingBoardBaseWidth, (int)lobby.DrawingBoardBaseHeight, TextureFormat.RGBA32, false, true);
                }
                if ((textureSize.x != texture.width) || (textureSize.y != texture.height))
                {
                    textureSize = new Vector2Int(texture.width, texture.height);
                }
                int texture_length = textureSize.x * textureSize.y;
                if (bitmap.Length != texture_length)
                {
                    NativeArray<Color32> old_bitmap = bitmap;
                    bitmap = new NativeArray<Color32>(texture_length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                    if (old_bitmap.IsCreated)
                    {
                        old_bitmap.Dispose();
                    }
                    ClearInternally();
                }
            }
        }

        /// <summary>
        /// Invokes drawing board filled event
        /// </summary>
        /// <param name="position">Fill position</param>
        /// <param name="color">Fill color</param>
        private void InvokeDrawingBoardFilledEvent(Vector2 position, IColor color)
        {
            if (onDrawingBoardFilled != null)
            {
                onDrawingBoardFilled.Invoke();
            }
            OnDrawingBoardFilled?.Invoke(position, color);
        }

        /// <summary>
        /// Invokes line drawn event
        /// </summary>
        /// <param name="from">Line start position</param>
        /// <param name="to">Line end posiiton</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        private void InvokeLineDrawEvent(Vector2 from, Vector2 to, IColor color, float lineWidth)
        {
            if (onLineDrawn != null)
            {
                onLineDrawn.Invoke();
            }
            OnLineDrawn?.Invoke(from, to, color, lineWidth);
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            UpdateTexture();
            ClearInternally();
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
            foreach (ScribblersSharp.IDrawCommand draw_command in ScribblersClientManager.Lobby.CurrentDrawing)
            {
                Vector2 from = new Vector2(draw_command.FromX, draw_command.FromY);
                Vector2 to = new Vector2(draw_command.ToX, draw_command.ToY);
                drawCommandQueue.Enqueue(new DrawCommand(draw_command.Type, from, to, draw_command.Color, draw_command.LineWidth));
                switch (draw_command.Type)
                {
                    case EDrawCommandType.Fill:
                        InvokeDrawingBoardFilledEvent(from, draw_command.Color);
                        break;
                    case EDrawCommandType.Line:
                        InvokeLineDrawEvent(from, to, draw_command.Color, draw_command.LineWidth);
                        break;
                }
            }
        }

        /// <summary>
        /// Gets invoked when a "next-turn" game message has been received
        /// </summary>
        private void ScribblersClientManagerNextTurnGameMessageReceivedEvent()
        {
            ClearInternally();
            if (onNextTurnGameMessageReceived != null)
            {
                onNextTurnGameMessageReceived.Invoke();
            }
            OnNextTurnGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "line" game message has been received
        /// </summary>
        /// <param name="fromX">X component of line start position</param>
        /// <param name="fromY">Y component of line start position</param>
        /// <param name="toX">X component of line end position</param>
        /// <param name="toY">Y component of line end position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        private void ScribblersClientManagerLineGameMessageReceivedEvent(float fromX, float fromY, float toX, float toY, IColor color, float lineWidth)
        {
            Vector2 from = new Vector2(fromX, fromY);
            Vector2 to = new Vector2(toX, toY);
            drawCommandQueue.Enqueue(new DrawCommand(EDrawCommandType.Line, from, to, color, lineWidth));
            InvokeLineDrawEvent(from, to, color, lineWidth);
            if (onLineGameMessageReceived != null)
            {
                onLineGameMessageReceived.Invoke();
            }
            OnLineGameMessageReceived?.Invoke(fromX, fromY, toX, toY, color, lineWidth);
        }

        /// <summary>
        /// Gets invoked when a "fill" game message has been received
        /// </summary>
        /// <param name="positionX">X component of flood fill position</param>
        /// <param name="positionY">X component of flood fill position</param>
        /// <param name="color">Flood fill position</param>
        private void ScribblersClientManagerFillGameMessageReceivedEvent(float positionX, float positionY, IColor color)
        {
            Vector2 position = new Vector2(positionX, positionY);
            drawCommandQueue.Enqueue(new DrawCommand(EDrawCommandType.Fill, position, position, color, 0.0f));
            InvokeDrawingBoardFilledEvent(position, color);
            if (onFillGameMessageReceived != null)
            {
                onFillGameMessageReceived.Invoke();
            }
            OnFillGameMessageReceived?.Invoke(positionX, positionY, color);
        }

        /// <summary>
        /// Gets invoked when a "clear" game message has been received
        /// </summary>
        private void ScribblersClientManagerClearDrawingBoardGameMessageReceivedEvent()
        {
            drawCommandQueue = new ConcurrentQueue<DrawCommand>();
            ClearInternally();
            if (onClearDrawingBoardGameMessageReceived != null)
            {
                onClearDrawingBoardGameMessageReceived.Invoke();
            }
            OnClearDrawingBoardGameMessageReceived?.Invoke();
        }

        /// <summary>
        /// Gets invoked when a "your-turn" game message has been received
        /// </summary>
        /// <param name="words">Words to choose</param>
        private void ScribblersClientManagerYourTurnGameMessageReceivedEvent(IReadOnlyList<string> words)
        {
            drawCommandQueue = new ConcurrentQueue<DrawCommand>();
            ClearInternally();
            if (onYourTurnGameMessageReceived != null)
            {
                onYourTurnGameMessageReceived.Invoke();
            }
            OnYourTurnGameMessageReceived?.Invoke(words);
        }

        /// <summary>
        /// Subscribes to Scribble.rs client events
        /// </summary>
        protected override void SubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived += ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived += ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnLineGameMessageReceived += ScribblersClientManagerLineGameMessageReceivedEvent;
            ScribblersClientManager.OnFillGameMessageReceived += ScribblersClientManagerFillGameMessageReceivedEvent;
            ScribblersClientManager.OnClearDrawingBoardGameMessageReceived += ScribblersClientManagerClearDrawingBoardGameMessageReceivedEvent;
            ScribblersClientManager.OnYourTurnGameMessageReceived += ScribblersClientManagerYourTurnGameMessageReceivedEvent;
        }

        /// <summary>
        /// Unsubscribes from Scribble.rs client events
        /// </summary>
        protected override void UnsubscribeScribblersClientEvents()
        {
            ScribblersClientManager.OnReadyGameMessageReceived -= ScribblersClientManagerReadyGameMessageReceivedEvent;
            ScribblersClientManager.OnNextTurnGameMessageReceived -= ScribblersClientManagerNextTurnGameMessageReceivedEvent;
            ScribblersClientManager.OnLineGameMessageReceived -= ScribblersClientManagerLineGameMessageReceivedEvent;
            ScribblersClientManager.OnFillGameMessageReceived -= ScribblersClientManagerFillGameMessageReceivedEvent;
            ScribblersClientManager.OnClearDrawingBoardGameMessageReceived -= ScribblersClientManagerClearDrawingBoardGameMessageReceivedEvent;
            ScribblersClientManager.OnYourTurnGameMessageReceived -= ScribblersClientManagerYourTurnGameMessageReceivedEvent;
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="from">Line start position</param>
        /// <param name="to">Line end position</param>
        /// <param name="color">Line color</param>
        /// <param name="lineWidth">Line width</param>
        public void DrawLine(Vector2 from, Vector2 to, ScribblersSharp.Color color, float lineWidth)
        {
            if (IsPlayerDrawing)
            {
                DrawCommand draw_command = new DrawCommand(EDrawCommandType.Line, from, to, color, lineWidth);
                drawCommandQueue.Enqueue(draw_command);
                ScribblersClientManager.SendLineGameMessage(from.x, from.y, to.x, to.y, color, lineWidth);
                InvokeLineDrawEvent(from, to, color, lineWidth);
            }
        }

        /// <summary>
        /// Flood fills drawing board
        /// </summary>
        /// <param name="position">Flood fill posiiton</param>
        /// <param name="color">Flood fill color</param>
        public void Fill(Vector2 position, ScribblersSharp.Color color)
        {
            if (IsPlayerDrawing)
            {
                DrawCommand draw_command = new DrawCommand(EDrawCommandType.Fill, position, position, color, 0.0f);
                drawCommandQueue.Enqueue(draw_command);
                ScribblersClientManager.SendFillGameMessage(position.x, position.y, color);
                InvokeDrawingBoardFilledEvent(position, color);
            }
        }

        /// <summary>
        /// Clears drawing board
        /// </summary>
        public void Clear()
        {
            if (IsPlayerDrawing)
            {
                drawCommandQueue = new ConcurrentQueue<DrawCommand>();
                ClearInternally();
                ScribblersClientManager.SendClearDrawingBoardGameMessage();
                if (FindObjectOfType<DrawingToolInputControllerScript>(true) is DrawingToolInputControllerScript drawing_tool_input_controller && (drawing_tool_input_controller.DrawingTool == EDrawingTool.Eraser))
                {
                    drawing_tool_input_controller.DrawingTool = EDrawingTool.Pen;
                }
            }
        }

        /// <summary>
        /// Performs an update
        /// </summary>
        public void PerformUpdate()
        {
            UpdateTexture();
            if (RawImage)
            {
                RawImage.texture = texture;
            }
            ++relativeFrameCount;
            relativeFrameCount %= 2U;
            if (isTextureReadyForUpdate && (relativeFrameCount == 0U))
            {
                isTextureReadyForUpdate = false;
                texture.SetPixelData(bitmap, 0);
                texture.Apply(false);
            }
        }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out RawImage raw_image))
            {
                RawImage = raw_image;
            }
            else
            {
                Debug.LogError($"Required component of type \"{ nameof(UnityEngine.UI.RawImage) }\" is missing.", this);
            }
            if (TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
            }
            else
            {
                Debug.LogError($"Required component of type \"{ nameof(RectTransform) }\" is missing.", this);
            }
            UpdateTexture();
            isKeepingDrawThreadRunning = true;
            drawThread = new Thread
            (
                () =>
                {
                    while (isKeepingDrawThreadRunning)
                    {
                        while (drawCommandQueue.TryDequeue(out DrawCommand draw_command))
                        {
                            switch (draw_command.Type)
                            {
                                case EDrawCommandType.Fill:
                                    FillInternally(draw_command.From, draw_command.Color);
                                    break;
                                case EDrawCommandType.Line:
                                    DrawLineInternally(draw_command.From, draw_command.To, draw_command.Color, draw_command.LineWidth);
                                    break;
                            }
                            isTextureReadyForUpdate = true;
                        }
                    }
                }
            );
            drawThread.Start();
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update() => PerformUpdate();

        /// <summary>
        /// Gets invoked when object gets destroyed
        /// </summary>
        private void OnDestroy()
        {
            isKeepingDrawThreadRunning = false;
            drawThread?.Join();
            drawThread = null;
            isTextureReadyForUpdate = false;
            if (bitmap.IsCreated)
            {
                bitmap.Dispose();
            }
        }
    }
}
