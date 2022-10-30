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
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

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
        collidersInRange = Physics.OverlapSphere(transform.position, aoeData.range, mask);
        foreach (Collider c in collidersInRange)
        {
            Debug.Log(c + " is in the area of effect.");
        }
    }

}
