using UnityEngine;

[RequiredComponent(typeof(Controls))]
public class PossessableCharacter : MonoBehaviour
{
  public float moveSpeed = 5f;
  private bool isPossessed = false;
  private CharacterController controller;

  void Start()
  {
    controller = GetComponent<CharacterController>();
  }

  void Update()
  {
    if (!isPossessed) return;

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 move = new Vector3(h, 0, v);
    controller.SimpleMove(move * moveSpeed);
  }

  public void SetPossessed(bool value)
  {
    isPossessed = value;

    if (isPossessed)
    {
      Debug.Log($"{gameObject.name} is now possessed!");
    }
    else
    {
      Debug.Log($"{gameObject.name} is no longer possessed!");
    }
  }

  public bool IsPossessed()
  {
    return isPossessed;
  }
}
