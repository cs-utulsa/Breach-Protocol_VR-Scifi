using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OneHandInteractable : XRGrabInteractable
{
    private PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //photonView.RequestOwnership();
        base.OnSelectEntered(args);
    }
}
