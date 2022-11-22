using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDoor : MonoBehaviour
{
    public Animator animator;
    public string openParam = "Open";

    [SerializeField]private bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
        animator.SetBool(openParam, isOpen);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool(openParam, isOpen);
    }
}
