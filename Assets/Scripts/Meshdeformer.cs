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

    float uniformScale = 1f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        _vertices = mesh.vertices;
        _dvertices = _vertices;
        _vvelocities = new Vector3[_vertices.Length];
    }

    void Update()
    {

        uniformScale = transform.localScale.x;

        for (int i = 0; i < _dvertices.Length; i++)
            UpdateVertex(i);

        mesh.vertices = _dvertices;
        mesh.RecalculateNormals();
    }

    void UpdateVertex(int index)
    {
        Vector3 vel = _vvelocities[index];
        Vector3 displacement = _dvertices[index] - _vertices[index];
        displacement *= uniformScale;
        vel -= displacement * 20f * Time.deltaTime;
        vel *= 1f - Damping * Time.deltaTime;
        _vvelocities[index] = vel;
        _dvertices[index] += vel * Time.deltaTime;
    }

    public void AddDeformingForceAtPoint(Vector3 point , float force , Vector3 normal)
    {
        Debug.DrawLine(Camera.main.transform.position, point);
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < _dvertices.Length; i++)
        {
            AddForceAtVertex(i, point, force);
        }
    }

    void AddForceAtVertex(int index , Vector3 point , float force)
    {
        Vector3 pointToVertex = _dvertices[index] - point;
        pointToVertex *= uniformScale;
        float ForceVector = force / (1 + pointToVertex.sqrMagnitude);
        float velocity = ForceVector * Time.deltaTime;
        _vvelocities[index] += pointToVertex.normalized * velocity;
    }
}
