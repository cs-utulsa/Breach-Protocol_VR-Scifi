using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarrier : MonoBehaviour
{
    public Material material;

    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        material = this.GetComponent<MeshRenderer>().material;
        offset = Random.Range(0.0f, 1.0f);
    }
    void FixedUpdate()
    {
        material.mainTextureOffset = new Vector2(offset, 0);
        offset += .01f;
        if (offset >= 1.0f)
        {
            offset = 0.0f;
        }
    }
}
