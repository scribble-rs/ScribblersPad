using ScribblersSharp;
using System;
using System.Collections.Generic;
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
        /// Minimal drawing frame time
        /// </summary>
        [SerializeField]
        private long minimalDrawingFrameTime = 1000L / 60L;

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
        /// Input draw command queue
        /// </summary>
        private readonly Queue<DrawCommand> inputDrawCommandQueue = new Queue<DrawCommand>();

        /// <summary>
        /// Output draw command queue
        /// </summary>
        private readonly Queue<DrawCommand> outputDrawCommandQueue = new Queue<DrawCommand>();

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Bitmap
        /// </summary>
        private Color32[] bitmap;

        /// <summary>
        /// Update tick stopwatch
        /// </summary>
        private System.Diagnostics.Stopwatch updateTickStopwatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Bitmap
        /// </summary>
        private Color32[] Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    bitmap = texture ? texture.GetPixels32() : Array.Empty<Color32>();
                }
                else if (bitmap.Length != (texture.width * texture.height))
                {
                    bitmap = texture.GetPixels32();
                }
                return bitmap;
            }
        }

        /// <summary>
        /// Minimal drawing frame time
        /// </summary>
        public long MinimalDrawingFrameTime
        {
            get => minimalDrawingFrameTime;
            set => minimalDrawingFrameTime = value;
        }

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
            ref Color32 current_color = ref bitmap[positionX + (positionY * textureWidth)];
            if ((current_color.r == targetColor.r) && (current_color.g == targetColor.g) && (current_color.b == targetColor.b))
            {
                current_color = newColor;
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
        private void DrawLineInternally(Vector2 from, Vector2 to, Color32 color, float lineWidth)
        {
            if (texture)
            {
                Color32[] bitmap = Bitmap;
                float max_axial_distance = Mathf.Max(Mathf.Abs(to.x - from.x), Mathf.Abs(to.y - from.y));
                Vector2Int texture_size = new Vector2Int(texture.width, texture.height);
                float half_line_width = lineWidth * 0.5f;
                Vector2Int origin = new Vector2Int(
                    Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(to.x, from.x) - half_line_width), 0, texture_size.x),
                    Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(to.y, from.y) - half_line_width), 0, texture_size.y));
                Vector2Int size = new Vector2Int(
                    Mathf.Clamp(Mathf.CeilToInt(Mathf.Abs(to.x - from.x) + lineWidth) + 1, 0, texture_size.x - origin.x),
                    Mathf.Clamp(Mathf.CeilToInt(Mathf.Abs(to.y - from.y) + lineWidth) + 1, 0, texture_size.y - origin.y));
                Parallel.For(0, size.x * size.y, (index) =>
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
                        float height_squared = Mathf.Abs((s * (s - a) * (s - b) * (s - c)) / (a_squared * 0.25f));
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
                        bitmap[position.x + (texture_size.x * position.y)] = color;
                    }
                });
            }
            if (onLineDrawn != null)
            {
                onLineDrawn.Invoke();
            }
            OnLineDrawn?.Invoke(from, to, color, lineWidth);
        }

        /// <summary>
        /// Flood fills internally
        /// </summary>
        /// <param name="position">Flood fill position</param>
        /// <param name="color">Flood fill color</param>
        private void FillInternally(Vector2 position, Color32 color)
        {
            if (texture)
            {
                Color32[] bitmap = Bitmap;
                Vector2Int texture_size = new Vector2Int(texture.width, texture.height);
                Vector2Int integer_position = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                if ((integer_position.x >= 0) && (integer_position.y >= 0) && (integer_position.x < texture_size.x) && (integer_position.y < texture_size.y))
                {
                    int target_index = integer_position.x + (integer_position.y * texture_size.x);
                    if (target_index < bitmap.Length)
                    {
                        Color32 target_color = bitmap[target_index];
                        if ((color.r != target_color.r) || (color.g != target_color.g) || (color.b != target_color.b))
                        {
                            Stack<Vector2Int> stack = new Stack<Vector2Int>();
                            stack.Push(integer_position);
                            while (stack.Count > 0)
                            {
                                Vector2Int current_position = stack.Pop();
                                int top_bound;
                                int bottom_bound;
                                int left_bound;
                                int right_bound;
                                bitmap[current_position.x + (current_position.y * texture_size.x)] = color;
                                for (top_bound = current_position.y + 1; (top_bound < texture_size.y) && SetColorIfColorIsTargeted(current_position.x, top_bound, ref target_color, ref color, texture_size.x); top_bound++)
                                {
                                    // ...
                                }
                                top_bound--;
                                for (bottom_bound = current_position.y - 1; (bottom_bound >= 0) && SetColorIfColorIsTargeted(current_position.x, bottom_bound, ref target_color, ref color, texture_size.x); bottom_bound--)
                                {
                                    // ...
                                }
                                bottom_bound++;
                                for (left_bound = current_position.x - 1; (left_bound >= 0) && SetColorIfColorIsTargeted(left_bound, current_position.y, ref target_color, ref color, texture_size.x); left_bound--)
                                {
                                    // ...
                                }
                                left_bound++;
                                for (right_bound = current_position.x + 1; (right_bound < texture_size.x) && SetColorIfColorIsTargeted(right_bound, current_position.y, ref target_color, ref color, texture_size.x); right_bound++)
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
                                            PushPositionIfColorIsTargeted(position_x, current_position.y - 1, ref target_color, texture_size.x, stack);
                                        }
                                        if ((current_position.y + 1) < texture_size.y)
                                        {
                                            PushPositionIfColorIsTargeted(position_x, current_position.y + 1, ref target_color, texture_size.x, stack);
                                        }
                                    }
                                }
                                for (int position_y = bottom_bound; position_y <= top_bound; position_y++)
                                {
                                    if (Mathf.Abs(current_position.y - position_y) > 1)
                                    {
                                        if (current_position.x > 0)
                                        {
                                            PushPositionIfColorIsTargeted(current_position.x - 1, position_y, ref target_color, texture_size.x, stack);
                                        }
                                        if ((current_position.x + 1) < texture_size.x)
                                        {
                                            PushPositionIfColorIsTargeted(current_position.x + 1, position_y, ref target_color, texture_size.x, stack);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (onDrawingBoardFilled != null)
            {
                onDrawingBoardFilled.Invoke();
            }
            OnDrawingBoardFilled?.Invoke(position, color);
        }

        /// <summary>
        /// Clears internally
        /// </summary>
        private void ClearInternally()
        {
            if (texture)
            {
                Color32[] bitmap = Bitmap;
                System.Drawing.Color canvas_color = ScribblersClientManager.Lobby.CanvasColor;
                Color32 clear_color = new Color32(canvas_color.R, canvas_color.G, canvas_color.B, canvas_color.A);
                Parallel.For(0, bitmap.Length, (index) => bitmap[index] = clear_color);
                texture.SetPixels32(bitmap);
                texture.Apply();
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
                    texture = new Texture2D((int)lobby.DrawingBoardBaseWidth, (int)(lobby.DrawingBoardBaseHeight), TextureFormat.ARGB32, false, true);
                }
                else if ((texture.width != lobby.DrawingBoardBaseWidth) || (texture.height != lobby.DrawingBoardBaseHeight))
                {
                    texture = new Texture2D((int)lobby.DrawingBoardBaseWidth, (int)(lobby.DrawingBoardBaseHeight), TextureFormat.ARGB32, false, true);
                }
            }
        }

        /// <summary>
        /// Gets invoked when a "ready" game message has been received
        /// </summary>
        private void ScribblersClientManagerReadyGameMessageReceivedEvent()
        {
            UpdateTexture();
            ClearInternally();
            foreach (ScribblersSharp.IDrawCommand draw_command in ScribblersClientManager.Lobby.CurrentDrawing)
            {
                inputDrawCommandQueue.Enqueue(new DrawCommand(draw_command.Type, new Vector2(draw_command.FromX, draw_command.FromY), new Vector2(draw_command.ToX, draw_command.ToY), new Color32(draw_command.Color.R, draw_command.Color.G, draw_command.Color.B, 0xFF), draw_command.LineWidth));
            }
            if (onReadyGameMessageReceived != null)
            {
                onReadyGameMessageReceived.Invoke();
            }
            OnReadyGameMessageReceived?.Invoke();
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
        private void ScribblersClientManagerLineGameMessageReceivedEvent(float fromX, float fromY, float toX, float toY, System.Drawing.Color color, float lineWidth)
        {
            inputDrawCommandQueue.Enqueue(new DrawCommand(EDrawCommandType.Line, new Vector2(fromX, fromY), new Vector2(toX, toY), new Color32(color.R, color.G, color.B, 0xFF), lineWidth));
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
        private void ScribblersClientManagerFillGameMessageReceivedEvent(float positionX, float positionY, System.Drawing.Color color)
        {
            Vector2 position = new Vector2(positionX, positionY);
            inputDrawCommandQueue.Enqueue(new DrawCommand(EDrawCommandType.Fill, position, position, new Color32(color.R, color.G, color.B, 0xFF), 0.0f));
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
            inputDrawCommandQueue.Clear();
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
            inputDrawCommandQueue.Clear();
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
        public void DrawLine(Vector2 from, Vector2 to, Color32 color, float lineWidth)
        {
            if (IsPlayerDrawing)
            {
                DrawCommand draw_command = new DrawCommand(EDrawCommandType.Line, from, to, color, lineWidth);
                inputDrawCommandQueue.Enqueue(draw_command);
                outputDrawCommandQueue.Enqueue(draw_command);
            }
        }

        /// <summary>
        /// Flood fills drawing board
        /// </summary>
        /// <param name="position">Flood fill posiiton</param>
        /// <param name="color">Flood fill color</param>
        public void Fill(Vector2 position, Color32 color)
        {
            if (IsPlayerDrawing)
            {
                DrawCommand draw_command = new DrawCommand(EDrawCommandType.Fill, position, position, color, 0.0f);
                inputDrawCommandQueue.Enqueue(draw_command);
                outputDrawCommandQueue.Enqueue(draw_command);
            }
        }

        /// <summary>
        /// Clears drawing board
        /// </summary>
        public void Clear()
        {
            if (IsPlayerDrawing)
            {
                inputDrawCommandQueue.Clear();
                outputDrawCommandQueue.Clear();
                ClearInternally();
                ScribblersClientManager.SendClearDrawingBoardGameMessageAsync();
            }
        }

        /// <summary>
        /// Processes next draw command in queue
        /// </summary>
        public void ProcessNextDrawCommandInQueue()
        {
            bool is_drawing = (inputDrawCommandQueue.Count > 0) || (outputDrawCommandQueue.Count > 0);
            long drawing_time = 0L;
            UpdateTexture();
            if (RawImage)
            {
                RawImage.texture = texture;
            }
            if (is_drawing)
            {
                while (is_drawing)
                {
                    is_drawing = false;
                    updateTickStopwatch.Start();
                    if (inputDrawCommandQueue.Count > 0)
                    {
                        DrawCommand draw_command = inputDrawCommandQueue.Dequeue();
                        switch (draw_command.Type)
                        {
                            case EDrawCommandType.Fill:
                                FillInternally(draw_command.From, draw_command.Color);
                                break;
                            case EDrawCommandType.Line:
                                DrawLineInternally(draw_command.From, draw_command.To, draw_command.Color, draw_command.LineWidth);
                                break;
                        }
                        is_drawing = inputDrawCommandQueue.Count > 0;
                    }
                    if (outputDrawCommandQueue.Count > 0)
                    {
                        DrawCommand draw_command = outputDrawCommandQueue.Dequeue();
                        switch (draw_command.Type)
                        {
                            case EDrawCommandType.Fill:
                                ScribblersClientManager.SendFillGameMessageAsync(draw_command.From.x, draw_command.From.y, System.Drawing.Color.FromArgb(0xFF, draw_command.Color.r, draw_command.Color.g, draw_command.Color.b));
                                break;
                            case EDrawCommandType.Line:
                                ScribblersClientManager.SendLineGameMessageAsync(draw_command.From.x, draw_command.From.y, draw_command.To.x, draw_command.To.y, System.Drawing.Color.FromArgb(0xFF, draw_command.Color.r, draw_command.Color.g, draw_command.Color.b), draw_command.LineWidth);
                                break;
                        }
                        is_drawing = is_drawing || (outputDrawCommandQueue.Count > 0);
                    }
                    updateTickStopwatch.Stop();
                    drawing_time += updateTickStopwatch.ElapsedMilliseconds;
                    is_drawing = is_drawing && (drawing_time < minimalDrawingFrameTime);
                }
                texture.SetPixels32(bitmap);
                texture.Apply();
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
            ClearInternally();
        }

        /// <summary>
        /// Gets invoked when script performs an update
        /// </summary>
        private void Update() => ProcessNextDrawCommandInQueue();
    }
}
