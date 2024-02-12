using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Used to initialise Right hand and Left hand Grab transforms: Method 2

public class XRGrabInteractableTwoAttached : XRGrabInteractable
{
    public Transform leftAttachedTransform;
    public Transform rightAttachedTransform;

    public override Transform GetAttachTransform(IXRInteractor interactor)
    {
        Transform i_attachTransform = null;

        if (interactor.transform.CompareTag("LeftHand"))
        {
            i_attachTransform = leftAttachedTransform;
        }
        if (interactor.transform.CompareTag("RightHand"))
        {
            i_attachTransform = rightAttachedTransform;
        }
        return i_attachTransform != null ? i_attachTransform : base.GetAttachTransform(interactor);
    }
}