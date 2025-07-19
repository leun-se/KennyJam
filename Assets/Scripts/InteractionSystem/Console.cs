using UnityEngine;

public class Console : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var skillCheck = interactor.GetComponent<CharacterAttributes>();

        if (skillCheck == null)
        {
            Debug.Log("Unable to open console, need hacking skill");
            return false;
        }
        else
        {
            Debug.Log("Console used, opening door");
            return true;
        }
       
    }
        
}
