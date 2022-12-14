using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}


public class WeaponIK : MonoBehaviour, IPunObservable
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Vector3 targetOffset;
    public Vector3 targetPosition;
    public Vector3 networkTargetPosition;
    public PhotonView photonView;

    public int iterations = 10;
    [Range(0, 1)]
    public float weight = 1.0f;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    public float angleLimit = 90.0f;
    public float distanceLimit = .5f;


    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        if (photonView == null)
        {
            photonView = GetComponent<PhotonView>();
        }
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    public Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if  (PhotonNetwork.IsMasterClient && (aimTransform == null || targetTransform == null))
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            targetPosition = GetTargetPosition();
            networkTargetPosition = targetPosition;
        }
        else
        {
            targetPosition = networkTargetPosition;
        }

            for (int i = 0; i < iterations; i++)
            {
                for (int b = 0; b < boneTransforms.Length; b++)
                {
                    Transform bone = boneTransforms[b];
                    float boneWeight = humanBones[b].weight * weight;
                    AimAtTarget(bone, networkTargetPosition, boneWeight);
                    //photonView.RPC("AimAtTarget", RpcTarget.All, bone, targetPosition, boneWeight);
                }
                //AimAtTarget(bone, targetPosition, weight);
            }

    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }

    public IEnumerator EaseInBoneWeight()
    {
        weight = 0.0f;
        while (weight < 1)
        {
            weight += 0.01f;
            yield return new WaitForSeconds(.1f);
        }
        weight = 1.0f;
    }
    public IEnumerator EaseOutBoneWeight()
    {
        while (weight > 0)
        {
            weight -= 0.5f;
            yield return new WaitForSeconds(.1f);
        }
        weight = 0.0f;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetPosition);
            stream.SendNext(weight);
        } else if (stream.IsReading)
        {
            networkTargetPosition = (Vector3) stream.ReceiveNext();
            weight = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void RPC_EnableWeaponIK()
    {
        photonView.RPC("EnableWeaponIK", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_DisableWeaponIK()
    {
        photonView.RPC("DisableWeaponIK", RpcTarget.All);
    }

    [PunRPC]
    public void EnableWeaponIK()
    {
        enabled = true;
    }
    [PunRPC]

    public void DisableWeaponIK()
    {
        enabled = false;
    }
}
