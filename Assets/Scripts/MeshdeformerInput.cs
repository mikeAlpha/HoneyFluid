using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshdeformerInput : MonoBehaviour
{
    [SerializeField]
    private float force = 10f;

    [SerializeField]
    private float offset = 0.1f;

    List<Meshdeformer> mds = new List<Meshdeformer>();

    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
            UpdateInput();
        else if(mds.Count > 0)
        {
            foreach(Meshdeformer m in mds)
            {
                m.IsDeforming = false;
            }
                
        }
            
    }

    void UpdateInput()
    {
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(mRay , out hitInfo , Mathf.Infinity))
        {
            Meshdeformer md = hitInfo.collider.GetComponent<Meshdeformer>();
            if (md)
            {
                Vector3 point = hitInfo.point;
                point += hitInfo.normal * offset;
                md.AddDeformingForceAtPoint(point, force , hitInfo.normal);
            }
        }
    }
}
