using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected Animator _animator;
    protected NavigatorComponent _navigator;
    protected CharacterCanvas _charCanvas;
    protected InteractionComponent _interaction;
    protected CharacterStateComponent _characterState;
    protected CharacterCameraRig _cameraRig;
    protected SelectionComponent _selection;


    [Header("Character Setup")]

    private CharacterConstants.Mode PreviousBehavior = CharacterConstants.Mode.None;
    private CharacterConstants.Mode CurrentBehavior = CharacterConstants.Mode.None;

    public CharacterConstants.Mode GetCurrentBehavior() { return CurrentBehavior; }
    public CharacterConstants.Mode GetPreviousBehavior() { return PreviousBehavior; }

    //Allowed behaviors
    [SerializeField] protected List<CharacterConstants.Behavior> AllowedBehaviors;
    [SerializeField] protected List<InteractableConstants.InteractableID> AllowedInteractables;

    public List<CharacterConstants.Behavior> GetAllowedBehaviors() { return AllowedBehaviors; }
    public List<InteractableConstants.InteractableID> GetAllowedInteractables() { return AllowedInteractables; }

    [Header("Debug")]

    [SerializeField] protected CharacterConstants.UpdateState updateState = CharacterConstants.UpdateState.None;
    [SerializeField] protected CharacterConstants.State CurrentState;

    internal CharacterConstants.ActionType LastAction = CharacterConstants.ActionType.None;
    internal Coroutine ActionCoroutine;

    internal float moveRadius = 30;

    protected SpawnData spawnData;

    internal virtual void Initialize(SpawnData _spawnData)
    {
        spawnData = _spawnData;
        StartCoroutine(DoInitialize());
    }

    internal virtual IEnumerator DoInitialize()
    {
        _characterState = this.gameObject.AddComponent<CharacterStateComponent>();
        _characterState.SetCharacterID(spawnData.ID);
        _characterState.SetTeam(spawnData.Team);

        //setup navigator
        GameObject NavigtorPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Navigator, this.transform) ;
        _navigator = NavigtorPrefab.GetComponent<NavigatorComponent>();

        yield return new WaitWhile(() => _navigator == null);

        GameObject CameraRigPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.CharacterCameraRig, NavigtorPrefab.transform);
        _cameraRig = CameraRigPrefab.GetComponent<CharacterCameraRig>();

        yield return new WaitWhile(() => _cameraRig == null);

        //setup character model and attach to navigator
        GameObject ModelPrefab = PrefabFactory.Instance.GetCharacterPrefab(_characterState.GetID(), NavigtorPrefab.transform);
       // ModelPrefab.transform.parent = NavigtorPrefab.transform;
        _animator = ModelPrefab.GetComponent<Animator>();

        ColorManager.Instance.SetObjectToColor(ModelPrefab, ColorManager.Instance.GetColorByTeam(_characterState.GetTeam()));

        yield return new WaitWhile(() => _animator == null);

        //attach a UI canvas to the model 
        GameObject CanvasPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.CharacterCanvas, ModelPrefab.transform);
        _charCanvas = CanvasPrefab.GetComponent<CharacterCanvas>();

        yield return new WaitWhile(() => _charCanvas == null);

        _charCanvas.Set_Text_ID(_characterState.GetID());

        GameObject InteractionPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Interaction, NavigtorPrefab.transform);
        _interaction = InteractionPrefab.GetComponent<InteractionComponent>();

        yield return new WaitWhile(() => _interaction == null);

        _interaction.MouseEnterEvent += OnMouseEnter;
        _interaction.MouseExitEvent += OnMouseExit;
        _interaction.MouseClickedEvent += OnMouseClicked;

        GameObject SelectionPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.SelectionComponent, NavigtorPrefab.transform);
        _selection = SelectionPrefab.GetComponent<SelectionComponent>();
        _selection.SetUnposessed();

        yield return new WaitWhile(() => _selection == null);

        //idle to tart with 
        SetCurrentState(CharacterConstants.State.Idle);
    }

    public void SetPositionRotation(Vector3 Position, Quaternion Rotation)
    {
        _navigator.gameObject.transform.position = Position;
        _navigator.gameObject.transform.rotation = Rotation;

    }
    public virtual void OnMouseEnter()
    {
        _charCanvas.Toggle(true);    }

    public virtual void OnMouseExit()
    {
        _charCanvas.Toggle(false);
    }

    public virtual void OnMouseClicked() { }

    private void OnDestroy()
    {
        CharacterCameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public virtual void OnNewDestination(Vector3 destination) { }
    public virtual void OnDestinationReached(Vector3 destination) { }

    public virtual void GoToIdle()
    {
        if (ActionCoroutine != null) StopCoroutine(ActionCoroutine);

        ToIdle();
    }

    public void ToIdle()
    {
        SetCurrentState(CharacterConstants.State.Idle);
        _animator.CrossFade(AnimationConstants.Idle, 0.5f);
        _navigator.SetCanMove(false);
    }

    public void ToMoving()
    {
        SetCurrentState(CharacterConstants.State.Moving);
        _animator.CrossFade(AnimationConstants.Walking, 0.1f);
        _navigator.SetCanMove(true);
    }

    public void ToInteracting()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Interacting);
        _animator.CrossFade(AnimationConstants.ButtonPush, 0.3f);
        _navigator.SetCanMove(false);

    }

    public void ToTalking()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Talking);
        _animator.CrossFade(AnimationConstants.Talking, 0.3f);
        _navigator.SetCanMove(false);
    }

    public void ToSitting()
    {
        StopAllCoroutines();
        SetCurrentState(CharacterConstants.State.Sitting);
        FadeToAnimation(AnimationConstants.Male_Sitting_2, 0.35f);
        _navigator.SetCanMove(false);
    }

   

    private void FadeToAnimation(string animation, float time)
    {
        if(_animator != null) _animator.CrossFade(animation, time);
    }

    internal void SetCurrentState(CharacterConstants.State newState)
    {
        CurrentState = newState;
        if (_charCanvas != null) _charCanvas.Set_Text_State(CurrentState.ToString());
    }

    public void SetCurrentBehavior(CharacterConstants.Mode NewMode)
    {
        PreviousBehavior = CurrentBehavior;
        CurrentBehavior = NewMode;
        if(_charCanvas != null) _charCanvas.Set_Text_Mode(CurrentBehavior.ToString());
    }

    public CharacterConstants.State GetCurrentState() { return CurrentState; }

    internal void SetUpdateState(CharacterConstants.UpdateState newState)
    {
        updateState = newState;
    }

    public CharacterConstants.UpdateState GetUpdateState() { return updateState; }

    public CharacterConstants.ActionType GetLastAction() { return LastAction; }

    public bool MoveToRandomLocation()
    {
        return _navigator.MoveToRandomLocation();
    }

    public bool MoveToLocation(Vector3 Location)
    {
        return _navigator.MoveToLocation(Location);
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
        SetCurrentBehavior(CharacterConstants.Mode.Selected);
        _selection.SetPossesed();
    }

    public virtual void PerformUnselect()
    {
        CharacterCameraManager.Instance.UnselectCharacterCamera();
        SetCurrentBehavior(GetPreviousBehavior());
        _selection.SetUnposessed();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(updateState == CharacterConstants.UpdateState.None)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
            Gizmos.DrawIcon(this.transform.position + new Vector3(0,0,0.5f), "Icon_Person_Male.png"); ;
        }
    }
#endif
}
