using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BehaviorCanvas behaviorCanvas;
    [SerializeField] private ConversationCanvas conversationCanvas;

    public enum UIState { None, Gameplay, Conversation };

    private UIState currentState = UIState.None;

    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    public void SetState(UIState newState)
    {
        ToggleCurrentCanvas(false);
        currentState = newState;
        ToggleCurrentCanvas(true);
    }

    private void Build()
    {
        behaviorCanvas.Reset();
        conversationCanvas.Reset();

        GameState.Instance.onStateChangedDelegate += OnStateChanged;

        OnStateChanged(GameState.Instance.GetState());
    }

    private void OnStateChanged(GameState.State newState)
    {
        if(newState == GameState.State.GamePlay)
        {
            SetState(UIState.Gameplay);
        }
        else if(newState == GameState.State.Conversation)
        {
            SetState(UIState.Conversation);
        }
        else
        {
            SetState(UIState.None);
        }
    }

    private void ToggleCurrentCanvas(bool flag)
    {
        if(GetCurrentCanvas() != null)
        {
            GetCurrentCanvas().Toggle(flag);
        }
    }

    private GameCanvas GetCurrentCanvas()
    {
        switch(currentState)
        {
            case UIState.Conversation:
                return conversationCanvas;
            case UIState.Gameplay:
                return behaviorCanvas;
        }

        return null;
    }

    public void HandleEvent(object context)
    {
        GameCanvas currentCanvas = GetCurrentCanvas();

        if(currentCanvas == null) { return; }

        System.Type type = context.GetType();

        if(type == typeof(InteractableConstants.InteractionContext))
        {
            currentCanvas.HandleEvent_InteractionContext((InteractableConstants.InteractionContext)context);
        }
        else if(type == typeof(UI.Events))
        {
            UI.Events uiEvent = (UI.Events)context;

            switch(uiEvent)
            {
                case UI.Events.Clear:
                    currentCanvas.Clear();
                    return;
                case UI.Events.CharacterDeselected:
                    currentCanvas.HandleEvent_CharacterDeselected();
                    return;
            }
        }
    }

    public void HandleEvent(object context, object contextParams)
    {
        GameCanvas currentCanvas = GetCurrentCanvas();

        if (currentCanvas == null) { return; }

        System.Type type = context.GetType();

        if (type == typeof(UI.Events))
        {
            UI.Events uiEvent = (UI.Events)context;

            switch (uiEvent)
            {
                case UI.Events.SetBehaviorText:
                    currentCanvas.HandleEvent_SetBehaviorText((AIConstants.BehaviorType)contextParams);
                    return;
                case UI.Events.SetAnimText:
                    currentCanvas.HandleEvent_SetAnimText((AnimationConstants.Anim)contextParams);
                    return;
                case UI.Events.UpdateBehaviorQueue:
                    currentCanvas.HandleEvent_UpdateBehaviorQueue((Queue<CharacterBehaviorScript>)contextParams);
                    return;
                case UI.Events.CharacterLine_Begin:
                    currentCanvas.HandleEvent_CharacterLineBegin((Dialogue)contextParams);
                    return;
                case UI.Events.CharacterSelected:
                    currentCanvas.HandleEvent_CharacterSelected((CharacterComponent)contextParams);
                    return;

            }
        }
    }
}
