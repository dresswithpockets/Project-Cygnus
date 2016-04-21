/*

UltimateOrbitCamera is a 3rd party component used for Camera game objects that rotate around a target.
This class provides significant functionality for orbital cameras.

*/

using UnityEngine;
using System.Collections;

public sealed class UltimateOrbitCamera : MonoBehaviour
{

	public Transform target;

	public LayerMask layerMask;

	public float distance = 10f;
	public float maxDistance = 25f;
	public float minDistance = 5f;

	public bool mouseControl = true;
	public string mouseAxisX = "Mouse X";
	public string mouseAxisY = "Mouse Y";
	public string mouseAxisZoom = "Mouse ScrollWheel";

	public bool keyboardControl = false;
	public string kbPanAxisX = "Horizontal";
	public string kbPanAxisY = "Vertical";
	public bool kbUseZoomAxis = false;
	public KeyCode zoomInKey = KeyCode.R;
	public KeyCode zoomOutKey = KeyCode.F;
	public string kbZoomAxisName = "";

	public float initialAngleX = 0f;
	public float initialAngleY = 0f;
	public bool invertAxisX = false;
	public bool invertAxisY = false;
	public bool invertAxisZoom = false;
	public float xSpeed = 1f;
	public float ySpeed = 1f;
	public float zoomSpeed = 5f;
	public float dampeningX = 0.9f;
	public float dampeningY = 0.9f;
	public float smoothingZoom = 0.1f;

	public bool limitY = true;
	public float yMinLimit = -60f;
	public float yMaxLimit = 60f;
	public float yLimitOffset = 0f;

	public bool limitX = false;
	public float xMinLimit = -60f;
	public float xMaxLimit = 60f;
	public float xLimitOffset = 0f;

	public bool clickToRotate = true;
	public bool leftClickToRotate = true;
	public bool rightClickToRotate = false;

	public bool autoRotateOn = false;
	public bool autoRotateReverse = false;
	public float autoRotateSpeed = 0.1f;

	public bool SpinEnabled = false;
	public bool spinUseAxis = false;
	public KeyCode spinKey = KeyCode.LeftControl;
	public string spinAxis = "";
	public float maxSpinSpeed = 3f;
	private bool spinning = false;
	private float spinSpeed = 0f;

	public bool cameraCollision = false;
	public float collisionRadius = 0.25f;

	private float xVelocity = 0f;
	private float yVelocity = 0f;
	private float zoomVelocity = 0f;

	private float targetDistance = 10f;
	private float x = 0f;
	private float y = 0f;
	private Vector3 position;

	[HideInInspector]
	public int invertXValue = 1;
	[HideInInspector]
	public int invertYValue = 1;
	[HideInInspector]
	public int invertZoomValue = 1;
	[HideInInspector]
	public int autoRotateReverseValue = 1;

	private Ray ray;
	private RaycastHit hit;

	private Transform _transform;

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
	private float pinchDist = 0f;
#endif

	void Start()
	{
		targetDistance = distance;
		if (invertAxisX)
		{
			invertXValue = -1;
		}
		else
		{
			invertXValue = 1;
		}
		if (invertAxisY)
		{
			invertYValue = -1;
		}
		else
		{
			invertYValue = 1;
		}
		if (invertAxisZoom)
		{
			invertZoomValue = -1;
		}
		else
		{
			invertZoomValue = 1;
		}
		if (autoRotateOn)
		{
			autoRotateReverseValue = -1;
		}
		else
		{
			autoRotateReverseValue = 1;
		}

		_transform = transform;

		if (GetComponent<Rigidbody>() != null)
			GetComponent<Rigidbody>().freezeRotation = true;

		x = initialAngleX;
		y = initialAngleY;
		_transform.Rotate(new Vector3(0f, initialAngleX, 0f), Space.World);
		_transform.Rotate(new Vector3(initialAngleY, 0f, 0f), Space.Self);

		position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
	}

