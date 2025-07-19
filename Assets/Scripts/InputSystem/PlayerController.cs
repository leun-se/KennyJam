using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;

    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerControls inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        inputActions = new PlayerControls();
        inputActions.Enable();
        inputActions.Player.Restart.performed += ctx => RestartLevel();
    }

    private void OnDestroy()
    {
        inputActions.Player.Restart.performed -= ctx => RestartLevel();
        inputActions.Disable();
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

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}