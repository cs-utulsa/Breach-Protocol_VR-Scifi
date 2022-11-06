using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class BreachCharge : MonoBehaviour, IPunObservable
{
    [Header("Animation and Audio")]
    public AudioSource source;
    public Light beepLight = null;
    public AudioClip placedAudio = null;
    public AudioClip beepAudio = null;
    public AudioClip blowUpAudio = null;
    public float flashRate = 1.0f;
    public string SurfaceTag = "Breachable Surface";
    public ParticleSystem[] explosiveParticles;

    [Header("Tags")]
    public string tagArmed = "Armed Breaching Charge";
    public string tagDisarmed = "Breaching Charge";

    [Header("Photon")]
    public PhotonView photonView;
    

    [Header("Debug")]
    [SerializeField] private bool chargeInSocketRange = false;
    [SerializeField] private bool chargeArmed = false;
    [SerializeField] private BreachableSurface breachableSurface = null;
    [SerializeField] private float timeFromLastFlash = 0.0f;
    public OneHandInteractable interactable;
    public float explosiveRange;


    void Awake()
    {
        source = GetComponent<AudioSource>();
        interactable = GetComponent<OneHandInteractable>();
        photonView = GetComponent<PhotonView>();
        chargeInSocketRange = false;
        chargeArmed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(SurfaceTag))
        {
            breachableSurface = other.GetComponentInParent<BreachableSurface>();
            chargeInSocketRange = true;
            chargeArmed = breachableSurface.IsBreacherAttached();
            timeFromLastFlash = 0.0f;
            StartCoroutine(BeepRoutine());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(SurfaceTag))
        {
            chargeInSocketRange = false;
            chargeArmed = false;
            timeFromLastFlash = 0.0f;
            beepLight.enabled = false;
            StopAllCoroutines();
        }
    }

    public bool GetChargeInSocketRange()
    {
        return chargeInSocketRange;
    }

    public BreachableSurface GetSurfaceToBreach()
    {
        return breachableSurface;
    }

    public bool GetIsChargeArmed()
    {
        return chargeArmed;
    }

    private void Beep()
    {
        beepLight.enabled = !beepLight.enabled;
        source.PlayOneShot(beepAudio);
    }

    private IEnumerator BeepRoutine()
    {
        while (breachableSurface.IsBreacherAttached())
        {
            if (timeFromLastFlash >= flashRate)
            {
                Beep();
                timeFromLastFlash = 0.0f;
            }
            else
            {
                if (!breachableSurface.IsBreacherAttached())
                {
                    break;
                }
                else
                {
                    timeFromLastFlash += Time.fixedDeltaTime;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
            }
        }
    }

    public void PlayPlacedSound()
    {
        if (interactable.firstInteractorSelecting.transform.CompareTag("Breachable Surface")) 
        {
            source.PlayOneShot(placedAudio);
        }
    }

    public void ChangeArmingStatus()
    {
        if (interactable.firstInteractorSelecting.transform.CompareTag("Breachable Surface"))
        {
            gameObject.tag = tagArmed;
            gameObject.GetComponentInChildren<Collider>().tag = tagArmed;
        }
        else
        {
            gameObject.tag = tagDisarmed;
            gameObject.GetComponentInChildren<Collider>().tag = tagDisarmed;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.tag);
        } else if (stream.IsReading)
        {
            gameObject.tag = (string) stream.ReceiveNext();
        }
    }
}
