using System;
using UnityEngine;

public class PopupCreator : MonoBehaviour
{

    public enum PopupDirection
	{
		North,
        South,
        East,
        West
	}

    [System.Serializable]
    public struct Popup
	{
        [TextArea]
		public string popupText;
        public Vector2 pointPosition;
        public PopupDirection pointDirection;
	} 

    [System.Serializable]
    public struct PopupYear
	{
		public float year;
        public Popup[] popupsList;
	} 

    [SerializeField] private PopupYear[] popupYearList; 
    [SerializeField] private GameObject popupPrefab; 
    [SerializeField] private Timeline timelineScript;

    private void Start()
    {
        timelineScript.timelineUpdated += TimelineUpdated;
    }

    private void TimelineUpdated(float year)
	{
        for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.GetComponent<Popups>().FadeOutAndDestroy();
		}

        for (int i = 0; i < popupYearList.Length; i++)
		{
			if (popupYearList[i].year == year)
			{
                CreatePopups(i);
                return;
			}
		}
	}

    private void CreatePopups(int popupYearListIndex)
	{
        PopupYear popupYear = popupYearList[popupYearListIndex];

        for (int i = 0; i < popupYear.popupsList.Length; i++)
		{
			GameObject newPopup = Instantiate(popupPrefab, transform);
            RectTransform newPopupRect = newPopup.GetComponent<RectTransform>();
            newPopupRect.anchoredPosition = popupYear.popupsList[i].pointPosition;
            newPopup.transform.GetChild(0).GetComponent<Popups>().SetDirection((int)popupYear.popupsList[i].pointDirection);
            newPopup.transform.GetChild(0).GetComponent<Popups>().SetText(popupYear.popupsList[i].popupText);
		}	
	}

}
