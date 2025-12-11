using UnityEngine;
using UnityEngine.U2D;
using System.Collections;

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
    [SerializeField] private SpriteShapeController mainSpriteShape;
    [SerializeField] private SpriteShapeRenderer mainSpriteRenderer;
    [SerializeField] private float morphTime;
    [SerializeField] private float maxAlpha;
    [SerializeField] private int removeDetailPointsLimit = 75;

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
                StopAllCoroutines();
                StartCoroutine(MorphMainSpriteShape(overlayYears[i].spriteShapeGameObject.GetComponent<SpriteShapeController>().spline));
                StartCoroutine(FadeRendererTo(maxAlpha));
                return;
			}
		}
        StartCoroutine(FadeRendererTo(0f));
    }

    private IEnumerator MorphMainSpriteShape(Spline morphInto)
	{
        Spline morphIntoCopy = new Spline();
        CopySplineData(morphIntoCopy, morphInto);

        //Remove some detail for complicated splines when moving
        if (mainSpriteShape.spline.GetPointCount() > removeDetailPointsLimit)
		{
			RemoveSplinePointsToTargetAmount(mainSpriteShape.spline, (int)(mainSpriteShape.spline.GetPointCount() * 0.75f));
		}
        if (morphIntoCopy.GetPointCount() > removeDetailPointsLimit)
		{
			RemoveSplinePointsToTargetAmount(morphIntoCopy, (int)(morphIntoCopy.GetPointCount() * 0.75f));
		}

        //make sure splines have same amount of points
        int morphIntoMultiplier;
        if(mainSpriteShape.spline.GetPointCount() < morphIntoCopy.GetPointCount() / 2)
		{
			AddSplinePointsToTargetAmount(mainSpriteShape.spline, morphIntoCopy.GetPointCount() / 2);
            morphIntoMultiplier = 2;
		}
		else
		{
            AddSplinePointsToTargetAmount(mainSpriteShape.spline, morphIntoCopy.GetPointCount());
            morphIntoMultiplier = 1;
		}
        AddSplinePointsToTargetAmount(morphIntoCopy, mainSpriteShape.spline.GetPointCount());

        Spline originalSpline = new Spline();
        CopySplineData(originalSpline, mainSpriteShape.spline);
        
        float time = 0f;
        while(time < morphTime)
		{
            for (int i = 0; i < mainSpriteShape.spline.GetPointCount(); i++)
			{
				Vector3 finalPosition = morphIntoCopy.GetPosition(morphIntoMultiplier * i);
                Vector3 originalPosition = originalSpline.GetPosition(i);

				try
				{
                    mainSpriteShape.spline.SetPosition(i, Vector3.Lerp(originalPosition, finalPosition, time/morphTime));
				}
				catch
				{
					//Points too close
                    mainSpriteShape.spline.RemovePointAt(i);
                    morphIntoCopy.RemovePointAt(morphIntoMultiplier * i); //removing this point might cause visual bugs, but itl fix after
                    i -= 1;
                    //Debug.Log("removed point: " + i);
				}
			}

            mainSpriteShape.RefreshSpriteShape();
			time += Time.deltaTime;
            yield return null;
		}


        CopySplineData(mainSpriteShape.spline, morphInto);
        mainSpriteShape.RefreshSpriteShape();
	}


    private void CopySplineData(Spline copyInto, Spline copyFrom){
        copyInto.Clear();

        for (int i = 0; i < copyFrom.GetPointCount(); i++)
		{
			copyInto.InsertPointAt(i, copyFrom.GetPosition(i));
            copyInto.SetTangentMode(i, ShapeTangentMode.Linear);
		}
    }

    private void AddSplinePointsToTargetAmount(Spline spline, int numPoints)
	{
		while (spline.GetPointCount() < numPoints)
		{
            try{
                int randomIndex = Random.Range(0, spline.GetPointCount() - 1);
                Vector3 midpoint = Vector3.Lerp(spline.GetPosition(randomIndex), spline.GetPosition(randomIndex + 1), 0.5f);
			    spline.InsertPointAt(randomIndex + 1, midpoint);
            }
			catch
			{
                //Points too close
                //Debug.Log("failed to add point");
				continue;
			}
		}
	}

    private void RemoveSplinePointsToTargetAmount(Spline spline, int numPoints)
	{
		while (spline.GetPointCount() > numPoints)
		{
            try{
                int randomIndex = Random.Range(0, spline.GetPointCount() - 1);
			    spline.RemovePointAt(randomIndex);
            }
			catch
			{
                //Points too close
                //Debug.Log("failed to add point");
				continue;
			}
		}
	}

    private IEnumerator FadeRendererTo(float alpha)
	{
        float time = 0f;
        float originalAlpha = mainSpriteRenderer.color.a;
        while(time < morphTime)
		{
            mainSpriteRenderer.color = new Color(mainSpriteRenderer.color.r, mainSpriteRenderer.color.g, mainSpriteRenderer.color.b, Mathf.SmoothStep(originalAlpha, alpha, time/morphTime));
            time += Time.deltaTime;
            yield return null;
        }

        mainSpriteRenderer.color = new Color(mainSpriteRenderer.color.r, mainSpriteRenderer.color.g, mainSpriteRenderer.color.b, Mathf.SmoothStep(originalAlpha, alpha, time/morphTime));
	}
}
