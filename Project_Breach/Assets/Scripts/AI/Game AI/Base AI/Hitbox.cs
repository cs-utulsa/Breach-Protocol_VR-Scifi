using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public AI_Health aiHealth;

    [PunRPC]
    public void OnRaycastHit(float value, Vector3 direction)
    {
        aiHealth.TakeDamage(value, direction);
    }

}
