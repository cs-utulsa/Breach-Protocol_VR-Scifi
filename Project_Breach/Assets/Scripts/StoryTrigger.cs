using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryTrigger : MonoBehaviour
{
    public AudioSource source;
    public PhotonView photonView;
    public AudioClip storyClip;
    public UnityEvent onEnter;
    public float delayInvoke;
    private bool hasPlayed;

    private void Start()
    {
        source = GetComponentInParent<AudioSource>();
        photonView = GetComponent<PhotonView>();
        hasPlayed = false;
    }

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasPlayed)
            {
                photonView.RPC("PlayStoryTrigger", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(delayInvoke);
        onEnter.Invoke();
        Destroy(this.gameObject,3.0f);
    }

    [PunRPC]
    public void PlayStoryTrigger()
    {
        hasPlayed = true;
        source.Stop();
        source.PlayOneShot(storyClip);
        StartCoroutine("StartEvent");
    }
}
