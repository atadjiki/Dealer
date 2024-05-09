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

    private IEnumerator Coroutine_Setup(CharacterData data)
    {
        _ID = data.ID;

        //create a navigator for the character
        GameObject navigatorObject = Instantiate<GameObject>(data.Navigator, this.transform);

        _navigator = navigatorObject.GetComponent<CharacterNavigator>();
        _navigator.DestinationReachedCallback += OnDestinationReached;

        //load the model and animator for this character
        GameObject modelObject = Instantiate<GameObject>(data.Model, navigatorObject.transform);

        _model = modelObject.GetComponent<CharacterModel>();
        _animator = modelObject.GetComponent<CharacterAnimator>();

        //place a camera follow target for the character
        GameObject cameraFollowObject = Instantiate<GameObject>(data.CameraFollow, navigatorObject.transform);

        _cameraFollow = cameraFollowObject.GetComponent<CharacterCameraFollow>();

        yield return null;
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    public void MoveTo(Vector3 destination)
    {
        _navigator.MoveTo(destination);
        _animator.SetAnim(CharacterAnim.Moving);
    }

    public void OnDestinationReached(CharacterNavigator navigator)
    {
        _animator.SetAnim(CharacterAnim.Idle);

    }

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }
}
