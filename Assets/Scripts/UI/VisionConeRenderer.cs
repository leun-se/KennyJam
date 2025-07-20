using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    [HideInInspector] public float viewAngle;
    [HideInInspector] public float viewDistance;

    public int segments = 30;

    private MeshFilter meshFilter;

    public float ViewAngle
    {
        get => viewAngle;
        set
        {
            if (viewAngle != value)
            {
                viewAngle = value;
                GenerateConeMesh();
            }
        }
    }

    public float ViewDistance
    {
        get => viewDistance;
        set
        {
            if (viewDistance != value)
            {
                viewDistance = value;
                GenerateConeMesh();
            }
        }
    }

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        GenerateConeMesh();
    }

    public void GenerateConeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float angleStep = viewAngle / segments;
        float startAngle = -viewAngle / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 point = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * viewDistance;
            vertices[i + 1] = point;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
