using UnityEngine;
using UnityEngine.Events;

public class Console : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    private bool _consoleStatus = false;
    [SerializeField] private UnityEvent _event;
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
            if(_consoleStatus == false)
            {
                Debug.Log("openingDoor");
                _event.Invoke();
                _consoleStatus = true;  
            }
            return true;

        }
       
    }
        
}
