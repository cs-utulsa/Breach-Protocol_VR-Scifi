using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerUI : MonoBehaviour, IPunObservable
{
    [Header("Tracked Data")]
    public Health playerHealth;
    public Shield playerShield;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider shieldSlider;

    [Header("Photon")]
    public PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(healthSlider.value);
            stream.SendNext(shieldSlider.value);
        } else if (stream.IsReading)
        {
           healthSlider.value = (float)stream.ReceiveNext();
           shieldSlider.value = (float)stream.ReceiveNext();
        }
        
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();
        playerHealth = rig.GetComponent<Health>();
        playerShield = rig.GetComponent<Shield>();
        healthSlider.maxValue = playerHealth.playerData.maxHealth;
        shieldSlider.maxValue = playerShield.playerData.maxShield;
        healthSlider.value = playerHealth.playerData.maxHealth;
        shieldSlider.value = playerShield.playerData.maxShield;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            healthSlider.value = playerHealth.getCurrentHealth();
            shieldSlider.value = playerShield.getShieldCharge();
        }
    }
}
