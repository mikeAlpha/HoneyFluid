using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meshdeformer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] _vertices, _dvertices;
    Vector3[] _vvelocities;

    public float Damping = 5f;
    public bool IsDeforming = false;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        _vertices = mesh.vertices;
        _dvertices = _vertices;
        _vvelocities = new Vector3[_vertices.Length];
    }

    void Update()
    {
        if (IsDeforming)
            mesh.vertices = _dvertices;
        else
        {
            mesh.vertices = _vertices;
            mesh.RecalculateNormals();
            return;
        }

        for (int i = 0; i < _dvertices.Length; i++)
            UpdateVertex(i);

        mesh.RecalculateNormals();
    }

    void UpdateVertex(int index)
    {
        Vector3 vel = _vvelocities[index];
        vel *= 1f - Damping * Time.deltaTime;
        _dvertices[index] += vel * Time.deltaTime;
    }

    public void AddDeformingForceAtPoint(Vector3 point , float force , Vector3 normal)
    {
        IsDeforming = true;
        Debug.DrawLine(Camera.main.transform.position, point);
        for (int i = 0; i < _dvertices.Length; i++)
        {
            AddForceAtVertex(i, point, force);
        }
    }

    void AddForceAtVertex(int index , Vector3 point , float force)
    {
        Vector3 pt = transform.InverseTransformPoint(point);
        Vector3 nrl = transform.InverseTransformPoint(point);

        Vector3 pointToVertex = _dvertices[index] - pt;
        float ForceVector = force / (1 + pointToVertex.sqrMagnitude);
        float velocity = ForceVector * Time.deltaTime;
        _vvelocities[index] += 2f * 0.05f * pointToVertex.normalized * velocity * 10;
    }
}
