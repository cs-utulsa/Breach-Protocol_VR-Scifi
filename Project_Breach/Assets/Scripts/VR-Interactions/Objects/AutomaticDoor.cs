using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public Animator animator;
    public string openParam = "Open";

    [SerializeField] private bool isOpen;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        isOpen = false;
        animator.SetBool(openParam, isOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool(openParam, isOpen);
    }
}
