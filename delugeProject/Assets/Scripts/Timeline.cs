using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Timeline : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private float[] validTimelineYears;
    [SerializeField] private float startingYear;
    [SerializeField] private float endingYear;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private GameObject timelineValidYearPrefab;
    [SerializeField] private Image prussiaBorders;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private RectTransform timelineYearsFolder;

    private float currentYear;    
    private bool dragging = false;    
    private InputAction mousePosition;
    private InputAction mouseDown;

	private void Start()
	{
        currentYear = validTimelineYears[0];
        mousePosition = InputSystem.actions.FindAction("MousePosition");
		mouseDown = InputSystem.actions.FindAction("MouseDown");

        for (int i = 0; i < validTimelineYears.Length; i++)
		{
            float instantiateXPos = ConvertValidTimelineYearsIndexToPosition(i);
            GameObject timelinevalidYear = Instantiate(timelineValidYearPrefab, timelineYearsFolder);
			timelinevalidYear.GetComponent<RectTransform>().anchoredPosition = new Vector2(instantiateXPos, rectTransform.anchoredPosition.y);
		}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        StartCoroutine(DragTimelineButton());
    }

    private IEnumerator DragTimelineButton()
	{
		while (dragging)
		{
            if (mouseDown.IsPressed())
            {
                Vector2 newPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, mousePosition.ReadValue<Vector2>(), Camera.main, out newPos);
                rectTransform.anchoredPosition = new Vector2(newPos.x, rectTransform.anchoredPosition.y);
            }
			else
			{
				dragging = false;

                int nearestTimelineIndex = 0;
                float nearestTimelineDistance = float.MaxValue;
                for (int i = 0; i < validTimelineYears.Length; i++)
                {
                    float XPos = ConvertValidTimelineYearsIndexToPosition(i);
                    if (Mathf.Abs(XPos - rectTransform.anchoredPosition.x) < nearestTimelineDistance)
					{
						nearestTimelineDistance = Mathf.Abs(XPos - rectTransform.anchoredPosition.x);
                        nearestTimelineIndex = i;
					}
                }
                rectTransform.anchoredPosition = new Vector2(ConvertValidTimelineYearsIndexToPosition(nearestTimelineIndex), rectTransform.anchoredPosition.y);
                currentYear = validTimelineYears[nearestTimelineIndex];
                UpdateTimelineYear(currentYear);
			}

			yield return null;
		}
	}

    private void UpdateTimelineYear(float year)
	{
        if (year > 1656f)
		{
			prussiaBorders.color = new Color(prussiaBorders.color.r, prussiaBorders.color.g, prussiaBorders.color.b, 1f);
		}
		else
		{
			prussiaBorders.color = new Color(prussiaBorders.color.r, prussiaBorders.color.g, prussiaBorders.color.b, 0.6941176471f);
		}
	}

    private float ConvertValidTimelineYearsIndexToPosition(int index)
	{
        //Desmos approved
		return ((maxX - minX)/(endingYear - startingYear) * (validTimelineYears[index] - startingYear)) + minX;
	}

    public bool GetIsDragging()
	{
		return dragging;
	}

}