	void Update()
	{
		if (target != null)
		{
			#region Input
			if (autoRotateOn)
			{
				xVelocity += autoRotateSpeed * autoRotateReverseValue * Time.deltaTime;
			}
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
			if(Input.touches.Length == 1)
			{
				xVelocity += Input.GetTouch(0).deltaPosition.x * xSpeed * invertXValue * 0.2f;
				yVelocity -= Input.GetTouch(0).deltaPosition.y * ySpeed * invertYValue * 0.2f;
			}
#else
			if (mouseControl && !Player_Controller.instance.game_paused)
			{
				if (!clickToRotate || ((leftClickToRotate && Input.GetMouseButton(0)) || (rightClickToRotate && Input.GetMouseButton(1))))
				{
					xVelocity += Input.GetAxis(mouseAxisX) * xSpeed * invertXValue;
					yVelocity -= Input.GetAxis(mouseAxisY) * ySpeed * invertYValue;

					spinning = false;
				}
				zoomVelocity -= Input.GetAxis(mouseAxisZoom) * zoomSpeed * invertZoomValue;
			}
			if (keyboardControl)
			{
				if (Input.GetAxis(kbPanAxisX) != 0 || Input.GetAxis(kbPanAxisY) != 0)
				{
					xVelocity -= Input.GetAxisRaw(kbPanAxisX) * (xSpeed / 2) * invertXValue;
					yVelocity += Input.GetAxisRaw(kbPanAxisY) * (ySpeed / 2) * invertYValue;
					spinning = false;
				}
				if (kbUseZoomAxis)
				{
					zoomVelocity += Input.GetAxis(kbZoomAxisName) * (zoomSpeed / 10) * invertXValue;
				}
				{
					if (Input.GetKey(zoomInKey))
						zoomVelocity -= (zoomSpeed / 10) * invertZoomValue;
					if (Input.GetKey(zoomOutKey))
						zoomVelocity += (zoomSpeed / 10) * invertZoomValue;
				}
			}
#endif
			if (SpinEnabled && ((mouseControl && clickToRotate) || keyboardControl))
			{
				if ((spinUseAxis && Input.GetAxis(spinAxis) != 0) || !spinUseAxis && Input.GetKey(spinKey))
				{
					spinning = true;
					spinSpeed = Mathf.Min(xVelocity, maxSpinSpeed);
				}
				if (spinning)
				{
					xVelocity = spinSpeed;
				}
			}
			#endregion

			#region Apply_Rotation_And_Position
			
			if (limitX)
			{
				if (x + xVelocity < xMinLimit + xLimitOffset)
				{
					xVelocity = (xMinLimit + xLimitOffset) - x;
				}
				else if (x + xVelocity > xMaxLimit + xLimitOffset)
				{
					xVelocity = (xMaxLimit + xLimitOffset) - x;
				}
				x += xVelocity;
				_transform.Rotate(new Vector3(0f, xVelocity, 0f), Space.World);
			}
			else
			{
				_transform.Rotate(new Vector3(0f, xVelocity, 0f), Space.World);
			}
			if (limitY)
			{
				if (y + yVelocity < yMinLimit + yLimitOffset)
				{
					yVelocity = (yMinLimit + yLimitOffset) - y;
				}
				else if (y + yVelocity > yMaxLimit + yLimitOffset)
				{
					yVelocity = (yMaxLimit + yLimitOffset) - y;
				}
				y += yVelocity;
				_transform.Rotate(new Vector3(yVelocity, 0f, 0f), Space.Self);
			}
			else
			{
				_transform.Rotate(new Vector3(yVelocity, 0f, 0f), Space.Self);
			}
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
			if (Input.touchCount == 2)
			{
				if (pinchDist == 0f)
				{
					pinchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
				}
				else
				{
					targetDistance += ((pinchDist - (float)Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position)) * 0.01f) * zoomSpeed * invertZoomValue;
					pinchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position); 
				}
				if (targetDistance < minDistance)
				{
					targetDistance = minDistance;	
				}
				else if (targetDistance > maxDistance)
				{
					targetDistance = maxDistance;	
				}
			}
			else
			{
				pinchDist = 0f;
			}
#endif
			if (targetDistance + zoomVelocity < minDistance)
			{
				zoomVelocity = minDistance - targetDistance;
			}
			else if (targetDistance + zoomVelocity > maxDistance)
			{
				zoomVelocity = maxDistance - targetDistance;
			}
			targetDistance += zoomVelocity;
			distance = Mathf.Lerp(distance, targetDistance, smoothingZoom);

			if (cameraCollision)
			{
				ray = new Ray(target.position, (_transform.position - target.position).normalized);
				if (Physics.SphereCast(ray.origin, collisionRadius, ray.direction, out hit, distance, layerMask))
				{
					distance = hit.distance;
				}
			}
			#endregion

			position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
			_transform.position = position;
			if (!SpinEnabled || !spinning)
				xVelocity *= dampeningX;
			yVelocity *= dampeningY;
			zoomVelocity = 0;
		}
		else
		{
			Debug.LogWarning("Orbit Cam - No Target Given");
		}
	}
}
