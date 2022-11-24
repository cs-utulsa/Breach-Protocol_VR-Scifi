using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    public Transform Head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator rightHandAnimator;
    public Animator leftHandAnimator;

    private PhotonView photonView;

    private Transform headRig;
    private Transform rightHandRig;
    private Transform leftHandRig;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        GameObject leftHandObject;
        GameObject rightHandObject;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/LeadHandL").gameObject;
            rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/LeadHandR").gameObject;
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/DemoHandL").gameObject;
            rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/DemoHandR").gameObject;

        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/ScoutHandL").gameObject;
            rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/ScoutHandR").gameObject;

        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/TechHandL").gameObject;
            rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/TechHandR").gameObject;

        }
        else
        {
            leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/LeadHandL").gameObject;
            rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/LeadHandR").gameObject;
        }
        leftHandObject.SetActive(true);
        rightHandObject.SetActive(true);
        leftHandObject.GetComponentInChildren<Animator>().enabled = true;
        rightHandObject.GetComponentInChildren<Animator>().enabled = true;
        leftHandObject.GetComponentInChildren<HandPresence>().enabled = true;
        rightHandObject.GetComponentInChildren<HandPresence>().enabled = true;

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(Head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }

    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator anim)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            anim.SetFloat("Trigger", triggerValue);
        }
        else
        {
            anim.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            anim.SetFloat("Grip", gripValue);
        }
        else
        {
            anim.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {

        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}