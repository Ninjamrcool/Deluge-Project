using UnityEngine;

public class CityCreator : MonoBehaviour
{

    [System.Serializable]
    public struct City
	{
		public string cityName;
        public Vector2 position;
	} 

    [SerializeField] private City[] cityList; 
    [SerializeField] private GameObject cityPrefab; 

    private void Start()
    {
        for (int i = 0; i < cityList.Length; i++)
		{
			GameObject newCity = Instantiate(cityPrefab, transform);
            newCity.GetComponent<RectTransform>().anchoredPosition = cityList[i].position;
            newCity.GetComponent<Cities>().SetName(cityList[i].cityName);
		}
    }
}
