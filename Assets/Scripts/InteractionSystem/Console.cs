using UnityEngine;
using UnityEngine.Events;

public class Console : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip consoleActivationSoundClip;
    [SerializeField] private AudioClip consoleNotActivatedSoundClip;
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
            SoundEffectsManager.instance.PlaySoundFXClip(consoleNotActivatedSoundClip, transform, 1f);
            return false;
        }
        else
        {
            if(_consoleStatus == false)
            {
                SoundEffectsManager.instance.PlaySoundFXClip(consoleActivationSoundClip, transform, 1f);
                Debug.Log("openingDoor");
                _event.Invoke();
                _consoleStatus = true;  
            }
            return true;

        }
       
    }
        
}
