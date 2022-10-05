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
    private Quaternion initialRotationoffset;
    private Transform activeSecondGrab;
    [SerializeField] bool inHolster = false;

    [Header("Object Type")]
    public bool oneHanded = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20.0f;
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
            item.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // compute rotation

        if (!oneHanded)
        {
            if (firstInteractor != null && secondInteractor != null)
            {
                if (snapToSecondHand)
                {
                    firstInteractor.transform.rotation = GetTwoHandRotation();
                }
                else
                {
                    firstInteractor.transform.rotation = GetTwoHandRotation() * initialRotationoffset;
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
        }


        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        try
        {
            if (twoHandRotationType == TwoHandRotationType.None)
            {
                targetRotation = Quaternion.LookRotation(secondInteractor.transform.position - firstInteractor.transform.position);
            }
            else if (twoHandRotationType == TwoHandRotationType.First)
            {

                targetRotation = Quaternion.LookRotation(secondInteractor.transform.position - firstInteractor.transform.position, firstInteractor.transform.up);

            }
            else
            {
                targetRotation = Quaternion.LookRotation(secondInteractor.transform.position - firstInteractor.transform.position, secondInteractor.transform.up);
            }
        }
        catch
        {
            return Quaternion.identity;
        }

        return targetRotation;
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        SetParentToXRRig();
        firstInteractor = args.interactorObject;
        attachInitialRotation = args.interactorObject.transform.localRotation;

        foreach (var item in secondHandGrabPoints)
        {
            item.enabled = true;
        }

        SetParentToXRRig();
        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        firstInteractor = null;
        secondInteractor = null;

        foreach (var item in secondHandGrabPoints)
        {
            item.enabled = false;
        }

        args.interactorObject.transform.localRotation = attachInitialRotation;
        base.OnSelectExited(args);

    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        secondInteractor = args.interactorObject;
        initialRotationoffset = Quaternion.Inverse(GetTwoHandRotation()) * firstInteractor.transform.rotation;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        secondInteractor = null;
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = isSelected && !interactor.Equals(firstInteractorSelecting);
        //bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor) && !inHolster;
        return base.IsSelectableBy(interactor);
    }

    public void SetParentToXRRig()
    {
        if (!inHolster)
            transform.SetParent(firstInteractorSelecting.transform);
    }

    public void SetParentToWorld()
    {
        transform.SetParent(null);
    }


}
