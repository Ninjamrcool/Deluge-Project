using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class Popups : MonoBehaviour
{

    [SerializeField] RectTransform popupBody;
    [SerializeField] RectTransform popupTop;
    [SerializeField] RectTransform popupBottom;
    [SerializeField] TMP_Text popupText;
	[SerializeField] Image[] images;
	[SerializeField] float popupFadeTime;
    [SerializeField] int linesRequiredToScaleUp;
    [SerializeField] Vector2[] popupDirectionOffsets;
    [SerializeField] GameObject[] pointers;

	private int direction;
	private float popupBodyHeight;
	private float popupBodyStartingY;
	private float popupTopStartingY;
	private float popupBottomStartingY;
	private float popupTextStartingY;

	private void Awake()
	{
		popupBodyHeight = popupBody.rect.height;
		popupBodyStartingY = popupBody.anchoredPosition.y;
		popupTopStartingY = popupTop.anchoredPosition.y;
		popupBottomStartingY = popupBottom.anchoredPosition.y;
		popupTextStartingY = popupText.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
		StartCoroutine(FadeImagesTo(0f, 0f));
	}

	private void Start()
	{
		StartCoroutine(FadeImagesTo(1f, popupFadeTime));
	}

    //Called when popup is created by PopupCreator script
    public void SetText(string newText)
	{
		popupText.text = newText;

		popupText.ForceMeshUpdate();

        int lineCount = popupText.textInfo.lineCount;
		if (lineCount < linesRequiredToScaleUp)
		{
			popupBody.localScale = new Vector3(popupBody.localScale.x, 1f, popupBody.localScale.z);
			return;
		}

		popupBody.localScale = new Vector3(popupBody.localScale.x, (1f / (linesRequiredToScaleUp - 1)) * lineCount, popupBody.localScale.z);
		float changeInHeight = popupBodyHeight * (popupBody.localScale.y - 1);

	
		if (direction == 0){//north
			popupBody.anchoredPosition = new Vector2(popupBody.anchoredPosition.x, popupBodyStartingY + (-0.5f * changeInHeight));
			popupBottom.anchoredPosition = new Vector2(popupBottom.anchoredPosition.x, popupBottomStartingY - changeInHeight);
		}
		else if (direction == 1){//south
			popupBody.anchoredPosition = new Vector2(popupBody.anchoredPosition.x, popupBodyStartingY + (0.5f * changeInHeight));
			popupText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(popupText.gameObject.GetComponent<RectTransform>().anchoredPosition.x, popupTextStartingY + changeInHeight);
			popupTop.anchoredPosition = new Vector2(popupTop.anchoredPosition.x, popupTopStartingY + changeInHeight);
		}
		else
		{
			popupText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(popupText.gameObject.GetComponent<RectTransform>().anchoredPosition.x, popupTextStartingY + (0.5f * changeInHeight));
			popupTop.anchoredPosition = new Vector2(popupTop.anchoredPosition.x, popupTopStartingY + (0.5f * changeInHeight));
			popupBottom.anchoredPosition = new Vector2(popupBottom.anchoredPosition.x, popupBottomStartingY - (0.5f * changeInHeight));
		}
	}

	public void FadeOutAndDestroy()
	{
		Destroy(gameObject, popupFadeTime);
		StopAllCoroutines();
		StartCoroutine(FadeImagesTo(0f, popupFadeTime));
	}

	//Called when popup is created by PopupCreator script
	public void SetDirection(int newDirection)
	{
		direction = newDirection;
		gameObject.GetComponent<RectTransform>().anchoredPosition += popupDirectionOffsets[direction];
		pointers[direction].SetActive(true);
	}

	private IEnumerator FadeImagesTo(float alpha, float fadeTime)
	{
        float time = 0f;
        float originalAlpha = images[0].color.a;
        while(time < fadeTime)
		{
			for (int i = 0; i < images.Length; i++)
			{
				images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.SmoothStep(originalAlpha, alpha, time/fadeTime));
			}
			popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, Mathf.SmoothStep(originalAlpha, alpha, time/fadeTime));
            time += Time.deltaTime;
            yield return null;
        }

		for (int i = 0; i < images.Length; i++)
		{
			images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.SmoothStep(originalAlpha, alpha, 1));
		}
		popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, Mathf.SmoothStep(originalAlpha, alpha, 1));
	}
}
