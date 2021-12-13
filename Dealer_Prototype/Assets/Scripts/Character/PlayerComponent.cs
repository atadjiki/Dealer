using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerComponent : NPCComponent
{
    private static PlayerComponent _instance;

    public static PlayerComponent Instance { get { return _instance; } }

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
    }

    internal override IEnumerator DoInitialize()
    {
        _characterState = this.gameObject.AddComponent<CharacterStateComponent>();
        _characterState.SetCharacterID(spawnData.ID);
        _characterState.SetTeam(CharacterConstants.Team.Ally);

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

        ColorManager.Instance.SetObjectToColor(ModelPrefab, ColorManager.Instance.GetPlayerColor());

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

        yield return new WaitWhile(() => _selection == null);

        //idle to tart with 
        SetCurrentState(CharacterConstants.State.Idle);

        _selection.SetPossesed();

        yield return null;
    }

    public override void PerformSelect()
    {
        CameraManager.Instance.FocusOnCharacter(this);
        SetCurrentBehavior(CharacterConstants.Mode.Possesed);
        _selection.SetPossesed();
        GoToIdle();
    }

    public override void PerformUnselect()
    {
        CameraManager.Instance.UnFocus();
        SetCurrentBehavior(GetPreviousBehavior());
        _selection.SetUnposessed();
        GoToIdle();
    }

    public override void OnMouseClicked()
    {
        NPCManager.Instance.HandleNPCSelection(this);
    }
}
