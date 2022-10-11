using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtraActionController : ActionBasedController
{
    [SerializeField]
    InputActionProperty m_PrimaryButtonAction;

    public InputActionProperty primaryButtonAction
    {
        get => m_PrimaryButtonAction;
        set => SetInputActionProperty(ref m_PrimaryButtonAction, value);
    }



    [SerializeField]
    InputActionProperty m_SecondaryButtonAction;

    public InputActionProperty SecondaryButtonAction
    {
        get => m_SecondaryButtonAction;
        set => SetInputActionProperty(ref m_SecondaryButtonAction, value);
    }
}
