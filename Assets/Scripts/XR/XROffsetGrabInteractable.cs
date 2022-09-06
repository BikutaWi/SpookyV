using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPosition;

    private Quaternion initialAttachLocalRotation;

    /// <summary>
    /// Function called when the game is started
    /// </summary>
    private void Start()
    {
        if(!attachTransform)
        {
            GameObject grabGameObject = new GameObject("Pivot");
            grabGameObject.transform.SetParent(transform, false);
            attachTransform = grabGameObject.transform;
        }

        initialAttachLocalPosition = attachTransform.localPosition;
        initialAttachLocalRotation = attachTransform.transform.rotation;
    }

    /// <summary>
    /// Need to use the obsolete version of this function because of the version of Unity wich don't work with the new one
    /// This function allows the upper part of the weapon to be pulled of and replaced after 
    /// </summary>
    /// <param name="interactor">interactor's name in the player</param>
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = initialAttachLocalPosition;
            attachTransform.localRotation = initialAttachLocalRotation;
        }

        base.OnSelectEntered(interactor);
    }
}
