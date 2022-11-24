using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Shield : MonoBehaviour, IPunObservable
{
    [Header("Player Data")]
    public PlayerData playerData;
    public Health playerHealth;
    public PhotonView photonView;

    [Header("Audio")]
    public AudioSource source;

    [Header("Runtime Variables")]
    [SerializeField] private float currentShield;
    [SerializeField] private float timeFromLastHit;
    [SerializeField] private bool rechargePlayed;


    // Start is called before the first frame update
    void Start()
    {
        currentShield = playerData.maxShield;
        source = GetComponent<AudioSource>();
        playerHealth = GetComponent<Health>();
        timeFromLastHit = playerData.maxRegenTimer;
    }

    void FixedUpdate()
    {
        if (CanRegenerate())
        {
            RegenerateShield();
        }
    }

    private bool CanRegenerate() {
        if (timeFromLastHit < playerData.maxRegenTimer)
        {
            timeFromLastHit += Time.fixedDeltaTime;
            return false;
        }
        else
        {
            if (currentShield < playerData.maxShield)
            {
                return true;
            }
            else
            {
                rechargePlayed = false;
                return false;
            }
        }
    }

    private void RegenerateShield()
    {
        if (!rechargePlayed)
        {
            rechargePlayed = true;
            source.PlayOneShot(playerData.shieldRecharge);
        }

        if (currentShield < playerData.maxShield)
        {
            currentShield += playerData.regenerationRate;
        }
        else
        {
            currentShield = playerData.maxShield;
        }
        
    }


    public void TakeDamage(float value)
    {
        if (currentShield > 0)
        {
            source.PlayOneShot(playerData.shieldHit);
            if (value >= currentShield)
            {
                currentShield = 0.0f;
            }
            else
            {
                currentShield -= value;
            }
        }
        else
        {
            playerHealth.TakeDamage(value);
        }
        rechargePlayed = false;
        timeFromLastHit = 0.0f;
    }

    public float getShieldCharge()
    {
        return currentShield;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentShield);
        } else if (stream.IsReading)
        {
            if (photonView.IsMine)
            {
                currentShield = (float)stream.ReceiveNext();
            }
        }
    }
}
