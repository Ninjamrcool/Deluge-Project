using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Timeline : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private float[] validTimelineYears;
    [SerializeField] private float startingYear;
    [SerializeField] private float endingYear;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private GameObject timelineValidYearPrefab;
    [SerializeField] private Image prussiaBorders;
    [SerializeField] private Image finalNationalBorders;
    [SerializeField] TMP_Text yearsText;

    [SerializeField] private GameObject timelineEndPage1;
    [SerializeField] private GameObject timelineEndPage2;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private RectTransform timelineYearsFolder;

    private float currentYear;    
    private bool dragging = false;    
    private InputAction mousePosition;
    private InputAction mouseDown;
    private InputAction forwardsTime;
    private InputAction backwardsTime;

    public delegate void TimelineUpdated(float year);
    public event TimelineUpdated timelineUpdated;

	private void Start()
	{
        currentYear = validTimelineYears[0];
        UpdateTimelineYear(currentYear);

        mousePosition = InputSystem.actions.FindAction("MousePosition");
		mouseDown = InputSystem.actions.FindAction("MouseDown");
		forwardsTime = InputSystem.actions.FindAction("ForwardsTime");
		backwardsTime = InputSystem.actions.FindAction("BackwardsTime");

        for (int i = 0; i < validTimelineYears.Length; i++)
		{
            float instantiateXPos = ConvertValidTimelineYearsIndexToPosition(i);
            GameObject timelinevalidYear = Instantiate(timelineValidYearPrefab, timelineYearsFolder);
			timelinevalidYear.GetComponent<RectTransform>().anchoredPosition = new Vector2(instantiateXPos, rectTransform.anchoredPosition.y);
		}
    }

    private void Update()
    {
        if (forwardsTime.WasPressedThisFrame() && !dragging)
        {
            for (int i = 0; i < validTimelineYears.Length; i++)
            {
                if (validTimelineYears[i] > currentYear)
                {
                    rectTransform.anchoredPosition = new Vector2(ConvertValidTimelineYearsIndexToPosition(i), rectTransform.anchoredPosition.y);
                    currentYear = validTimelineYears[i];
                    UpdateTimelineYear(currentYear);
                    return;
                }
            }            
        }
        else if (backwardsTime.WasPressedThisFrame() && !dragging)
        {
            for (int i = validTimelineYears.Length - 1; i >= 0; i--)
            {
                if (validTimelineYears[i] < currentYear)
                {
                    rectTransform.anchoredPosition = new Vector2(ConvertValidTimelineYearsIndexToPosition(i), rectTransform.anchoredPosition.y);
                    currentYear = validTimelineYears[i];
                    UpdateTimelineYear(currentYear);
                    return;
                }
            }            
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
                rectTransform.anchoredPosition = new Vector2(Mathf.Max(minX, Mathf.Min(newPos.x, maxX)), rectTransform.anchoredPosition.y);
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
        if (year >= 1668)
		{
			yearsText.text = " ";
		}
        else if (year - Mathf.Floor(year) < 0.5f)
		{
			yearsText.text = "Jan. " + Mathf.Floor(year).ToString();
		}
		else
		{
			yearsText.text = "June. " + Mathf.Floor(year).ToString();
		}
        

        if (year > 1656f)
		{
			prussiaBorders.color = new Color(prussiaBorders.color.r, prussiaBorders.color.g, prussiaBorders.color.b, 1f);
		}
		else
		{
			prussiaBorders.color = new Color(prussiaBorders.color.r, prussiaBorders.color.g, prussiaBorders.color.b, 0.6941176471f);
		}

        if (year >= 1667f)
		{
			finalNationalBorders.color = new Color(finalNationalBorders.color.r, finalNationalBorders.color.g, finalNationalBorders.color.b, 1f);
		}
		else
		{
			finalNationalBorders.color = new Color(finalNationalBorders.color.r, finalNationalBorders.color.g, finalNationalBorders.color.b, 0f);
		}

        if (year == 1668)
        {
            timelineEndPage1.SetActive(true);
        }
        else
        {
            timelineEndPage1.SetActive(false);
        }

        if (year == 1669)
        {
            timelineEndPage2.SetActive(true);
        }
        else
        {
            timelineEndPage2.SetActive(false);
        }

        if (timelineUpdated != null)
        {
            timelineUpdated(year);
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
