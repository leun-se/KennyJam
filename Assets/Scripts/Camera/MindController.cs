using UnityEngine;

public class MindController : MonoBehaviour
{
    private GameObject currentControlledCharacter;

    public GameObject CurrentControlledCharacter => currentControlledCharacter;

    public bool IsPossessed(GameObject obj)
    {
        return obj == currentControlledCharacter;
    }

    // Highlights possessed character yellow, or resets color
    public void SetPossessedHighlight(GameObject obj, bool possessed)
    {
        if (obj == null) return;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.material.color = possessed ? Color.yellow : Color.white;
        }
    }

    public void Possess(GameObject newCharacter)
    {
        if (newCharacter == null) return;

        // Reset old possessed highlight
        if (currentControlledCharacter != null)
            SetPossessedHighlight(currentControlledCharacter, false);

        currentControlledCharacter = newCharacter;

        // Highlight new possessed character yellow
        SetPossessedHighlight(currentControlledCharacter, true);

        var newMovement = currentControlledCharacter.GetComponent<PlayerController>();
        if (newMovement != null)
            newMovement.enabled = true;
    }

    public void ReturnControlToMind()
    {
        if (currentControlledCharacter != null)
        {
            SetPossessedHighlight(currentControlledCharacter, false);

            var oldMovement = currentControlledCharacter.GetComponent<PlayerController>();
            if (oldMovement != null)
                oldMovement.enabled = false;
        }

        currentControlledCharacter = null;
    }
}
