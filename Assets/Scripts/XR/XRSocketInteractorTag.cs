using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class allowing to place objects only if they have a specific tag
/// </summary>
public class XRSocketInteractorTag : XRSocketInteractor
{
    public string tag;

    /// <summary>
    /// Methods called when player try to put a gameobject somewhere (with interactor)
    /// </summary>
    /// <param name="interactable">Name of interactable's gameobject</param>
    /// <returns></returns>
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(tag);
    }
}
