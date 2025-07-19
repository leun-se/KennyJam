using UnityEngine;

public class MindController : MonoBehaviour
{
    private GameObject currentControlledCharacter;

    public GameObject CurrentControlledCharacter => currentControlledCharacter;

    public bool IsPossessed(GameObject obj)
    {
        return obj == currentControlledCharacter;
    }
    
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
        if (newCharacter == null || newCharacter == currentControlledCharacter)
            return;

        if (currentControlledCharacter != null)
        {
            SetPossessedHighlight(currentControlledCharacter, false);

            var oldController = currentControlledCharacter.GetComponent<PlayerController>();
            if (oldController != null)
                oldController.enabled = false;
        }

        currentControlledCharacter = newCharacter;

        SetPossessedHighlight(currentControlledCharacter, true);

        var newController = currentControlledCharacter.GetComponent<PlayerController>();
        if (newController != null)
            newController.enabled = true;
    }

    public void ReturnControlToMind()
    {
        if (currentControlledCharacter != null)
        {
            SetPossessedHighlight(currentControlledCharacter, false);

            var controller = currentControlledCharacter.GetComponent<PlayerController>();
            if (controller != null)
                controller.enabled = false;

            currentControlledCharacter = null;
        }
    }
}
