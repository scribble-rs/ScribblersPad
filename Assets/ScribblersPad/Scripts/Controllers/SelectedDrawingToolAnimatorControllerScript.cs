using ScribblersSharp;
using UnityEngine;

namespace ScribblersPad.Controllers
{
    [RequireComponent(typeof(Animator))]
    public class SelectedDrawingToolAnimatorControllerScript : AScribblersClientControllerScript
    {
        private static readonly int selectedDrawingToolHash = Animator.StringToHash("selectedDrawingTool");

        private EDrawingTool lastSelectedDrawingTool = EDrawingTool.Pen;

        private bool lastIsPlayerAllowedToDraw;

        public Animator SelectedDrawingToolAnimator { get; private set; }

        public DrawingToolInputControllerScript DrawingToolInputController { get; private set; }

        protected override void SubscribeScribblersClientEvents()
        {
            // ...
        }

        protected override void UnsubscribeScribblersClientEvents()
        {
            // ...
        }

        protected override void Start()
        {
            base.Start();
            if (TryGetComponent(out Animator selected_drawing_tool_animator))
            {
                SelectedDrawingToolAnimator = selected_drawing_tool_animator;
            }
            else
            {
                Debug.LogError($"Please attach an \"{ nameof(Animator) }\" component to this game object.", this);
            }
            DrawingToolInputController = FindObjectOfType<DrawingToolInputControllerScript>(true);
            if (!DrawingToolInputController)
            {
                Debug.LogError($"Failed to find a game object in scene with a \"{ nameof(DrawingToolInputControllerScript) }\" component attached.", this);
            }
        }

        private void Update()
        {
            if (SelectedDrawingToolAnimator && DrawingToolInputController && ScribblersClientManager && ScribblersClientManager.Lobby is ILobby lobby)
            {
                EDrawingTool selected_drawing_tool = DrawingToolInputController.DrawingTool;
                bool is_player_allowed_to_draw = lobby.IsPlayerAllowedToDraw;
                if ((lastSelectedDrawingTool != selected_drawing_tool) || (lastIsPlayerAllowedToDraw != is_player_allowed_to_draw))
                {
                    lastSelectedDrawingTool = selected_drawing_tool;
                    lastIsPlayerAllowedToDraw = is_player_allowed_to_draw;
                    SelectedDrawingToolAnimator.SetInteger(selectedDrawingToolHash, is_player_allowed_to_draw ? (int)selected_drawing_tool : -1);
                }
            }
        }
    }
}
