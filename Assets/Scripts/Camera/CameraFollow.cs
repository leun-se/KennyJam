using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(10, 10, -10);
    public float followSpeed = 5f;

    private Transform target;
    private Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = transform.position;
        transform.rotation = Quaternion.Euler(10, 10, 0); 
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Maintain rotation and follow direction
            transform.position = Vector3.Lerp(
                transform.position,
                target.position + transform.rotation * offset,
                followSpeed * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                defaultPosition,
                followSpeed * Time.deltaTime
            );
        }
    }
}