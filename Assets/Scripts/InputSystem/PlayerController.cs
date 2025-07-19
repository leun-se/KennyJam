using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;

    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 newPos = rb.position + move * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            Quaternion targetRot = Quaternion.LookRotation(-move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", move.magnitude);
        }
    }
}