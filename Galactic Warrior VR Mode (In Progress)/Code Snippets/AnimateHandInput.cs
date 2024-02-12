using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Used to animate VR hands

public class AnimateHandInput : MonoBehaviour
{
    public Animator _handAnimator;
    public InputActionProperty _pinchAnimationAction;
    public InputActionProperty _gripAnimationAction;

    // Update is called once per frame
    void Update()
    {
        float _triggerValue = _pinchAnimationAction.action.ReadValue<float>();
        _handAnimator.SetFloat("Trigger", _triggerValue);
       
        float _gripValue = _gripAnimationAction.action.ReadValue<float>();
        _handAnimator.SetFloat("Grip", _gripValue);
    }
}
