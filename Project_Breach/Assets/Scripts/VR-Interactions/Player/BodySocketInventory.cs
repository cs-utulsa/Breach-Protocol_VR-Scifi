using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

[System.Serializable] 
public class BodySocket
{
    public GameObject gameObject;
    [Range(0.1f, 1.0f)]
    public float heightRatio;
}


public class BodySocketInventory : MonoBehaviour
{
    public GameObject HMD;
    //public BodySocket[] bodySockets;
    //private Vector3 currentHMDPosition;
    //private Quaternion currentHMDRotation;

    [Header("Adjustable Offset")]
    public Vector3 offset;
    public float setXRotation = 0.0f;
    public float setYRotation = 0.0f;

    void Update()
    {
        transform.position = HMD.transform.position + Vector3.up * offset.y
            + Vector3.ProjectOnPlane(HMD.transform.right, Vector3.up).normalized * offset.x
            + Vector3.ProjectOnPlane(HMD.transform.forward, Vector3.up).normalized * offset.z;

        transform.eulerAngles = new Vector3(setXRotation, HMD.transform.eulerAngles.y + setYRotation, 0);
    }

    /*
    private void Update()
    {
        currentHMDPosition = HMD.transform.transform.position;
        currentHMDRotation = HMD.transform.rotation;
        foreach(var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketInventory();
    }

    private void UpdateBodySocketHeight(BodySocket bs)
    {
        bs.gameObject.transform.position = new Vector3(bs.gameObject.transform.position.x, (HMD.transform.position.y) * bs.heightRatio, bs.gameObject.transform.position.z);
    }

    private void UpdateSocketInventory()
    {
        //transform.position = new Vector3(currentHMDPosition.x, 0, currentHMDPosition.z);
        transform.rotation = new Quaternion(transform.rotation.x, currentHMDRotation.y, transform.rotation.z, currentHMDRotation.w);

        transform.position = HMD.transform.position + Vector3.up * offset.y
        + Vector3.ProjectOnPlane(HMD.transform.right, Vector3.up).normalized * offset.x
    + Vector3.ProjectOnPlane(HMD.transform.forward, Vector3.up).normalized * offset.z;
    }
    */
}
