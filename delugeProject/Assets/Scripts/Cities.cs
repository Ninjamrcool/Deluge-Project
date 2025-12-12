using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class Cities : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float maxScale = 1.425f;
    [SerializeField] float baseOrthographicSize;
	[SerializeField] float moveDownSubtext = -71.3f;
	[SerializeField] float subtextFadeTime = 1f;
    [SerializeField] TMP_Text cityText;
    [SerializeField] TMP_Text citySubText;
    private Camera mainCamera;

    private void Start()
	{
		mainCamera = Camera.main;

        mainCamera.GetComponent<CameraControls>().cameraZoomed += ScaleCity;

        RectTransform textRectTransform = cityText.gameObject.GetComponent<RectTransform>();
		RectTransform subTextRectTransform = citySubText.gameObject.GetComponent<RectTransform>();
        if (Random.Range(0, 2) == 0)
		{
			textRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, textRectTransform.anchoredPosition.y * -1);
			subTextRectTransform.anchoredPosition = new Vector2(subTextRectTransform.anchoredPosition.x, subTextRectTransform.anchoredPosition.y + moveDownSubtext);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
		StopAllCoroutines();
		StartCoroutine(FadeSubtextTo(1f, subtextFadeTime));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		StopAllCoroutines();
		StartCoroutine(FadeSubtextTo(0f, subtextFadeTime));
    }

	private void ScaleCity()
	{
		transform.localScale = Vector3.one * Mathf.Min(maxScale, mainCamera.orthographicSize / baseOrthographicSize);
	}

    //Called when city is created by CityCreator script
    public void SetName(string newName)
	{
		cityText.text = newName;
	}

	//Called when city is created by CityCreator script
    public void SetSubName(string newSubName)
	{
		if (newSubName == null || newSubName.Length == 0)
		{
			citySubText.text = "";
		}
		else
		{
			citySubText.text = "(" + newSubName + ")";
		}
	}

	private IEnumerator FadeSubtextTo(float alpha, float fadeTime)
	{
        float time = 0f;
        float originalAlpha = citySubText.color.a;
        while(time < fadeTime)
		{
			citySubText.color = new Color(citySubText.color.r, citySubText.color.g, citySubText.color.b, Mathf.SmoothStep(originalAlpha, alpha, time/fadeTime));
            time += Time.deltaTime;
            yield return null;
        }

		citySubText.color = new Color(citySubText.color.r, citySubText.color.g, citySubText.color.b, Mathf.SmoothStep(originalAlpha, alpha, 1));
	}
}
