using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UltimateOrbitCamera))]
public class UltimateOrbitCameraEditor : Editor 
{
	private SerializedObject m_object;
	private UltimateOrbitCamera myTarget;

	private bool showMouseOptions;
	private bool showKeyboardOptions;
	private bool showGenericOptions = true;
	private bool showAutoRotateOptions;
	private bool showSpinOptions;
	private bool showCollisionOptions;

	public void OnEnable()
	{
		myTarget = (UltimateOrbitCamera)target;
		m_object = new SerializedObject(target);
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.Separator();

		myTarget.target = EditorGUILayout.ObjectField("Target", myTarget.target, typeof(Transform)) as Transform;
		myTarget.layerMask = EditorGUILayout.MaskField("Collision Mask", myTarget.layerMask, UnityEditorInternal.InternalEditorUtility.layers /*new string[] { "Everything", "Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Player", "PlayerCamera" }*/);
		m_object.Update();
		EditorGUILayout.Separator();
		
		showGenericOptions = EditorGUILayout.Foldout(showGenericOptions, "Orbit Camera Options");
		if (showGenericOptions)
		{
			myTarget.initialAngleX = EditorGUILayout.FloatField("Initial Angle X", myTarget.initialAngleX);
			myTarget.initialAngleY = EditorGUILayout.FloatField("Initial Angle Y", myTarget.initialAngleY);
			
			myTarget.xSpeed = EditorGUILayout.FloatField("X Speed", myTarget.xSpeed);
			myTarget.ySpeed = EditorGUILayout.FloatField("Y Speed", myTarget.ySpeed);
			myTarget.zoomSpeed = EditorGUILayout.FloatField("Zoom Speed", myTarget.zoomSpeed);
			
			myTarget.dampeningX = EditorGUILayout.Slider("Dampening X",myTarget.dampeningX,0.01f,0.99f);
			myTarget.dampeningY = EditorGUILayout.Slider("Dampening Y", myTarget.dampeningY,0.01f,0.99f);
			myTarget.smoothingZoom = EditorGUILayout.Slider("Zoom Smoothing", myTarget.smoothingZoom,0.01f,0.99f);
			
			myTarget.distance = EditorGUILayout.FloatField("Start Distance", myTarget.distance);
			myTarget.minDistance = Mathf.Min(myTarget.maxDistance,EditorGUILayout.FloatField("Min Distance", myTarget.minDistance));
			myTarget.maxDistance = Mathf.Max(myTarget.minDistance,EditorGUILayout.FloatField("Max Distance", myTarget.maxDistance));
			
			myTarget.limitX = EditorGUILayout.Toggle("Limit X",myTarget.limitX);
			if(!myTarget.limitX)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.xMinLimit = Mathf.Min(myTarget.xMaxLimit,EditorGUILayout.FloatField("X Min", myTarget.xMinLimit));
			myTarget.xMaxLimit = Mathf.Max(myTarget.xMinLimit,EditorGUILayout.FloatField("X Max", myTarget.xMaxLimit));
			myTarget.xLimitOffset = EditorGUILayout.FloatField("X Limit Offset", myTarget.xLimitOffset);
			GUI.color = new Color(1f,1f,1f);

			myTarget.limitY = EditorGUILayout.Toggle("Limit Y",myTarget.limitY);
			if(!myTarget.limitY)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.yMinLimit = Mathf.Min(myTarget.yMaxLimit,EditorGUILayout.FloatField("Y Min", myTarget.yMinLimit));
			myTarget.yMaxLimit = Mathf.Max(myTarget.yMinLimit,EditorGUILayout.FloatField("Y Max", myTarget.yMaxLimit));
			myTarget.yLimitOffset = EditorGUILayout.FloatField("Y Limit Offset", myTarget.yLimitOffset);
			GUI.color = new Color(1f,1f,1f);
			myTarget.invertAxisX = EditorGUILayout.Toggle("Invert Axis X",myTarget.invertAxisX);
			myTarget.invertAxisY = EditorGUILayout.Toggle("Invert Axis Y",myTarget.invertAxisY);
			myTarget.invertAxisZoom = EditorGUILayout.Toggle("Invert Zoom",myTarget.invertAxisZoom);
		}

		GUI.color = new Color(1f,1f,1f);
		EditorGUILayout.Separator();

		showMouseOptions = EditorGUILayout.Foldout(showMouseOptions, "Mouse Input Options");
		if (showMouseOptions)
		{
			myTarget.mouseControl = EditorGUILayout.Toggle("Mouse Control",myTarget.mouseControl);

			if(!myTarget.mouseControl)
				GUI.color = new Color(0.5f,0.5f,0.5f);

			myTarget.clickToRotate = EditorGUILayout.Toggle("Click To Rotate",myTarget.clickToRotate);

			if(!myTarget.clickToRotate)
				GUI.color = new Color(0.5f,0.5f,0.5f);

			myTarget.leftClickToRotate = EditorGUILayout.Toggle("Left Click",myTarget.leftClickToRotate);
			myTarget.rightClickToRotate = EditorGUILayout.Toggle("Right Click",myTarget.rightClickToRotate);

			if(myTarget.mouseControl)
				GUI.color = new Color(1f,1f,1f);

			myTarget.mouseAxisX = EditorGUILayout.TextField("Axis X Name",myTarget.mouseAxisX);
			myTarget.mouseAxisY = EditorGUILayout.TextField("Axis Y Name",myTarget.mouseAxisY);
			myTarget.mouseAxisZoom = EditorGUILayout.TextField("Zoom Axis Name",myTarget.mouseAxisZoom);
		}
		GUI.color = new Color(1f,1f,1f);

		EditorGUILayout.Separator();

		showKeyboardOptions = EditorGUILayout.Foldout(showKeyboardOptions, "Keyboard Input Options");
		if (showKeyboardOptions)
		{
			myTarget.keyboardControl = EditorGUILayout.Toggle("Keyboard Control",myTarget.keyboardControl);
			if(!myTarget.keyboardControl)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.kbPanAxisX = EditorGUILayout.TextField("Axis X Name",myTarget.kbPanAxisX);
			myTarget.kbPanAxisY = EditorGUILayout.TextField("Axis Y Name",myTarget.kbPanAxisY);
			myTarget.kbUseZoomAxis = EditorGUILayout.Toggle("Use Zoom Input Axis?",myTarget.kbUseZoomAxis);
			if(myTarget.kbUseZoomAxis)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			EditorGUILayout.PropertyField(m_object.FindProperty("zoomInKey"),true);
			EditorGUILayout.PropertyField(m_object.FindProperty("zoomOutKey"),true);
			if(myTarget.kbUseZoomAxis && myTarget.keyboardControl){
				GUI.color = new Color(1f,1f,1f);
			}
			else{
				GUI.color = new Color(0.5f,0.5f,0.5f);
			}
			myTarget.kbZoomAxisName = EditorGUILayout.TextField("Zoom Axis Name",myTarget.kbZoomAxisName);
		}
		GUI.color = new Color(1f,1f,1f);

		EditorGUILayout.Separator();

		showAutoRotateOptions = EditorGUILayout.Foldout(showAutoRotateOptions, "Auto Rotate Options");
		if (showAutoRotateOptions)
		{
			myTarget.autoRotateOn = EditorGUILayout.Toggle("Auto Rotate",myTarget.autoRotateOn);
			if(!myTarget.autoRotateOn)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.autoRotateReverse = EditorGUILayout.Toggle("Reverse",myTarget.autoRotateReverse);
			myTarget.autoRotateSpeed = EditorGUILayout.FloatField("Speed", myTarget.autoRotateSpeed);
		}
		GUI.color = new Color(1f,1f,1f);

		EditorGUILayout.Separator();

		showSpinOptions = EditorGUILayout.Foldout(showSpinOptions, "Spin Options");
		if (showSpinOptions)
		{
			myTarget.SpinEnabled = EditorGUILayout.Toggle("Spin Enabled",myTarget.SpinEnabled);
			if(!myTarget.SpinEnabled)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.maxSpinSpeed = EditorGUILayout.FloatField("Max Speed", myTarget.maxSpinSpeed);
			myTarget.spinUseAxis = EditorGUILayout.Toggle("Use Button Input Axis?",myTarget.spinUseAxis);
			if(myTarget.spinUseAxis)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			EditorGUILayout.PropertyField(m_object.FindProperty("spinKey"),true);
			if(myTarget.spinUseAxis && myTarget.SpinEnabled){
				GUI.color = new Color(1f,1f,1f);
			}
			else{
				GUI.color = new Color(0.5f,0.5f,0.5f);
			}
			myTarget.spinAxis = EditorGUILayout.TextField("Button Axis Name",myTarget.spinAxis);

		}
		GUI.color = new Color(1f,1f,1f);

		EditorGUILayout.Separator();

		showCollisionOptions = EditorGUILayout.Foldout(showCollisionOptions, "Collision Options");
		if (showCollisionOptions)
		{
			myTarget.cameraCollision = EditorGUILayout.Toggle("Collision Enabled",myTarget.cameraCollision);
			if(!myTarget.cameraCollision)
				GUI.color = new Color(0.5f,0.5f,0.5f);
			myTarget.collisionRadius = EditorGUILayout.FloatField("Collision Radius", myTarget.collisionRadius);
		}
		GUI.color = new Color(1f,1f,1f);
		EditorGUILayout.Separator();
		if(myTarget.invertAxisX)
		{
			myTarget.invertXValue = -1;
		}
		else
		{
			myTarget.invertXValue = 1;
		}
		if(myTarget.invertAxisY)
		{
			myTarget.invertYValue = -1;
		}
		else
		{
			myTarget.invertYValue = 1;
		}
		if(myTarget.invertAxisZoom)
		{
			myTarget.invertZoomValue = -1;
		}
		else
		{
			myTarget.invertZoomValue = 1;
		}
		if(myTarget.autoRotateReverse)
		{
			myTarget.autoRotateReverseValue = -1;
		}
		else
		{
			myTarget.autoRotateReverseValue = 1;
		}
	}
}
