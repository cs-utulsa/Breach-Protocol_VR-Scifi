using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BreachCharge : MonoBehaviour
{
    [Header("Animation and Audio")]
    public AudioSource source;
    public Light beepLight = null;
    public AudioClip beepAudio = null;
    public AudioClip blowUpAudio = null;
    public float flashRate = 1.0f;
    public string SurfaceTag = "Breachable Surface";
    public ParticleSystem[] explosiveParticles;
    

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

}
