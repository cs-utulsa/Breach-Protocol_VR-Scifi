using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerWeapon : RaycastWeapon
{
    [Header("Interactable")]
    public XRBaseInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    private void LateUpdate()
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
