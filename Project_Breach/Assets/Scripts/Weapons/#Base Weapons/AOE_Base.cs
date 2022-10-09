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
    public MeshRenderer meshRenderer = null;

    [Header("Audio")]
    public AudioSource source = null;

    protected bool activated = false;
    protected float currentTime = 0.0f;

    [SerializeField] protected LayerMask mask;


    private void Awake()
    {
        activated = false;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        beepLight = GetComponentInChildren<Light>();
        beepLight.enabled = false;
        mask = LayerMask.GetMask(aoeData.layerMask);
    }

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
        CheckForEffected();
        Destroy(gameObject, 3.0f);
    }

    private void Detonate()
    {
        beepLight.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        meshRenderer.enabled = false;

        source.PlayOneShot(aoeData.detonationAudio);

        foreach (var particle in aoeData.particles)
        {
            particle.Emit(5);
        }
    }


    private void Beep()
    {
        beepLight.enabled = !beepLight.enabled;
        source.PlayOneShot(aoeData.beepSound);
    }


    protected virtual void CheckForEffected()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeData.range, mask);  // Memory leak?
        //List<AIHealth> aiHealths = new List<AIHealth>();
        foreach (Collider c in colliders)
        {
           /*
            if ((c.CompareTag("Grunt") || c.CompareTag("Turret")) && !aiHealths.Contains(c.GetComponentInParent<AIHealth>()))
            {
                aiHealths.Add(c.GetComponentInParent<AIHealth>());
            }
           */
        }
    }

}
