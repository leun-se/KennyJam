using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    [HideInInspector] public float viewAngle;
    [HideInInspector] public float viewDistance;

    public int segments = 30;

    private MeshFilter meshFilter;

    private float[] vertexDistances;

    private float lastViewAngle;
    private float lastViewDistance;
    private Quaternion lastRotation;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        vertexDistances = new float[segments + 1];
    }

    void Start()
    {
        lastViewAngle = viewAngle;
        lastViewDistance = viewDistance;
        lastRotation = transform.rotation;

        for (int i = 0; i < vertexDistances.Length; i++)
            vertexDistances[i] = viewDistance;

        GenerateConeMesh();
    }

    void Update()
    {
        if (viewAngle != lastViewAngle || viewDistance != lastViewDistance || transform.rotation != lastRotation)
        {
            GenerateConeMesh();
            lastViewAngle = viewAngle;
            lastViewDistance = viewDistance;
            lastRotation = transform.rotation;
        }
    }

    public void UpdateVertexDistances(float[] distances)
    {
        if (distances == null || distances.Length != segments + 1)
        {
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        float angleStep = viewAngle / segments;
        float startAngle = -viewAngle / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 dir = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
            vertices[i + 1] = dir * distances[i];
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
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

            float dist = viewDistance;
            if (i < vertexDistances.Length)
                dist = vertexDistances[i];

            Vector3 point = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * dist;
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