using UnityEngine;
using UnityEngine.Events;
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip leverPullSound;
    [SerializeField] private string _prompt;
    private bool _leverStatus;
    private Animator _anim;
    [SerializeField] private UnityEvent _event;

    public string InteractionPrompt => _prompt;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public bool Interact(Interactor interactor)
    {
        if (_leverStatus == false)
        {
            Debug.Log(_leverStatus);
            _anim.SetBool("LeverStatus", true);
            _event.Invoke();
            _leverStatus = !_leverStatus;
            SoundEffectsManager.instance.PlaySoundFXClip(leverPullSound, transform, 1f);
            return true;
        }
        else
        {
            Debug.Log(_leverStatus);
            _anim.SetBool("LeverStatus", false);
            _leverStatus = !_leverStatus;
            SoundEffectsManager.instance.PlaySoundFXClip(leverPullSound, transform, 1f);
            return false;
        }
    }
   
}
