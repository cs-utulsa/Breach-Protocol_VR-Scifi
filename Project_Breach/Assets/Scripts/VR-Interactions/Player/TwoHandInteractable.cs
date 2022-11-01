
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;
    public float breakDistance = 0.1f;
    public Rigidbody rb = null;

    private IXRSelectInteractor firstInteractor, secondInteractor;
    private Quaternion attachInitialRotation;
    private Quaternion initialRotationOffset;
    private bool inInventory = false;
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        rb.maxAngularVelocity = 20.0f;
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }
        
    }


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && firstInteractorSelecting != null && !firstInteractorSelecting.transform.gameObject.CompareTag("Inventory"))
        {
            if (snapToSecondHand)
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation();
            }

            else
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }

            foreach (var grab in secondHandGrabPoints)
            {
                if (grab.isSelected)
                {
                    if (Mathf.Abs(Vector3.Distance(secondInteractor.transform.position, grab.transform.position)) > breakDistance)
                    {
                        grab.enabled = false;
                        grab.enabled = true;
                    }
                }
            }

        }
        base.ProcessInteractable(updatePhase);
    }


    private Quaternion GetTwoHandRotation()
    {
        Transform attachTransform1;
        if (firstInteractorSelecting.transform == null)
        {
            attachTransform1 = secondInteractor.transform;
        }
        else
        {
             attachTransform1= firstInteractorSelecting.transform;
        }
        Transform attachTransform2 = secondInteractor.transform;

        switch (twoHandRotationType)
        {
            case TwoHandRotationType.None:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position);

            case TwoHandRotationType.First:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, firstInteractorSelecting.transform.up);

            case TwoHandRotationType.Second:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);

            default:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);
        }

    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        secondInteractor = args.interactorObject;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * secondInteractor.transform.rotation;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        StopAllCoroutines();
        photonView.RequestOwnership();
        attachInitialRotation = args.interactorObject.transform.localRotation;
        if (firstInteractorSelecting.transform.gameObject.CompareTag("Inventory"))
        {
            inInventory = true;
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        secondInteractor = null;
        args.interactorObject.transform.localRotation = attachInitialRotation;
        base.OnSelectExited(args);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isalreadygrabbed = firstInteractorSelecting != null && !firstInteractorSelecting.transform.gameObject.CompareTag("Inventory") &&
            !interactor.Equals(firstInteractorSelecting) && !inInventory;

        return (base.IsSelectableBy(interactor) && !isalreadygrabbed);
        
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

}
