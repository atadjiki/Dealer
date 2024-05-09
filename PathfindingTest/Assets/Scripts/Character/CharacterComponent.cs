using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterComponent : MonoBehaviour
{
    private CharacterID _ID;

    private CharacterNavigator _navigator;

    private CharacterAnimator _animator;
    private CharacterModel _model;

    private CharacterCameraFollow _cameraFollow;

    public void Setup(CharacterData data)
    {
        StartCoroutine(Coroutine_Setup(data));
    }

    //construct the character using their defined data
    private IEnumerator Coroutine_Setup(CharacterData data)
    {
        _ID = data.ID;

        //create a navigator for the character
        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.parent = this.transform;
        _navigator = navigatorObject.AddComponent<CharacterNavigator>();
        yield return new WaitUntil( () => _navigator != null );
        _navigator.DestinationReachedCallback += OnDestinationReached;
        _navigator.SetSpeed(data.MovementSpeed);

        //load the model and animator for this character
        GameObject modelObject = Instantiate<GameObject>(data.Model, navigatorObject.transform);
        _model = modelObject.GetComponent<CharacterModel>();
        yield return new WaitUntil(() => _model != null);
        _animator = modelObject.GetComponent<CharacterAnimator>();
        yield return new WaitUntil(() => _animator != null);

        //place a camera follow target for the character
        GameObject cameraFollowObject = new GameObject("Camera Follow");
        cameraFollowObject.transform.parent = navigatorObject.transform;
        cameraFollowObject.transform.localPosition = data.CameraFollowOffset;
        _cameraFollow = cameraFollowObject.AddComponent<CharacterCameraFollow>();
        yield return new WaitUntil(() => _cameraFollow != null);

        Vector3 spawnLocation = EncounterUtil.GetRandomTile();
        Teleport(spawnLocation);

        Debug.Log("Character " + data.ID + " spawned at " + spawnLocation.ToString());
        yield return null;
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    //Movement interface
    public void MoveTo(Vector3 destination)
    {
        _navigator.MoveTo(destination);
        _animator.SetAnim(CharacterAnim.MOVING);
    }

    public void OnDestinationReached(CharacterNavigator navigator)
    {
        _animator.SetAnim(CharacterAnim.IDLE);

    }

    public void Teleport(Vector3 destination)
    {
        _navigator.Teleport(destination);
    }

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }
}
