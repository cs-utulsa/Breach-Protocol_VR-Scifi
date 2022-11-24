using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [Range(1,9)]
    public int buttonValue = 0;

    private Keypad keypad;

    private void Start()
    {
        keypad = GetComponentInParent<Keypad>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            if (!keypad.photonView.IsMine)
            {
                keypad.photonView.RequestOwnership();
            }
            keypad.EnterCharacter(buttonValue.ToString());
        }
    }
}
