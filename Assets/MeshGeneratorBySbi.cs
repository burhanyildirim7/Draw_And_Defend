using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorBySbi : MonoBehaviour
{

    public static MeshGeneratorBySbi instance;

    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> centerVertices = new List<Vector3>();
    private Vector3 lastVert;
    public List<int> triangles = new List<int>();

    public Mesh mesh;

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void CreateShape()
	{
        mesh.Clear();
        triangles.Clear();
        int vertCount = vertices.Count;
		for (int i = 0; i < vertices.Count/2; i++)
		{
            int indexRoot = i * 2;
            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = (indexRoot + 2) % vertCount;
            int indexInnerNext = (indexRoot + 3) % vertCount;

            triangles.Add(indexRoot);
            triangles.Add(indexOuterNext);
            triangles.Add(indexInnerNext);

            triangles.Add(indexRoot);
            triangles.Add(indexOuterNext);
            triangles.Add(indexInnerRoot);            
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();

	}

    public void AddVertice(Vector3 vert)
	{
        if(vert != lastVert) {
            centerVertices.Add(vert);
            lastVert = vert;
        }      
	}

    public void CalculateVertices()
	{
        int lastBounds = centerVertices.Count * 4;
		for (int i = 0; i < lastBounds; i++)
		{
           // vertices[i] = 
		}
	}
}
