using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.sqrMagnitude > 0.01f)
        {
            Vector3 targetPos = rb.position + move * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            Quaternion targetRot = Quaternion.LookRotation(move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}