using UnityEngine;

public class MindController : MonoBehaviour
{
    public GameObject possessionArrowPrefab;

    private GameObject currentControlledCharacter;
    private GameObject currentArrow;

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

            if (currentArrow != null)
                Destroy(currentArrow);
        }

        currentControlledCharacter = newCharacter;

        SetPossessedHighlight(currentControlledCharacter, true);

        var newController = currentControlledCharacter.GetComponent<PlayerController>();
        if (newController != null)
            newController.enabled = true;

        if (possessionArrowPrefab != null)
        {
            currentArrow = Instantiate(possessionArrowPrefab, currentControlledCharacter.transform);

            currentArrow.transform.localPosition = new Vector3(0, 1.8f, 0);
            currentArrow.transform.localRotation = Quaternion.identity;
        }
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

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    private void Update()
    {
        if (currentArrow != null && Camera.main != null)
        {
            Vector3 dir = Camera.main.transform.position - currentArrow.transform.position;
            dir.y = 0;
            currentArrow.transform.rotation = Quaternion.LookRotation(-dir);
        }
    }
}