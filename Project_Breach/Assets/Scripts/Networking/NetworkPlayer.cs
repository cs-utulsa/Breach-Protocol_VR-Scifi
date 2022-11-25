using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using System.Linq;
using UnityEngine.ProBuilder;

public class NetworkPlayer : MonoBehaviour
{
    public Transform Head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator rightHandAnimator;
    public Animator leftHandAnimator;

    public Material leadMaterial;
    public Material demoMaterial;
    public Material scoutMaterial;
    public Material techMaterial;
    public Material defaultMaterial;

    private PhotonView photonView;

    private Transform headRig;
    private Transform rightHandRig;
    private Transform leftHandRig;
    private bool hasDefaultMaterial;
    private int defaultMatIndex;
    // Start is called before the first frame update
    void Start()
    {
        defaultMatIndex = -1;
        hasDefaultMaterial = false;
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        GameObject leftHandObject = rig.transform.Find("Camera Offset/LeftHand Controller/PlayerHandLeft/LeftWrist").gameObject;
        GameObject rightHandObject = rig.transform.Find("Camera Offset/RightHand Controller/PlayerHandRight/RightWrist").gameObject;
        SkinnedMeshRenderer leftSkinMesh = leftHandObject.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer rightSkinMesh = rightHandObject.GetComponent<SkinnedMeshRenderer>();
        for (int i = 0; i < leftSkinMesh.materials.Length; i++)
        {
            Debug.Log(leftSkinMesh.materials[i].name);
            if (leftSkinMesh.materials[i].name ==  defaultMaterial.name + " (Instance)")
            {
                hasDefaultMaterial = true;
                defaultMatIndex = i;
                Debug.Log("Default Material Found");
                break;
            }
        }
        Material[] materials = leftSkinMesh.materials;
        if (hasDefaultMaterial)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                materials[1] = leadMaterial;
                Debug.Log("Assigned Lead");
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                materials[1] = demoMaterial;
                Debug.Log("Assigned Demo");
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                materials[1] = scoutMaterial;
                Debug.Log("Assigned Scout");
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
            {
                materials[1] = techMaterial;
                Debug.Log("Assigned Tech");
            }
            else
            {
                materials[defaultMatIndex] = defaultMaterial;
                Debug.Log("Assigned Default");
            }

            leftSkinMesh.materials = materials;
            rightSkinMesh.materials = materials;
        }




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