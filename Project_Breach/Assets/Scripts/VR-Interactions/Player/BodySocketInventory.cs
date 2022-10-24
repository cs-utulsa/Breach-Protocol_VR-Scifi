using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
    public BodySocket[] bodySockets;
    private Vector3 currentHMDPosition;
    private Quaternion currentHMDRotation;
    

    private void Update()
    {
        currentHMDPosition = HMD.transform.position;
        currentHMDRotation = HMD.transform.rotation;
        foreach(var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketInventory();
    }

    private void UpdateBodySocketHeight(BodySocket bs)
    {
        bs.gameObject.transform.position = new Vector3(bs.gameObject.transform.position.x, (transform.parent.transform.position.y + currentHMDPosition.y) * bs.heightRatio, bs.gameObject.transform.position.z);
    }

    private void UpdateSocketInventory()
    {
        transform.position = new Vector3(currentHMDPosition.x, 0, currentHMDPosition.z);
        transform.rotation = new Quaternion(transform.rotation.x, currentHMDRotation.y, transform.rotation.z, currentHMDRotation.w);
    }
}
