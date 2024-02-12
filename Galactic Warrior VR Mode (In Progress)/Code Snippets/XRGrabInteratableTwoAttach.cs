using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Used to initialise Right hand and Left hand Grab transforms

public class XRGrabInteratableTwoAttach : XRGrabInteractable
{
    [SerializeField]Transform _leftAttachTransform, _rightAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = _leftAttachTransform;
            Debug.Log("leftAP");
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = _rightAttachTransform;
            Debug.Log("RightAP");
        }

        base.OnSelectEntered(args);
        Debug.Log("Selected arg:", args.interactorObject.transform.gameObject);
    }

}
