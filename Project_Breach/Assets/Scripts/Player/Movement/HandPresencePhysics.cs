using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public Renderer ghostHand;
    public float ghostHandDistance = 0.05f;
    private Collider[] handColliders;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void FixedUpdate()
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
