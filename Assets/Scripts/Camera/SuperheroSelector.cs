using UnityEngine;
using UnityEngine.InputSystem;

public class SuperheroSelector : MonoBehaviour
{
    public float rayDistance = 10f;
    public LayerMask possessableLayer;
    public MindController mindController;

    private GameObject lastHovered;
    private Renderer[] lastHoveredRenderers;
    private Material[][] lastOriginalMaterials;

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();

        inputActions.Player.Possess.performed += ctx => AttemptPossess();
        inputActions.Player.Return.performed += ctx => AttemptReturn();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, possessableLayer))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj != lastHovered)
            {
                ClearHighlight();

                if (!mindController.IsPossessed(hitObj))
                {
                    HighlightGreen(hitObj);
                    lastHovered = hitObj;
                }
                else
                {
                    lastHovered = null;
                }
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void AttemptPossess()
    {
        if (lastHovered != null)
        {
            mindController.Possess(lastHovered);
            ClearHighlight();
        }
    }

    private void AttemptReturn()
    {
        mindController.ReturnControlToMind();
    }

    private void HighlightGreen(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        lastOriginalMaterials = new Material[renderers.Length][];
        lastHoveredRenderers = renderers;

        for (int i = 0; i < renderers.Length; i++)
        {
            lastOriginalMaterials[i] = renderers[i].materials;
            Material[] greenMats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < greenMats.Length; j++)
            {
                greenMats[j] = new Material(renderers[i].materials[j]);
                greenMats[j].color = Color.violet; // this line changes color
            }
            renderers[i].materials = greenMats;
        }
    }

    private void ClearHighlight()
    {
        if (lastHoveredRenderers != null)
        {
            for (int i = 0; i < lastHoveredRenderers.Length; i++)
            {
                lastHoveredRenderers[i].materials = lastOriginalMaterials[i];
            }
        }

        lastHovered = null;
        lastHoveredRenderers = null;
        lastOriginalMaterials = null;
    }
}
