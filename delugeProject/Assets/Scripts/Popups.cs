using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Popups : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] RectTransform popupBody;
    [SerializeField] RectTransform popupTop;
    [SerializeField] RectTransform popupBottom;
    [SerializeField] TMP_Text popupText;
	[SerializeField] Image[] images;
	[SerializeField] float popupFadeTime;
    [SerializeField] int linesInPrefab;
    [SerializeField] int linesRequiredToHover;
    [SerializeField] Vector2[] popupDirectionOffsets;
    [SerializeField] GameObject[] pointers;

	private float lastLineCount;
	private Coroutine lastScaleCoroutine;
	private string savedText;
	private float savedLineCount;

	private int direction;
	private float popupBodyHeight;
	private float popupBodyStartingY;
	private float popupTopStartingY;
	private float popupBottomStartingY;
	private float popupTextStartingY;
	private float popupBodyStartingYScale;
	

	private void Awake()
	{
		popupBodyHeight = popupBody.rect.height;
		popupBodyStartingYScale = popupBody.localScale.y;
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
	public void OnPointerEnter(PointerEventData eventData)
    {
		popupText.ForceMeshUpdate();
		if (lastScaleCoroutine != null)
		{
			StopCoroutine(lastScaleCoroutine);
		}
		lastScaleCoroutine = StartCoroutine(ScaleImageTo(savedLineCount, popupFadeTime));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		popupText.ForceMeshUpdate();
		if (lastScaleCoroutine != null)
		{
			StopCoroutine(lastScaleCoroutine);
		}
		lastScaleCoroutine = StartCoroutine(ScaleImageTo(Mathf.Min(savedLineCount, linesRequiredToHover), popupFadeTime));
    }

    //Called when popup is created by PopupCreator script
    public void SetText(string newText)
	{
		savedText = newText;
		
		popupText.text = savedText;
		popupText.ForceMeshUpdate();

        int lineCount = popupText.textInfo.lineCount;
		savedLineCount = lineCount;
		UpdateScale(Mathf.Min(lineCount, linesRequiredToHover));
	}

	public void FadeOutAndDestroy()
	{
		Destroy(transform.parent.gameObject, popupFadeTime);
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

	public void UpdateScale(float lineCount) //float to allow interpolation 
	{
		lastLineCount = lineCount;
		popupBody.localScale = new Vector3(popupBody.localScale.x, (popupBodyStartingYScale / (linesInPrefab - 1)) * (lineCount - 1), popupBody.localScale.z);
		float changeInHeight = popupBodyHeight * (popupBody.localScale.y - popupBodyStartingYScale);

	
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

		popupText.text = savedText;
		popupText.ForceMeshUpdate();
		if (popupText.textInfo.lineCount > Mathf.Floor(lineCount))
		{
			string shortText = "";
			for (int i = 0; i < Mathf.Floor(lineCount); i++)
			{
				shortText += popupText.text.Substring(popupText.textInfo.lineInfo[i].firstCharacterIndex, popupText.textInfo.lineInfo[i].characterCount);
			}
			shortText = shortText.Substring(0, shortText.Length - 3) + "..."; 
			popupText.text = shortText;
		}
		popupText.ForceMeshUpdate();
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

	private IEnumerator ScaleImageTo(float finalLineCount, float scaleTime)
	{
		float originalLineCount = lastLineCount;
        float time = 0f;
        while(time < scaleTime)
		{
			UpdateScale(Mathf.SmoothStep(originalLineCount, finalLineCount, time/scaleTime));
            time += Time.deltaTime;
            yield return null;
        }

		UpdateScale(finalLineCount);
	}

	
}
