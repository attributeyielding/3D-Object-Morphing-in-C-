using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MorphOBJ : MonoBehaviour
{
    public Mesh meshA; // First mesh
    public Mesh meshB; // Second mesh (must have the same vertex count as meshA)
    public float morphDuration = 2f; // Duration of the morph in seconds

    private MeshFilter meshFilter;
    private Mesh morphedMesh;
    private float morphProgress = 0f;
    private bool isMorphing = false;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (meshA == null || meshB == null)
        {
            Debug.LogError("Please assign both meshes in the inspector.");
            return;
        }

        if (meshA.vertexCount != meshB.vertexCount)
        {
            Debug.LogError("Meshes must have the same vertex count to morph.");
            return;
        }

        // Create a new mesh for the morphing process
        morphedMesh = new Mesh();
        meshFilter.mesh = morphedMesh;

        // Initialize the morphed mesh with the first mesh
        morphedMesh.vertices = meshA.vertices;
        morphedMesh.triangles = meshA.triangles;
        morphedMesh.normals = meshA.normals;
    }

    void Update()
    {
        if (isMorphing)
        {
            morphProgress += Time.deltaTime / morphDuration;

            if (morphProgress >= 1f)
            {
                morphProgress = 1f;
                isMorphing = false;
            }

            MorphMeshes(morphProgress);
        }

        // Example input to start morphing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMorph();
        }
    }

    public void StartMorph()
    {
        morphProgress = 0f;
        isMorphing = true;
    }

    private void MorphMeshes(float t)
    {
        Vector3[] verticesA = meshA.vertices;
        Vector3[] verticesB = meshB.vertices;
        Vector3[] morphedVertices = new Vector3[verticesA.Length];

        for (int i = 0; i < verticesA.Length; i++)
        {
            morphedVertices[i] = Vector3.Lerp(verticesA[i], verticesB[i], t);
        }

        morphedMesh.vertices = morphedVertices;
        morphedMesh.triangles = meshA.triangles; // Assuming the same topology
        morphedMesh.RecalculateNormals();
    }
}
