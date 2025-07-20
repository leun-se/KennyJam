using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CapsuleCollider))]
public class GuardVision : MonoBehaviour
{
    private Animator animator;

    [Header("Detection Settings")]
    public float viewDistance = 1f;
    public float viewAngle = 90f;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    [Header("Rotation Settings")]
    public bool patrolRotate = false;
    public float rotationSpeed = 2f;
    public float waitTimeAtEachRotation = 2f;
    public float[] patrolAngles = { 0f, 90f, 180f, 270f };


    [Header("References")]
    public GameObject exclamationPoint;
    public VisionConeRenderer coneRenderer;

    [Header("Chase Settings")]
    public float chaseSpeed = 2f;

    private Transform targetPlayer;
    private bool isChasing = false;
    private bool playerFrozen = false;

    private CapsuleCollider guardCollider;

    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isRotating = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        guardCollider = GetComponent<CapsuleCollider>();

        if (exclamationPoint != null)
            exclamationPoint.SetActive(false);

        if (coneRenderer != null)
        {
            coneRenderer.viewAngle = viewAngle;
            coneRenderer.viewDistance = viewDistance;
        }
    }

    private void Update()
    {
        if (!isChasing)
        {
            if (patrolRotate && patrolAngles.Length > 0)
                PatrolRotate();
            else
                DetectPlayer();
            UpdateConeVisibility();
            SetWalkingAnimation(false);
        }
        else
        {
            ChasePlayer();
            SetWalkingAnimation(true);
        }
    }

    private void PatrolRotate()
    {
        if (isRotating)
        {
            float targetAngle = patrolAngles[currentPatrolIndex];
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                isRotating = false;
                waitTimer = 0f;
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtEachRotation)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolAngles.Length;
                isRotating = true;
            }
        }
    }
    
    private void UpdateConeVisibility()
    {
        if (coneRenderer == null) return;

        int rayCount = coneRenderer.segments;
        float angleStep = viewAngle / rayCount;
        float startAngle = -viewAngle / 2f;

        float[] distances = new float[rayCount + 1];

        Vector3 rayOrigin = transform.position + Vector3.up * 0.25f;

        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Vector3 rayDirection = transform.rotation * Quaternion.Euler(0f, currentAngle, 0f) * -Vector3.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, viewDistance, obstacleMask))
            {
                distances[i] = hit.distance;
            }
            else
            {
                distances[i] = viewDistance;
            }
        }

        coneRenderer.UpdateVertexDistances(distances);
    }

    private void SetWalkingAnimation(bool walking)
    {
        if (animator != null)
            animator.SetBool("isWalking", walking);
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider hit in hits)
        {
            Transform player = hit.transform;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.25f;
            Vector3 dirToPlayer = (player.position - rayOrigin).normalized;
            float angle = Vector3.Angle(-transform.forward, dirToPlayer);

            if (angle < viewAngle / 2f)
            {
                float distance = Vector3.Distance(rayOrigin, player.position);

                if (Physics.Raycast(rayOrigin, dirToPlayer, out RaycastHit hitInfo, distance, obstacleMask))
                {
                    continue;
                }

                targetPlayer = player;
                isChasing = true;

                if (!playerFrozen)
                {
                    PlayerController controller = player.GetComponent<PlayerController>();
                    if (controller != null)
                    {
                        controller.Freeze();
                        playerFrozen = true;
                    }
                }

                if (exclamationPoint != null)
                    exclamationPoint.SetActive(true);

                if (coneRenderer != null)
                    coneRenderer.gameObject.SetActive(false);

                break;
            }
        }
    }

    private void ChasePlayer()
    {
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        transform.position += direction * chaseSpeed * Time.deltaTime;

        Collider playerCollider = targetPlayer.GetComponent<Collider>();
        if (guardCollider != null && playerCollider != null)
        {
            if (guardCollider.bounds.Intersects(playerCollider.bounds))
            {
                SetWalkingAnimation(false);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 forward = -transform.forward;
        float halfAngle = viewAngle / 2f;

        Vector3 right = Quaternion.Euler(0, halfAngle, 0) * forward;
        Vector3 left = Quaternion.Euler(0, -halfAngle, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, right * viewDistance);
        Gizmos.DrawRay(transform.position, left * viewDistance);
    }
}
