using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever : MonoBehaviour
{
    [Header("Lever Attributes")]
    public HingeJoint hinge;
    public float bounds;
    public float damper;
    [Range(0.0f, 1.0f)] public float threshhold; 
    public UnityEvent onLeverPull;
    public XRGrabInteractable handleInteractable;
    public Rigidbody rb;

    [SerializeField] private bool isPulled;
    [SerializeField] private bool isInitalized;


    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        handleInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        JointLimits limits = hinge.limits;
        JointSpring spring = hinge.spring;
        bounds = Mathf.Abs(bounds);
        limits.min = -bounds;
        limits.max = bounds;
        spring.damper = damper;
        hinge.limits = limits;
        hinge.spring = spring;
        transform.rotation = new Quaternion(0.0f, transform.rotation.y, transform.rotation.z, 0.0f);
        isPulled = false;
        isInitalized = false;
    }

     void Update()
    {
        if (!isInitalized)
        {
            isInitalized = true;
            this.enabled = false;
        }

        if(transform.localRotation.x >= threshhold)
        {
            isPulled = true;
            onLeverPull.Invoke();
            handleInteractable.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            this.enabled = false;
        }
    }

    public bool GetIsPulled()
    {
        return isPulled;
    }
}
