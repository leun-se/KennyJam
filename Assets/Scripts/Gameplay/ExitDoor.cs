using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private AudioClip CharacterExitSoundCLip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Possessable"))
        {
            Debug.Log($"{other.name} exited!");

            var mind = FindFirstObjectByType<MindController>();
            if (mind != null && mind.IsPossessed(other.gameObject))
            {
                mind.ReturnControlToMind();
            }

            ExitAndShrink exit = other.GetComponent<ExitAndShrink>();
            if (exit != null)
            {
                Vector3 doorDirection = transform.forward;
                SoundEffectsManager.instance.PlaySoundFXClip(CharacterExitSoundCLip, transform, 1f);
                exit.StartExit(doorDirection);
            }
            else
            {
                other.gameObject.SetActive(false);
            }

            GameManager.Instance?.RegisterEscape();
        }
    }
}
