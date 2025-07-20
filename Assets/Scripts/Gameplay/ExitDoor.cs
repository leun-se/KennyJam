using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Possessable"))
        {
            Debug.Log($"{other.name} exited!");

            var mind = FindObjectOfType<MindController>();
            if (mind != null && mind.IsPossessed(other.gameObject))
            {
                mind.ReturnControlToMind();
            }

            var exit = other.GetComponent<ExitAndShrink>();
            if (exit != null)
            {
                Vector3 doorDir = transform.forward;
                doorDir.y = 0f;
                doorDir.Normalize();
                exit.StartExit(doorDir);
            }

            GameManager.Instance?.RegisterEscape();
        }
    }
}
