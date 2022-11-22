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

    public void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
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
        photonView.RequestOwnership();

        if (firstInteractorSelecting.transform.gameObject.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachPoint;
        }
        else
        {
            attachTransform = rightAttachPoint;
        }

        base.OnSelectEntered(args);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.useGravity);
        }
        else if (stream.IsReading)
        {
            rb.useGravity = (bool) stream.ReceiveNext();
        }
    }
}
