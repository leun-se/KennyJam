using UnityEngine;

public class MainCharacter : MonoBehaviour
{
  public MindControlManager controlManager;
  public Camera monitorCamera;

  void Start()
  {
    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = true;
  }

  void Update()
  {
    // possession logic in other scripts
  }

  public void PossessTarget(PossessableCharacter target)
  {
    controlManager.PossessTarget(target);
  }

  public void ReleaseControl()
  {
    controlManager.ReleaseControl();
  }
}
