using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OneHandInteractable : XRGrabInteractable, IPunObservable
{
    private PhotonView photonView;
    public Rigidbody rb;

    public Transform leftAttachPoint;
    public Transform rightAttachPoint;

    private bool grabbedOverNetwork;
    private bool grabbedByMe;

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        grabbedOverNetwork = false;
        grabbedByMe = false;
    }
    /*
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
    */

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        grabbedOverNetwork = true;
        grabbedByMe = true;
        photonView.RequestOwnership();

        if (firstInteractorSelecting.transform.gameObject.CompareTag("Inventory"))
        {
            ChangeToWorldCollisionLayer();
        }
        else if (firstInteractorSelecting.transform.gameObject.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachPoint;
            RestoreInteractableLayer();
        }
        else
        {
            attachTransform = rightAttachPoint;
            RestoreInteractableLayer();
        }

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        grabbedOverNetwork = false;
        grabbedByMe = false;
        base.OnSelectExited(args);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (grabbedOverNetwork && grabbedByMe)
        {
            return true;
        }

        if (grabbedOverNetwork && !photonView.IsMine)
        {
            return false;
        } 



        return (base.IsSelectableBy(interactor));

    }

    private void ChangeToWorldCollisionLayer()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.layer = LayerMask.NameToLayer("WorldCollision");
        }
    }

    private void RestoreInteractableLayer()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.useGravity);
            stream.SendNext(grabbedOverNetwork);
        }
        else if (stream.IsReading)
        {
            rb.useGravity = (bool) stream.ReceiveNext();
            grabbedOverNetwork = (bool)stream.ReceiveNext();
        }
    }
}
