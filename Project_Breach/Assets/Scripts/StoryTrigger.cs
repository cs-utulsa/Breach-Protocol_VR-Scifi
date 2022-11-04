using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryTrigger : MonoBehaviour
{
    public AudioSource source;
    public AudioClip storyClip;
    public UnityEvent onEnter;
    public float delayInvoke;
    private bool hasPlayed;

    private void Start()
    {
        source = GetComponentInParent<AudioSource>();
        hasPlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasPlayed)
            {
                hasPlayed = true;
                //onEnter.Invoke();
                source.Stop();
                source.PlayOneShot(storyClip);
                StartCoroutine(StartEvent());
            }
        }
    }

    public IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(delayInvoke);
        onEnter.Invoke();
    }
}
