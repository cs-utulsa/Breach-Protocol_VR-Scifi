using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode] 
public class AI_Sensor : MonoBehaviour
{
    public AI_Data aiData;
    public Color meshColor = Color.red;

    private LayerMask targetLayers;
    private LayerMask occlusionLayers;
    Collider[] colliders = new Collider[50];

    public List<GameObject> Objects
    {
        get
        {
            objects.RemoveAll(obj => !obj);
            return objects;
        }
    }

    private List<GameObject> objects = new List<GameObject>();
    public GameObject target;

    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / aiData.scanFrequency;
        targetLayers = LayerMask.GetMask(aiData.targetLayers);
        occlusionLayers = LayerMask.GetMask(aiData.occlusionLayers);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scanTimer -= Time.fixedDeltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    public void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, aiData.scanDistance, colliders, targetLayers, QueryTriggerInteraction.Collide);
        objects.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj) && obj.CompareTag("Target"))
            {
                //&& obj.CompareTag("Target")
                objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < 0 || direction.y > aiData.scanHeight)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > aiData.scanAngle)
        {
            return false;
        }
        origin.y += aiData.scanHeight / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        return true;
    }


    /*

  Mesh CreateWedgeMesh()
  {
      Mesh mesh = new Mesh();
      int segments = 10;
      int numTriangles = (segments * 4) + 2 + 2;
      int numVerticies = numTriangles * 3;

      Vector3[] verticies = new Vector3[numVerticies];

      int[] triangles = new int[numVerticies];

      Vector3 bottomCenter = Vector3.zero;
      Vector3 bottomLeft = Quaternion.Euler(0, -aiData.scanAngle, 0) * Vector3.forward * aiData.scanDistance;
      Vector3 bottomRight = Quaternion.Euler(0, aiData.scanAngle, 0) * Vector3.forward * aiData.scanDistance;

      Vector3 topCenter = bottomCenter + Vector3.up * aiData.scanAngle;
      Vector3 topRight = bottomRight + Vector3.up * aiData.scanAngle;
      Vector3 topLeft = bottomLeft + Vector3.up * aiData.scanAngle;

      int vert = 0;

      // left side
      verticies[vert++] = bottomCenter;
      verticies[vert++] = bottomLeft;
      verticies[vert++] = topLeft;

      verticies[vert++] = topLeft;
      verticies[vert++] = topCenter;
      verticies[vert++] = bottomCenter;


      // right side
      verticies[vert++] = bottomCenter;
      verticies[vert++] = topCenter;
      verticies[vert++] = topRight;

      verticies[vert++] = topRight;
      verticies[vert++] = bottomRight;
      verticies[vert++] = bottomCenter;


      float currentAngle = -aiData.scanAngle;
      float deltaAngle = (aiData.scanAngle * 2) / segments;
      for (int i = 0; i < segments; ++i)
      {
          bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * aiData.scanDistance;
          bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * aiData.scanDistance;

          topRight = bottomRight + Vector3.up * aiData.scanHeight;
          topLeft = bottomLeft + Vector3.up * aiData.scanHeight;

          // far side
          verticies[vert++] = bottomLeft;
          verticies[vert++] = bottomRight;
          verticies[vert++] = topRight;

          verticies[vert++] = topRight;
          verticies[vert++] = topLeft;
          verticies[vert++] = bottomLeft;

          // top
          verticies[vert++] = topCenter;
          verticies[vert++] = topLeft;
          verticies[vert++] = topRight;


          //bottom
          verticies[vert++] = bottomCenter;
          verticies[vert++] = bottomRight;
          verticies[vert++] = bottomLeft;

          currentAngle += deltaAngle;
      }


      for (int i = 0; i < numVerticies; i++)
      {
          triangles[i] = i;
      }

      mesh.vertices = verticies;
      mesh.triangles = triangles;
      mesh.RecalculateNormals();

      return mesh;

  }



  private void OnValidate()
  {
      mesh = CreateWedgeMesh();
      scanInterval = 1.0f / aiData.scanFrequency;
  }


  private void OnDrawGizmos()
  {
      if (mesh)
      {
          Gizmos.color = meshColor;
          Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
      }


      Gizmos.DrawWireSphere(transform.position, aiData.scanDistance);
      for ( int i = 0; i < count; i++)
      {
          Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
      }

      Gizmos.color = Color.green;
      foreach(var obj in objects)
      {
          Gizmos.DrawSphere(obj.transform.position, 0.2f);
      }

  }
  */



}
