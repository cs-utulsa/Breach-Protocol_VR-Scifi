using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimationController : MonoBehaviour
{
    public Animator[] animators;
    public Animator activeAnimator;
    public XRDirectInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        interactor = GetComponent<XRDirectInteractor>();

        foreach (Animator anim in animators)
        {
            if (anim.isActiveAndEnabled)
            {
                activeAnimator = anim;
                break;
            }
        }
    }


    private void Update()
    {
        if (!activeAnimator.isActiveAndEnabled)
        {
            foreach (Animator anim in animators)
            {
                if (anim.isActiveAndEnabled)
                {
                    activeAnimator = anim;
                    break;
                }
            }
        }
        else
        {
            if (interactor.hasSelection && interactor.firstInteractableSelected.transform.tag == "Primary Weapon" || interactor.hasSelection && interactor.firstInteractableSelected.transform.tag == "Secondary Weapon")
            {
                activeAnimator.SetBool("Primary Grab", true);
            }
            else
            {
                activeAnimator.SetBool("Primary Grab", false);
            }
        }
    }
}
