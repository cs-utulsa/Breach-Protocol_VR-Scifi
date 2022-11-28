using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HealthStation : MonoBehaviour
{
    public XRGrabInteractable[] interactables;
    public float healRate = 10.0f;
    public float timeBetweenHeals = 1.0f;
    public AudioSource source;
    public AudioClip grabAudio;
    public AudioClip healAudio;

    [SerializeField] private int pointsGrabbed;
    [SerializeField] private float timer;

    private void Start()
    {
        interactables = GetComponentsInChildren<XRGrabInteractable>();
        source = GetComponent<AudioSource>();
        pointsGrabbed = 0;
        timer = 1.0f;
        enabled = false;
    }

    private void FixedUpdate()
    {
        if (pointsGrabbed >= 2)
        {
            if (interactables[0].isSelected && interactables[1].isSelected)
            {
                //Debug.Log(interactables[0].firstInteractorSelecting.transform.root);
                //Debug.Log(interactables[1].firstInteractorSelecting.transform.root);
                if (interactables[0].firstInteractorSelecting.transform.root.Equals(interactables[1].firstInteractorSelecting.transform.root) && timer <= 0.0f)
                {
                    Health playerHealth = interactables[0].firstInteractorSelecting.transform.root.GetComponent<Health>();
                    if (playerHealth.getCurrentHealth() != playerHealth.playerData.maxHealth)
                    {
                        playerHealth.Heal(healRate);
                        Debug.Log("Heal Player");
                        source.PlayOneShot(healAudio);
                    }
                    timer = timeBetweenHeals;
                }
                else
                {
                    timer -= Time.fixedDeltaTime;
                }
            }
        }
    }

    public void PointGrabbed()
    {
        pointsGrabbed++;
        enabled = true;
        Debug.Log("Point Grabbed");
        source.PlayOneShot(grabAudio);
    }

    public void PointLetGo()
    {
        pointsGrabbed--;
        if (pointsGrabbed <= 0)
        {
            pointsGrabbed = 0;
            enabled = false;
        }
        timer = 1.0f;
        Debug.Log("Point Let Go");
    }

}
