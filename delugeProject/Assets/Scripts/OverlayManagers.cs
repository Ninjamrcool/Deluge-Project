using UnityEngine;
using UnityEngine.U2D;

public class OverlayManagers : MonoBehaviour
{

    [System.Serializable]
    public struct OverlayYear
	{
		public GameObject spriteShapeGameObject;
        public float year;
	}

    [SerializeField] private OverlayYear[] overlayYears;
    [SerializeField] private Timeline timelineScript;

    private void Start()
    {
        timelineScript.timelineUpdated += TimelineUpdated;
    }

    private void TimelineUpdated(float year)
    {
        for (int i = 0; i < overlayYears.Length; i++)
		{
			if (overlayYears[i].year <= year && (i == overlayYears.Length - 1 || overlayYears[i+1].year > year))
			{
				overlayYears[i].spriteShapeGameObject.GetComponent<SpriteShapeRenderer>().enabled = true;
			}
			else
			{
                overlayYears[i].spriteShapeGameObject.GetComponent<SpriteShapeRenderer>().enabled = false;
			}
		}
    }
}
