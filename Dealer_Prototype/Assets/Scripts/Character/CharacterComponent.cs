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

    public CharacterConstants.BehaviorType PreviousBehavior = CharacterConstants.BehaviorType.None;
    public CharacterConstants.BehaviorType CurrentBehavior = CharacterConstants.BehaviorType.None;

    public CharacterConstants.Mode CharacterMode = CharacterConstants.Mode.None;

    [Header("Debug")]

    [SerializeField] protected CharacterConstants.UpdateState updateState = CharacterConstants.UpdateState.None;

    public CharacterConstants.UpdateState GetUpdateState() { return updateState; }

    [Range(0.0f, 10.0f)]
    public float IdleSeconds_Max = 5.0f;

    internal float moveRadius = 30;

    protected SpawnData spawnData;

    protected CharacterBehaviorScript _currentBehaviorScript;

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
        GameObject NavigtorPrefab = PrefabFactory.Instance.CreatePrefab(RegistryID.Navigator, this.transform);
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

        SetCurrentBehavior(CharacterConstants.BehaviorType.None);
    }

    public void SetPositionRotation(Vector3 Position, Quaternion Rotation)
    {
        _navigator.gameObject.transform.position = Position;
        _navigator.gameObject.transform.rotation = Rotation;

    }
    public virtual void OnMouseEnter()
    {
        _charCanvas.Toggle(true);

        if (CharacterMode == CharacterConstants.Mode.Selected)
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Deselect);

        else
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Select);
    }

    public virtual void OnMouseExit()
    {
        _charCanvas.Toggle(false);
        GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public virtual void OnMouseClicked() { }

    private void OnDestroy()
    {
        CharacterCameraManager.Instance.UnRegisterCharacterCamera(this);
    }

    public virtual void OnNewDestination(Vector3 destination)
    {
        DebugExtension.DebugWireSphere(destination, Color.green, 1, 1, false);
    }

    public virtual void OnDestinationReached(Vector3 destination)
    {
        DebugExtension.DebugWireSphere(destination, Color.green, 1, 1, false);
    }

    public void ToIdle()
    {
        _animator.CrossFade(AnimationConstants.Idle, 0.5f);
        _navigator.SetCanMove(false);
    }

    public void ToMoving()
    {
        _animator.CrossFade(AnimationConstants.Walking, 0.1f);
        _navigator.SetCanMove(true);
    }

    public void ToInteracting()
    {
        StopAllCoroutines();
        _animator.CrossFade(AnimationConstants.ButtonPush, 0.3f);
        _navigator.SetCanMove(false);

    }

    private void FadeToAnimation(string animation, float time)
    {
        if (_animator != null) _animator.CrossFade(animation, time);
    }

    public void SetCurrentBehavior(CharacterConstants.BehaviorType NewBehavior)
    {
        PreviousBehavior = CurrentBehavior;
        CurrentBehavior = NewBehavior;
        if (_charCanvas != null) _charCanvas.Set_Text_Mode(CurrentBehavior.ToString());
    }

    internal void SetUpdateState(CharacterConstants.UpdateState newState)
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (updateState == CharacterConstants.UpdateState.None)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, 0.5f);
            Gizmos.DrawIcon(this.transform.position + new Vector3(0, 0, 0.5f), "Icon_Person_Male.png"); ;
        }
    }
#endif
}
