using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //private footsteps steps = new footsteps();
    private Vector2 moveInput;
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerControls inputActions;

    public bool isFrozen = false;

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
        if (isFrozen) return;

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            //steps.walking();
            Vector3 newPos = rb.position + move * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            Quaternion targetRot = Quaternion.LookRotation(-move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
        //else
        //{
        //    steps.doneWalking();
        //}

        if (animator != null)
        {
            animator.SetFloat("Speed", move.magnitude);
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Freeze()
    {
        isFrozen = true;
        moveInput = Vector2.zero;

        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }
    
}