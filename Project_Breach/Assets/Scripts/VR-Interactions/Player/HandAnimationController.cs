using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimationController : MonoBehaviour
{
    public Animator animator;
    public XRDirectInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        interactor = GetComponent<XRDirectInteractor>();
    }


    private void Update()
    {
        if (interactor.hasSelection && interactor.firstInteractableSelected.transform.tag == "Primary Weapon" || interactor.hasSelection && interactor.firstInteractableSelected.transform.tag == "Secondary Weapon")
        {
            animator.SetBool("Primary Grab", true);
        }
        else
        {
            animator.SetBool("Primary Grab", false);
        }
    }
}
