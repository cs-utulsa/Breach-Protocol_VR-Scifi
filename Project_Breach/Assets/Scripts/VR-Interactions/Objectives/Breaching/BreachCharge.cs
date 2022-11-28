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
    public AudioClip holsteredAudio = null;
    public float flashRate = 5.0f;
    public string SurfaceTag = "Breachable Surface";
    public string ArmedTag = "Armed Breaching Charge";
    public string DisarmedTag = "Breaching Charge";
    public ParticleSystem[] explosiveParticles;

    private bool isDetonated = false;

    [Header("Keypad")]
    public Keypad keypad;

    [Header("Photon")]
    public PhotonView photonView;
    

    [Header("Debug")]
    [SerializeField] private bool chargeInSocketRange = false;
    [SerializeField] private bool chargeArmed = false;
    [SerializeField] private bool isBeeping = false;
    [SerializeField] private BreachableSurface breachableSurface = null;
    [SerializeField] private float timeFromLastFlash = 0.0f;
    public OneHandInteractable interactable;
    public float explosiveRange;


    void Awake()
    {
        keypad = GetComponent<Keypad>();
        source = GetComponent<AudioSource>();
        interactable = GetComponent<OneHandInteractable>();
        photonView = GetComponent<PhotonView>();
        chargeInSocketRange = false;
        chargeArmed = false;
        isBeeping = false;
        isDetonated = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(SurfaceTag))
        {
            breachableSurface = other.GetComponentInParent<BreachableSurface>();
            chargeInSocketRange = true;
            gameObject.GetComponentInChildren<Collider>().tag = DisarmedTag;
            timeFromLastFlash = 0.0f;
            beepLight.color = Color.red;

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (interactable.isSelected && interactable.firstInteractorSelecting.transform.gameObject.CompareTag(SurfaceTag) && !isBeeping)
        {
            isBeeping = true;
            StartCoroutine(BeepRoutine());
        }

        if (other.gameObject.CompareTag(SurfaceTag))
        {
            chargeInSocketRange = true;
            chargeArmed = keypad.GetIsActivated();
            if (chargeArmed)
            {
                gameObject.GetComponentInChildren<Collider>().tag = ArmedTag;
                beepLight.color = Color.green;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(SurfaceTag))
        {
            chargeInSocketRange = false;
            chargeArmed = false;
            timeFromLastFlash = 0.0f;
            gameObject.GetComponentInChildren<Collider>().tag = DisarmedTag;
            beepLight.enabled = false;
            beepLight.color = Color.red;
            isBeeping = false;
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
        } else if (interactable.firstInteractorSelecting.transform.CompareTag("Inventory"))
        {
            source.PlayOneShot(holsteredAudio);
        }
    }

    public void ChangeArmingStatus()
    {
        if (interactable.firstInteractorSelecting.transform.CompareTag("Breachable Surface"))
        {
            keypad.GenerateNewCode();
        }
        else
        {
            keypad.ResetKeypad();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(keypad.GetEnteredCode());
        } else if (stream.IsReading)
        {
            keypad.SetEnteredCode((string)stream.ReceiveNext());
            keypad.CheckForActivation();
            if (keypad.GetIsActivated())
            {
                chargeArmed = true;
            }
            else
            {
                chargeArmed = false;
            }
        }
    }

    public bool GetIsDetonated()
    {
        return isDetonated;
    }

    public void SetIsDetonated(bool detonated)
    {
        isDetonated = detonated;
    }
}
