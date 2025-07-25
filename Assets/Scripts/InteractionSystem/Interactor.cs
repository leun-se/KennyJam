using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private MindController mindController;
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private IInteractable _interactable;

    private void Awake()
    {
        mindController = FindFirstObjectByType<MindController>();
    }
    private void Update()
    {
        if (mindController != null && mindController.CurrentControlledCharacter != gameObject)
        {
            if (_interactionPromptUI.IsDisplayed)
                _interactionPromptUI.Close();

            return;
        }

        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();
            GameObject foundCharacter = _colliders[0].gameObject;

            if (_interactable != null)
            {
                if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                if (Keyboard.current.fKey.wasPressedThisFrame) _interactable.Interact(this);
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

}
