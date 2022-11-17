using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;

    public float speedThreshold = 0.1f;

    private void Start()
    {
     
       animator = GetComponent<Animator>();
       vrRig = GetComponentInParent<VRRig>();
       previousPos = vrRig.head.vrTarget.position;
        
    }

    private void Update()
    {
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedThreshold);
        animator.SetFloat("Speed", headsetLocalSpeed.magnitude);
    }
}
