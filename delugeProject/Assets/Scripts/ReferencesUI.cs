using UnityEngine;
using UnityEngine.InputSystem;

public class ReferencesUI : MonoBehaviour
{
    [SerializeField] GameObject referencesUIContainer;

    private InputAction escapeUI;

    private void Start()
	{
		escapeUI = InputSystem.actions.FindAction("EscapeUI");
	}
    private void Update()
	{
		if (escapeUI.IsPressed() && referencesUIContainer.activeSelf)
		{
			CloseReferencesUI();
		}
	}

    public void OpenReferencesUI()
    {
        referencesUIContainer.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseReferencesUI()
    {
        referencesUIContainer.SetActive(false);
        Time.timeScale = 1f;
    }
}
