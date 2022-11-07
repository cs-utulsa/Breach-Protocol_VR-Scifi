using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Base : MonoBehaviour
{
    [Header("AOE_Data")]
    public AOE_Data aoeData;

    [Header("Visuals")]
    public Rigidbody rb = null;
    public Light beepLight = null;
    public MeshRenderer[] meshRenderers;
    public PhotonView photonView;

    [Header("Audio")]
    public AudioSource source = null;

    protected bool activated = false;
    protected float currentTime = 0.0f;

    [SerializeField] protected LayerMask mask;
    [SerializeField] protected Collider[] collidersInRange;


    public virtual void Awake()
    {
        activated = false;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        beepLight = GetComponentInChildren<Light>();
        beepLight.enabled = false;
        photonView = GetComponent<PhotonView>();
        mask = LayerMask.GetMask(aoeData.layerMask);
    }

    [PunRPC]
    public void ActivateThrowable()
    {
        if (!activated)
        {
            activated = true;
            source.PlayOneShot(aoeData.activationAudio);
            beepLight.enabled = true;
            StartCoroutine(DetonateRoutine());
        }
    }

    public IEnumerator DetonateRoutine()
    {
        float timeFromLastFlash = 0.0f;
        float flashRate = 1.0f;
        while (currentTime <= aoeData.detonationTimer)
        {
            currentTime += Time.deltaTime;
            if (timeFromLastFlash >= flashRate)
            {
                Beep();
                //photonView.RPC("Beep", RpcTarget.AllBuffered);
                timeFromLastFlash = 0.0f;
                flashRate /= aoeData.flashRateSpeedUp;
            }
            else
            {
                timeFromLastFlash += Time.deltaTime;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Detonate();
        //photonView.RPC("Detonate", RpcTarget.AllBuffered);
        CheckForEffected();
        //photonView.RPC("CheckForEffected", RpcTarget.AllBuffered);
        Destroy(gameObject, 3.0f);

    }

    protected void Detonate()
    {
        beepLight.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

        source.PlayOneShot(aoeData.detonationAudio);

        foreach (var particle in aoeData.particles)
        {
            ParticleSystem thisParticle = Instantiate(particle, this.transform.position, Quaternion.identity, this.transform);
            thisParticle.Emit(5);
        }
        Destroy(this.gameObject, 3.0f);
    }


    protected void Beep()
    {
        beepLight.enabled = !beepLight.enabled;
        source.PlayOneShot(aoeData.beepSound);
    }

    [PunRPC]
    protected virtual void CheckForEffected()
    {
        collidersInRange = Physics.OverlapSphere(transform.position, aoeData.range, mask);
        foreach (Collider c in collidersInRange)
        {
            Debug.Log(c + " is in the area of effect.");
        }
    }

    public void RPC_ActivateThrowable()
    {
        photonView.RPC("ActivateThrowable", RpcTarget.All);
    }

    public virtual void RPC_CheckForEffected()
    {
        photonView.RPC("CheckForEffected", RpcTarget.All);
    }
}
