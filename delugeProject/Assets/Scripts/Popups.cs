using UnityEngine;
using TMPro;

public class Popups : MonoBehaviour
{

    [SerializeField] RectTransform popupBody;
    [SerializeField] RectTransform popupTop;
    [SerializeField] TMP_Text popupText;
    [SerializeField] int linesRequiredToScaleUp;

	private float popupBodyHeight;
	private float popupTopStartingY;
	private float popupTextStartingY;

	private void Awake()
	{
		popupBodyHeight = popupBody.rect.height;
		popupTopStartingY = popupTop.anchoredPosition.y;
		popupTextStartingY = popupText.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
	}

    //Called when popup is created by PopupCreator script
    public void SetText(string newText)
	{
		popupText.text = newText;

		popupText.ForceMeshUpdate();

        int lineCount = popupText.textInfo.lineCount;
		if (lineCount >= linesRequiredToScaleUp)
		{
			popupBody.localScale = new Vector3(popupBody.localScale.x, (1f / (linesRequiredToScaleUp - 1)) * lineCount, popupBody.localScale.z);
			float changeInHeight = popupBodyHeight * (popupBody.localScale.y - 1);

			popupTop.anchoredPosition = new Vector2(popupTop.anchoredPosition.x, popupTopStartingY + changeInHeight);
			popupText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(popupText.gameObject.GetComponent<RectTransform>().anchoredPosition.x, popupTextStartingY + changeInHeight);
		}
		else
		{
			popupBody.localScale = new Vector3(popupBody.localScale.x, 1f, popupBody.localScale.z);
		}

	}
}
