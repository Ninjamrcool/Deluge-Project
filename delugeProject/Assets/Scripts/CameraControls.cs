using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private Vector2 topRightCorner;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private float dragModifier = 0.7f;
    [SerializeField] private float scrollModifier = 0.7f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 9f;

    private Vector2 lastMousePosition;
    private InputAction mousePosition;
    private InputAction mouseDown;
    private InputAction mouseScroll;
    private Camera cameraComponent;

	private void Start()
	{
		mousePosition = InputSystem.actions.FindAction("MousePosition");
		mouseDown = InputSystem.actions.FindAction("MouseDown");
		mouseScroll = InputSystem.actions.FindAction("MouseScroll");
		cameraComponent = gameObject.GetComponent<Camera>();
	}

	private void Update()
    {
        //Camera Dragging
        if (mouseDown.WasPressedThisFrame())
		{
			lastMousePosition = mousePosition.ReadValue<Vector2>();
		}
        else if (mouseDown.IsPressed() && lastMousePosition != null)
		{
            Vector2 newMousePosition = mousePosition.ReadValue<Vector2>();
            transform.position += (Vector3)(lastMousePosition - newMousePosition) * dragModifier;
            lastMousePosition = newMousePosition;
		}

        //Camera Zooming
        if (mouseScroll.ReadValue<Vector2>() != Vector2.zero)
		{
			cameraComponent.orthographicSize -= mouseScroll.ReadValue<Vector2>().y * scrollModifier;
            cameraComponent.orthographicSize = Mathf.Max(minZoom, Mathf.Min(maxZoom, cameraComponent.orthographicSize));
		}

        //Camera bounds
        Vector3 cameraBottomLeftCorner = cameraComponent.ViewportToWorldPoint(new Vector3(0, 0, cameraComponent.nearClipPlane));
        Vector3 cameraTopRightCorner = cameraComponent.ViewportToWorldPoint(new Vector3(1, 1, cameraComponent.nearClipPlane));

        if (cameraBottomLeftCorner.x < bottomLeftCorner.x)
		{
			transform.position = new Vector3(transform.position.x + bottomLeftCorner.x - cameraBottomLeftCorner.x, transform.position.y, transform.position.z);
		}
        else if (cameraTopRightCorner.x > topRightCorner.x)
		{
			transform.position = new Vector3(transform.position.x + topRightCorner.x - cameraTopRightCorner.x, transform.position.y, transform.position.z);
		}

        if (cameraBottomLeftCorner.y < bottomLeftCorner.y)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + bottomLeftCorner.y - cameraBottomLeftCorner.y, transform.position.z);
		}
        else if (cameraTopRightCorner.y > topRightCorner.y)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + topRightCorner.y - cameraTopRightCorner.y, transform.position.z);
		}
    }
}
