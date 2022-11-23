using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AutomaticDoor : MonoBehaviour
{
    public Animator animator;
    public AudioSource source;
    public string openParam = "Open";
    public AudioClip openClip;
    public AudioClip closeClip;
    public PhotonView photonView;


    [SerializeField] private bool isOpen;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        source = GetComponentInChildren<AudioSource>();
        photonView = GetComponent<PhotonView>();
        isOpen = false;
        animator.SetBool(openParam, isOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Target")) && !isOpen)
        {
            RPC_OpenDoor();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Target")) && isOpen)
        {
            RPC_CloseDoor();
        }
    }

    [PunRPC]
    public void RPC_OpenDoor()
    {
        photonView.RPC("OpenDoor", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_CloseDoor()
    {
        photonView.RPC("CloseDoor", RpcTarget.All);
    }
    [PunRPC]
    public void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(openParam, isOpen);
        source.Stop();
        source.PlayOneShot(openClip);
    }
    [PunRPC]
    public void CloseDoor()
    {
        isOpen = false;
        animator.SetBool(openParam, isOpen);
        source.Stop();
        source.PlayOneShot(closeClip);
    }
}
