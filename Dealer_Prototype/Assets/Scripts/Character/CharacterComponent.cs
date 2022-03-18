using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterAnimationComponent _characterAnimation;
    protected NavigatorComponent _navigator;
    protected CharacterCanvas _charCanvas;
    protected CharacterStateCanvas _charStateCanvas;
    protected Light _light;
    protected InteractionComponent _interaction;
    protected CharacterStateComponent _characterState;
    protected CharacterCameraRig _cameraRig;
    protected SelectionComponent _selection;
    protected ScheduleComponent _schedule;

    [Header("Character Setup")]

    private AIConstants.BehaviorType PreviousBehavior = AIConstants.BehaviorType.None;
    private AIConstants.BehaviorType CurrentBehavior = AIConstants.BehaviorType.None;

    public AIConstants.Mode CharacterMode = AIConstants.Mode.None;

    public AIConstants.AIState AIState = AIConstants.AIState.None;

    private AnimationConstants.Anim CurrentAnimation = AnimationConstants.Anim.Idle;

    [Header("Debug")]

    [SerializeField] protected AIConstants.UpdateState updateState = AIConstants.UpdateState.None;

    public AIConstants.UpdateState GetUpdateState() { return updateState; }

    [Range(0.0f, 10.0f)]
    public float IdleSeconds_Max = 5.0f;

    internal float moveRadius = 30;

    private bool bHasInitialized = false;

    protected SpawnData spawnData;

    public SpawnData GetSpawnData() { return spawnData; }

    //behavior queue
    private Queue<CharacterBehaviorScript> _behaviorQueue;
    private int _maxQueueSize = 3;

    internal virtual void Initialize(SpawnData _spawnData)
    {
        spawnData = _spawnData;
        StartCoroutine(DoInitialize());
    }

    internal virtual IEnumerator DoInitialize()
    {
        _characterState = this.gameObject.AddComponent<CharacterStateComponent>();
        _characterState.SetCharacterID(spawnData.ID);
        _characterState.SetTeam(spawnData.GetTeam());
        _characterState.SetGender(CharacterConstants.GetGenderBYID(spawnData.ID));

        //setup navigator
        GameObject NavigatorPrefab = PrefabFactory.CreatePrefab(RegistryID.Navigator, this.transform);
        _navigator = NavigatorPrefab.GetComponent<NavigatorComponent>();
        _navigator.SetCanMove(true);

        yield return new WaitWhile(() => _navigator == null);

        GameObject CameraRigPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterCameraRig, NavigatorPrefab.transform);
        _cameraRig = CameraRigPrefab.GetComponent<CharacterCameraRig>();

        yield return new WaitWhile(() => _cameraRig == null);

        //setup character model and attach to navigator
        GameObject ModelPrefab = PrefabFactory.GetCharacterPrefab(_characterState.GetID(), NavigatorPrefab.transform);
        // ModelPrefab.transform.parent = NavigtorPrefab.transform;
        _animator = ModelPrefab.GetComponent<Animator>();

        _characterAnimation = ModelPrefab.GetComponent<CharacterAnimationComponent>();
        _characterAnimation.SetSocket(_navigator.gameObject);

       // ColorManager.Instance.SetObjectToColor(ModelPrefab, ColorManager.Instance.GetColorByTeam(_characterState.GetTeam()));

        yield return new WaitWhile(() => _animator == null);

        GameObject CharacterLightPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterLight, ModelPrefab.transform);
        _light = CharacterLightPrefab.GetComponent<Light>();

        yield return new WaitWhile(() => _light == null);

        //attach a UI canvas to the model 
        GameObject CharCanvasPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterCanvas, ModelPrefab.transform);
        _charCanvas = CharCanvasPrefab.GetComponent<CharacterCanvas>();

        yield return new WaitWhile(() => _charCanvas == null);

        _charCanvas.Set_Text_ID(_characterState.GetID());

        //attach a UI canvas to the model 
        //GameObject CharStateCanvasPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterStateCanvas, ModelPrefab.transform);
        //_charStateCanvas = CharStateCanvasPrefab.GetComponent<CharacterStateCanvas>();

        //yield return new WaitWhile(() => _charStateCanvas == null);

        GameObject InteractionPrefab = PrefabFactory.CreatePrefab(RegistryID.Interaction, NavigatorPrefab.transform);
        _interaction = InteractionPrefab.GetComponent<InteractionComponent>();

        yield return new WaitWhile(() => _interaction == null);

        _interaction.MouseEnterEvent += OnMouseEnter;
        _interaction.MouseExitEvent += OnMouseExit;
        _interaction.MouseClickedEvent += OnMouseClicked;
        _interaction.CharPtr = this;

        GameObject SelectionPrefab = PrefabFactory.CreatePrefab(RegistryID.SelectionComponent, NavigatorPrefab.transform);
        _selection = SelectionPrefab.GetComponent<SelectionComponent>();
        _selection.SetUnposessed();

        yield return new WaitWhile(() => _selection == null);

        _behaviorQueue = new Queue<CharacterBehaviorScript>();

        SetCurrentBehavior(AIConstants.BehaviorType.None);

        FadeToAnimation(spawnData.InitialAnim, 0.0f, false);

        bHasInitialized = true;
    }

    public bool HasInitialized() { return bHasInitialized; }

    public void SetPositionRotation(Vector3 Position, Quaternion Rotation)
    {
        _navigator.gameObject.transform.position = Position;
        _navigator.gameObject.transform.rotation = Rotation;

    }
    public virtual void OnMouseEnter()
    {
        _charCanvas.Toggle(true);

        if(PlayableCharacterManager.Instance.IsPlayerLocked() == false)
        {
            if (CharacterMode == AIConstants.Mode.Selected)
                GameplayCanvas.Instance.SetInteractionTipTextContext(InteractableConstants.InteractionContext.Deselect);

            else
                GameplayCanvas.Instance.SetInteractionTipTextContext(InteractableConstants.InteractionContext.Select);
        }
        else if(CharacterMode != AIConstants.Mode.Selected)
        {
            GameplayCanvas.Instance.SetInteractionTipTextContext(InteractableConstants.InteractionContext.Talk);
        }
    }

    public virtual void OnMouseExit()
    {
        _charCanvas.Toggle(false);
        GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public virtual void OnMouseClicked()
    {
        if (PlayableCharacterManager.Instance.GetSelectedCharacter() != null)
        {
            PlayableCharacterManager.Instance.AttemptInteractWithPossesedCharacter(this);
        }
    }

    private void OnDestroy()
    {
        CharacterCameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public virtual void OnNewDestination(Vector3 destination) { }

    public virtual void OnDestinationReached(Vector3 destination) { }

    public void FadeToAnimation(AnimationConstants.Anim anim, float time, bool canMove)
    {
        string animString = AnimationConstants.FetchAnimString(_characterState, anim);

        if (_animator != null) _animator.CrossFade(animString, time);
        if(_navigator != null) _navigator.SetCanMove(canMove);
        SetCurrentAnimation(anim);

        DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "Fading to anim " + animString);
    }

    public AIConstants.BehaviorType GetCurrentBehavior() { return CurrentBehavior; }

    public void SetCurrentBehavior(AIConstants.BehaviorType NewBehavior)
    {
        PreviousBehavior = CurrentBehavior;
        CurrentBehavior = NewBehavior;
        if (_charCanvas != null) _charCanvas.Set_Text_Mode(CurrentBehavior.ToString());
        GameplayCanvas.Instance.SetBehaviorText(CurrentBehavior);
    }

    public AnimationConstants.Anim GetCurrentAnimation() { return CurrentAnimation; }

    public void SetCurrentAnimation(AnimationConstants.Anim anim)
    {
        CurrentAnimation = anim;
        GameplayCanvas.Instance.SetAnimationText(anim.ToString());
    }

    public void SetAIState(AIConstants.AIState state)
    {
        AIState = state;

        if(_charStateCanvas != null)
        {
            _charStateCanvas.SetText_State(AIState.ToString().ToLower().Trim());
        }  
    }

    internal void SetUpdateState(AIConstants.UpdateState newState)
    {
        updateState = newState;
    }

    public string GetID()
    {
        if (_interaction != null)
            return _characterState.GetID();
        else
            return "";
    }

    public virtual void PerformSelect()
    {
        CharacterCameraManager.Instance.SelectCharacterCamera(this);
        _selection.SetPossesed();
        _charCanvas.Toggle(true);
    }

    public virtual void PerformUnselect()
    {
        CharacterCameraManager.Instance.UnselectCharacterCamera();
        CurrentBehavior = PreviousBehavior;
        _selection.SetUnposessed();
        _charCanvas.Toggle(false);
    }

    public NavigatorComponent GetNavigatorComponent()
    {
        return _navigator;
    }

    public void AbortBehaviors()
    {
        foreach(CharacterBehaviorScript behavior in _behaviorQueue)
        {
            behavior.AbortBehavior();
        }

        _behaviorQueue.Clear();

        bool success;
        CharacterBehaviorScript behaviorScript = BehaviorHelper.IdleBehavior(this, out success);

        AddNewBehavior(behaviorScript);
    }

    public void AddNewBehavior(CharacterBehaviorScript behaviorScript)
    {
        if(_behaviorQueue.Count < _maxQueueSize)
        {
            _behaviorQueue.Enqueue(behaviorScript);

            ProcessBehaviorQueue();
        }
        else
        {
            Destroy(behaviorScript.gameObject);
        }
    }

    private void ProcessBehaviorQueue()
    {
        if (_behaviorQueue.Count > 0 && _behaviorQueue.Peek().GetBehaviorState() == CharacterBehaviorScript.BehaviorState.Ready)
        {
            _behaviorQueue.Peek().BeginBehavior();
        }

        GameplayCanvas.Instance.UpdateBehaviorQueue(_behaviorQueue);
    }

    public void OnBehaviorFinished(CharacterBehaviorScript finishedBehavior)
    {
        if (_behaviorQueue.Peek() == finishedBehavior)
        {
            _behaviorQueue.Dequeue();
            ProcessBehaviorQueue();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (updateState == AIConstants.UpdateState.None)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
            Gizmos.DrawIcon(this.transform.position + new Vector3(0, 0, 0.5f), "Icon_Person_Male.png"); ;
        }
    }
#endif
}
