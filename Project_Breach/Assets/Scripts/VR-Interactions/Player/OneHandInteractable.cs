using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEditor.Experimental.GraphView.GraphView;

public class OneHandInteractable : XRGrabInteractable
{
    public void ChangeLayerOnDrop(float delay)
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.layer = LayerMask.NameToLayer("WorldCollision");
        }
        StartCoroutine(RestoreLayers(delay));
    }

    private IEnumerator RestoreLayers(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Collider collider in colliders)
        {

            collider.gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }
}
