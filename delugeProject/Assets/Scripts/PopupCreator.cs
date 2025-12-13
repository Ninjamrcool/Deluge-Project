using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image popupsToggleButtonImage;
    [SerializeField] private Sprite popupsEnabledSprite;
    [SerializeField] private Sprite popupsDisabledSprite;

    private bool popupsEnabled = true;

    private void Start()
    {
        timelineScript.timelineUpdated += TimelineUpdated;
    }

    private void TimelineUpdated(float year)
	{
        for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetChild(0).gameObject.GetComponent<Popups>().FadeOutAndDestroy();
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
            if (!popupsEnabled)
            {
                newPopupRect.localScale = Vector3.zero;
            }
		}	
	}


    public void TogglePopups()
    {
        GameObject[] popups = GameObject.FindGameObjectsWithTag("Popup");
        popupsEnabled = !popupsEnabled;
        if (popupsEnabled)
        {
            popupsToggleButtonImage.sprite = popupsEnabledSprite;
        }
        else
        {
            popupsToggleButtonImage.sprite = popupsDisabledSprite;
        }

        for (int i = 0; i < popups.Length; i++)
        {
            if (popupsEnabled)
            {
                popups[i].GetComponent<RectTransform>().localScale = Vector3.one;
            }
            else
            {
                popups[i].GetComponent<RectTransform>().localScale = Vector3.zero;
            }
        }
    }

}
