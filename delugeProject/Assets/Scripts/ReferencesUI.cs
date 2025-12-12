using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ReferencesUI : MonoBehaviour
{
    [System.Serializable]
    public struct Reference
	{
        [TextArea]
		public string text;
        [TextArea]
        public string link;
	} 

    [SerializeField] GameObject referencesUIContainer;
    [SerializeField] GameObject referencesTextContainer;
    [SerializeField] GameObject referenceTextPrefab;
    [SerializeField] Reference[] references;

    private InputAction escapeUI;

    private void Start()
	{
		escapeUI = InputSystem.actions.FindAction("EscapeUI");

        for (int i = 0; i < references.Length; i++)
		{
			GameObject reference = Instantiate(referenceTextPrefab, referencesTextContainer.transform);
            reference.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = references[i].text;
            reference.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().ForceMeshUpdate();
            int lineCount = reference.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().textInfo.lineCount;
            reference.GetComponent<RectTransform>().sizeDelta = new Vector2(reference.GetComponent<RectTransform>().sizeDelta.x, lineCount * 33f);
            reference.GetComponent<ReferenceText>().SetLink(references[i].link);
		}
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
