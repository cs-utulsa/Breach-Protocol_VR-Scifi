using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BreachableSurface : MonoBehaviour
{
    [Header("Socket")]
    public SocketTagCheck socket;

    [Header("Photon")]
    public PhotonView photonView;

    [Header("Debug")]
    [SerializeField] bool breacherAttached = false;

    void Awake()
    {
        socket = GetComponentInChildren<SocketTagCheck>();
        photonView = GetComponent<PhotonView>();
        breacherAttached = false;
    }

    public void BreacherAttached()
    {
        breacherAttached = true;
    }

    public void BreacherDetached()
    {
        breacherAttached = false;
    }

    public bool IsBreacherAttached()
    {
        return breacherAttached;
    }

}
