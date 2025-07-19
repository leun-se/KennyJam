using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    private bool _leverStatus;
    private Animator _anim;

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
            _leverStatus = !_leverStatus;
            return true;
        }
        else
        {
            Debug.Log(_leverStatus);
            _anim.SetBool("LeverStatus", false);
            _leverStatus = !_leverStatus;
            return false;
        }
    }
   
}
