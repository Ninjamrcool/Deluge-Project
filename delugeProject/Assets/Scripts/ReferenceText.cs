using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ReferenceText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text referenceText;
    [SerializeField] float nonBlueLevel = 0.2f;
    private string link;

    public void SetLink(string newLink)
    {
        link = newLink;
    }


    public void ReferenceClicked()
    {
        Application.OpenURL(link);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		referenceText.color = new Color(nonBlueLevel, nonBlueLevel, referenceText.color.b, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		referenceText.color = new Color(1f, 1f, referenceText.color.b, 1f);
    }
}
