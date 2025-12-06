using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Timeline : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private float[] validTimelineYears;
    [SerializeField] private float startingYear;
    [SerializeField] private float endingYear;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private GameObject timelineValidYearPrefab;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private RectTransform timelineYearsFolder;

    private bool dragging = false;    
    private InputAction mousePosition;
    private InputAction mouseDown;

	private void Start()
	{
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
			}

			yield return null;
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
