using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private Vector2 topRightCorner;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private float dragModifier = 0.7f;

    private Vector2 lastMousePosition;
    private InputAction mousePosition;
    private InputAction mouseDown;

	private void Start()
	{
		mousePosition = InputSystem.actions.FindAction("MousePosition");
		mouseDown = InputSystem.actions.FindAction("MouseDown");
	}

	private void Update()
    {
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

        transform.position = new Vector3(
            Mathf.Max(bottomLeftCorner.x, Mathf.Min(topRightCorner.x, transform.position.x)),
            Mathf.Max(bottomLeftCorner.y, Mathf.Min(topRightCorner.y, transform.position.y)),
            transform.position.z
        );
    }
}
