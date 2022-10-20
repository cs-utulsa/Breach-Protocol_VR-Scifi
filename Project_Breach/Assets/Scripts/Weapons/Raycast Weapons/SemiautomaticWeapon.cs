using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SemiautomaticWeapon : RaycastWeapon
{
    [Header("Interactable")]
    public XRBaseInteractable interactable;

    protected override void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        base.Awake();
    }

    public virtual void LateUpdate()
    {
        if (interactable.isSelected)
        {
            if (interactable.firstInteractorSelecting.transform.gameObject.TryGetComponent<ExtraActionController>(out ExtraActionController actionController))
            {
                float primaryButtonValue = actionController.primaryButtonAction.action.ReadValue<float>();
                if (primaryButtonValue >= 1.0f && !isCharging)
                {
                    Recharge();
                }
            }
        }

    }
}
