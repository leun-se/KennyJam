using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera PlayerCamera;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TextMeshProUGUI PromptText;

    private void Start()
    {
        PlayerCamera = Camera.main;

        if (UIPanel != null)
            UIPanel.SetActive(false);
        else
            Debug.LogError("UIPanel is not assigned on " + gameObject.name);
    }

    private void LateUpdate()
    {
        var rotation = PlayerCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool IsDisplayed = false;
    public void SetUp(string promptText)
    {
        PromptText.text = promptText;
        UIPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        UIPanel.SetActive(false);
        IsDisplayed = false;
    }
}