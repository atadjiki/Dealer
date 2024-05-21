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

    //construct the character using their defined data
    public IEnumerator Coroutine_Setup(CharacterData data)
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
        EnvironmentUtil.Scan();
    }

    public void Teleport(Vector3 destination)
    {
        _navigator.Teleport(destination);
    }

    public Vector3 GetWorldLocation()
    {
        return _navigator.GetWorldLocation();
    }

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }

    public void ToggleVisibility(bool flag)
    {
        _model.ToggleVisibility(flag);
    }

    public bool IsAlive()
    {
        return true;
    }

    public bool IsDead()
    {
        return false;
    }

    public bool HasActionPoints()
    {
        return true;
    }

    public void DecrementActionPoints(int amount)
    {

    }

    public CharacterID GetID()
    {
        return _ID;
    }
}
