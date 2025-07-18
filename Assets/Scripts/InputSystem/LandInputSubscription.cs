using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;

public class LandInputSubscription : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool MenuInput { get; private set; } = false;
    public bool ActionInput { get; private set; } = false;
    InputMap _Input = null;

    private void OnEnable()//subscribe to inputs
    {
        _Input = new InputMap();
        _Input.PlayerInputMap.Enable();

        _Input.PlayerInputMap.Movement.performed += SetMovement;
        _Input.PlayerInputMap.Movement.canceled += SetMovement;

        _Input.PlayerInputMap.ActionInput.started += SetAction;
        _Input.PlayerInputMap.ActionInput.canceled += SetAction;
    }

    private void OnDisable()//unsubscribe to inputs
    {
        _Input.PlayerInputMap.Movement.performed -= SetMovement;
        _Input.PlayerInputMap.Movement.canceled -= SetMovement;

        _Input.PlayerInputMap.ActionInput.performed -= SetAction;
        _Input.PlayerInputMap.ActionInput.canceled -= SetAction;

        _Input.PlayerInputMap.Disable();
    }

    private void Update()
    {
        MenuInput = _Input.PlayerInputMap.Menu.WasPressedThisFrame();
    }

    void SetMovement(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    void SetAction(InputAction.CallbackContext ctx)
    {
        ActionInput = ctx.started;
    }


}
