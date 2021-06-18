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

    void OnEnable()
    {
        EventHandler.RegisterEvent<Vector3>("UpdateInputPosition", UpdateInput);
    }

    void OnDisable()
    {
        EventHandler.UnregisterEvent<Vector3>("UpdateInputPosition", UpdateInput);
    }

    void UpdateInput(Vector3 pos)
    {
        Ray mRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit hitInfo;
        if(Physics.Raycast(mRay , out hitInfo , Mathf.Infinity))
        {
            Meshdeformer md = hitInfo.collider.GetComponent<Meshdeformer>();
            if (md)
            {
                Vector3 point = hitInfo.point;
                point += hitInfo.normal * offset;
                md.AddDeformingForceAtPoint(point, force , hitInfo.normal);
                //mds.Add(md);
            }
        }
    }
}
