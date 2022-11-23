
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using Random = UnityEngine.Random;

public class Keypad : MonoBehaviour, IPunObservable
{
    public KeypadButton[] keypadButtons;
    public TextMeshProUGUI masterKeyText;
    public TextMeshProUGUI enterKeyText;
    public PhotonView photonView;
    public int numCharacters;

    [SerializeField] private int keyCode;
    [SerializeField] private string enteredString;
    [SerializeField] private bool activated;

    private void Start()
    {
        keypadButtons = GetComponentsInChildren<KeypadButton>();
        photonView = GetComponent<PhotonView>();
        keyCode = -1;
        enteredString = null;
        activated = false;
    }

    public void GenerateNewCode()
    {
        string generatedKeyCode = "";
        for (int i = 0; i < numCharacters; i++)
        {
            generatedKeyCode += Random.Range(1, 9).ToString();
        }
        keyCode = int.Parse(generatedKeyCode);
        ResetKeypad();
        masterKeyText.text = keyCode.ToString();
    }

    public void EnterCharacter(string character)
    {
        if (!activated)
        {
            enteredString += character;
            enterKeyText.text = enteredString;
            if (enteredString.Length <= numCharacters)
            {
                CheckForActivation();
            }
            else
            {
                GenerateNewCode();
            }
        }


    }

    public void CheckForActivation()
    {
        if (Convert.ToInt32(enteredString).Equals(keyCode))
        {
            activated = true;
        }
    }

    public void ResetKeypad()
    {
        enteredString = null;
        activated = false;
        enterKeyText.text = "";
        masterKeyText.text = "";
    }

    public bool GetIsActivated()
    {
        return activated;
    }

    public string GetEnteredCode()
    {
        return enteredString;
    }

    public void SetEnteredCode(string entered)
    {
        enteredString = entered;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(masterKeyText.text);
            stream.SendNext(enterKeyText.text);
            stream.SendNext(keyCode);
            stream.SendNext(enteredString);
            stream.SendNext(activated);
        } else if (stream.IsReading)
        {
            masterKeyText.text = (string) stream.ReceiveNext();
            enterKeyText.text = (string) stream.ReceiveNext();
            keyCode = (int)stream.ReceiveNext();
            enteredString = (string)stream.ReceiveNext();
            activated = (bool)stream.ReceiveNext();
        }
    }
}
