using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [Header("Detonator Range")]
    public Collider detonatorRange;

    [Header("Animation")]
    public Animator animator = null;
    public string buttonPressParam = "Pressed";
    public Material breachIndicatorMat = null;

    [Header("Debug")]
    [SerializeField] bool readyToDetonate;
    [SerializeField] bool pulledOnce = false;
    [SerializeField] BreachCharge breachCharge = null;

    [Header("Photon")]
    public PhotonView photonView;
    void Awake()
    {
        detonatorRange = GetComponent<Collider>();
        detonatorRange.isTrigger = true;
        readyToDetonate = false;
        breachIndicatorMat.SetColor("_EmissionColor", Color.red);
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (readyToDetonate && breachCharge.GetIsChargeArmed())
        {
            breachIndicatorMat.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            breachIndicatorMat.SetColor("_EmissionColor", Color.red);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Armed Breaching Charge"))
        {
            breachCharge = other.GetComponentInParent<BreachCharge>();
            if (breachCharge.GetChargeInSocketRange() && breachCharge.GetIsChargeArmed())
            {
                breachIndicatorMat.SetColor("_EmissionColor", Color.green);
                readyToDetonate = true;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Armed Breaching Charge"))
        {
            readyToDetonate = false;
            breachIndicatorMat.SetColor("_EmissionColor", Color.red);
        }

    }

    public void DetonatorPulled()
    {
        if (readyToDetonate && breachCharge.GetSurfaceToBreach().IsBreacherAttached() && !pulledOnce)
        {
            readyToDetonate = false;
            pulledOnce = true;
            StartCoroutine(DetonateRoutine());
        }
        animator.SetTrigger(buttonPressParam);
    }

    private IEnumerator DetonateRoutine()
    {
        breachCharge.flashRate = breachCharge.flashRate / 10.0f;
        yield return new WaitForSeconds(2.0f);

        //breachCharge.source.mute = true;
        breachCharge.gameObject.GetComponent<AudioSource>().PlayOneShot(breachCharge.blowUpAudio);
        foreach (MeshRenderer rend in breachCharge.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        breachCharge.GetSurfaceToBreach().gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (MeshRenderer rend in breachCharge.GetSurfaceToBreach().gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        breachCharge.GetSurfaceToBreach().socket.enabled = false;
        foreach (var particle in breachCharge.explosiveParticles)
        {
            particle.Emit(5);
        }


        Destroy(breachCharge.gameObject, 3.0f);
        Destroy(breachCharge.GetSurfaceToBreach().gameObject, 3.0f);
        //Destroy(breachCharge.gameObject, 5.0f);
        breachIndicatorMat.SetColor("_EmissionColor", Color.red);
        breachCharge.flashRate = breachCharge.flashRate * 10.0f;
        yield return new WaitForSeconds(0.25f);
        breachCharge.source.mute = false;
        pulledOnce = false;
        breachCharge = null;
    }


}
