using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IPunObservable
{
    [Header("Tracked Data")]
    public Health playerHealth;
    public Shield playerShield;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider shieldSlider;
    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondaryText;

    [Header("Inventory Sockets")]
    public SocketTagCheck primarySocket;
    public SocketTagCheck secondarySocket;

    [Header("Photon")]
    public PhotonView photonView;

    private bool primaryFull;
    private bool secondaryFull;
    private void Start()
    {
        playerHealth = GetComponentInParent<Health>();
        playerShield = GetComponentInParent<Shield>();
        photonView = GetComponentInParent<PhotonView>();
        healthSlider.maxValue = playerHealth.playerData.maxHealth;
        shieldSlider.maxValue = playerShield.playerData.maxShield;
        primaryFull = false;
        secondaryFull = false;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            shieldSlider.value = playerShield.getShieldCharge();
            healthSlider.value = playerHealth.getCurrentHealth();
        }
    }

    public void TogglePrimaryUI()
    {
        primaryFull = !primaryFull;
        if (primaryFull)
        {
            primaryText.text = "You have a primary.";
        }
        else
        {
            primaryText.text = "No primary.";
        }
    }

    public void ToggleSecondaryUI()
    {
        secondaryFull = !secondaryFull;
        if (secondaryFull)
        {
            secondaryText.text = "You have a secondary.";
        }
        else
        {
            secondaryText.text = "No secondary.";
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(primaryFull);
            stream.SendNext(secondaryFull);
            stream.SendNext(playerHealth.getCurrentHealth());
            stream.SendNext(playerShield.getShieldCharge());
        } else if (stream.IsReading)
        {
            primaryFull = (bool) stream.ReceiveNext();
            secondaryFull = (bool)stream.ReceiveNext();
            healthSlider.value = (float)stream.ReceiveNext();
            shieldSlider.value = (float)stream.ReceiveNext();
        }
    }
}
