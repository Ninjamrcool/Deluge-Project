using UnityEngine;
using TMPro;
public class Cities : MonoBehaviour
{
    [SerializeField] private float maxScale = 1.425f;
    [SerializeField] float baseOrthographicSize;
    [SerializeField] TMP_Text cityText;
    private Camera mainCamera;

    private void Start()
	{
		mainCamera = Camera.main;

        mainCamera.GetComponent<CameraControls>().cameraZoomed += ScaleCity;

        RectTransform textRectTransform = cityText.gameObject.GetComponent<RectTransform>();
        if (Random.Range(0, 2) == 0)
		{
			textRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, textRectTransform.anchoredPosition.y * -1);
		}
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
}
