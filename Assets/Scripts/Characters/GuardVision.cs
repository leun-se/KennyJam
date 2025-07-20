using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CapsuleCollider))]
public class GuardVision : MonoBehaviour
{
    [Header("Detection Settings")]
    public float viewDistance = 7f;
    public float viewAngle = 90f;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    [Header("References")]
    public GameObject exclamationPoint;
    public VisionConeRenderer coneRenderer;

    [Header("Chase Settings")]
    public float chaseSpeed = 2f;

    private Transform targetPlayer;
    private bool isChasing = false;
    private bool playerFrozen = false;

    private CapsuleCollider guardCollider;

    private void Start()
    {
        guardCollider = GetComponent<CapsuleCollider>();

        if (exclamationPoint != null)
            exclamationPoint.SetActive(false);

        if (coneRenderer != null)
        {
            coneRenderer.viewAngle = viewAngle;
            coneRenderer.viewDistance = viewDistance;
            coneRenderer.GenerateConeMesh();
        }
    }

    private void Update()
    {
        if (!isChasing)
        {
            DetectPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider hit in hits)
        {
            Transform player = hit.transform;

            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(-transform.forward, dirToPlayer);
            
            if (angle < viewAngle / 2f)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distance, obstacleMask))
                {
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
    }

    private void ChasePlayer()
    {
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

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