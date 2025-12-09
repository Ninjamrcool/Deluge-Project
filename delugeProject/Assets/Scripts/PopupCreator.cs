using UnityEngine;

public class PopupCreator : MonoBehaviour
{
    [System.Serializable]
    public struct Popup
	{
        [TextArea]
		public string popupText;
        public Vector2 pointPosition;
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
			Destroy(transform.GetChild(i).gameObject);
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
            //if (Random.Range(0, 2) == 0)
			//{
			//	newPopupRect.localScale = new Vector3(-1 * newPopupRect.localScale.x, newPopupRect.localScale.y, newPopupRect.localScale.z);
			//}
            newPopup.GetComponent<Popups>().SetText(popupYear.popupsList[i].popupText);
		}	
	}

}
