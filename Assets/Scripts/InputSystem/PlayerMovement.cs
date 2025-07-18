using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        animator.SetFloat("Speed", moveInput.magnitude);
    }
}