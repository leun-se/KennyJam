using UnityEngine;

public class MindControlManager : MonoBehaviour
{
  public PossessableCharacter currentCharacter;

  void update()
  {
    if (Input.GetKeydown(KeyCode.R))
    {
      ReleaseControl();
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      PossessClosest();
    }
  }

  public void Possess(PossessableCharacter character)
  {
    if (currentCharacter != null)
    {
      currentCharacter.SetPossessed(false);
    }

    currentCharacter = character;
    currentCharacter.SetPossessed(true);
  }

  public void ReleaseControl()
  {
    if (currentCharacter != null)
    {
      currentCharacter.SetPossessed(false);
      currentCharacter = null;
    }
  }

  void PossessCloset()
  {
    PossessableCharacter[] all = FindObjectsOfType<PossessableCharacter>();
    float closest = Mathf.Infinity;
    PossessableCharacter nearest = null;

    foreach (var pc in all)
    {
      float dist = Vector3.Distance(transform.position, pc, transform.position);
      if (dist < closest)
      {
        closest = dist;
        nearest = pc;
      }
    }

    if (nearest != null)
    {
      Possess(nearest);
    }
  }
}