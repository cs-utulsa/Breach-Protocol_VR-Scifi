using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public Renderer ghostHand;
    public float ghostHandDistance = 0.05f;
    public float handTeleportThreshold = 0.0f;
    public InputActionReference moveJoystick;
    public XRBaseInteractor interactor;
    private Collider[] handColliders;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponentInParent<XRBaseInteractor>();
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    public void EnableCollider()
    {
        foreach(var item in handColliders)
        {
            item.enabled = true;
        }
    }

    public void EnableColliderDelay(float delay)
    {
        Invoke("EnableCollider", delay);
    }

    public void DisableCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
        }
    }

    public void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > ghostHandDistance)
        {
            ghostHand.enabled = true;
        }
        else
        {
            ghostHand.enabled = false;
        }

        if (moveJoystick.action.ReadValue<Vector2>().magnitude > handTeleportThreshold)
        {
            isMoving = true;
            transform.position = target.position;
            transform.rotation = target.rotation;
            DisableCollider();
        }
        else
        {
            isMoving = false;
            if (!handColliders[0].enabled && !interactor.isSelectActive)
            {
                EnableCollider();
            }
        }
        
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            // position
            rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

            // rotation
            Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
            rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
            Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
            rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
        }

    }
}
