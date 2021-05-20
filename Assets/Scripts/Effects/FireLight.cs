using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
	public AnimationCurve lightCurve;
	public float fireSpeed = 1f;

	private Light _lightComp;
	private float _initialIntensity;

	private Vector3 _pos;

	public float magnitude = 1f;

	private void Awake()
	{
		_lightComp = GetComponent<Light>();
		_initialIntensity = _lightComp.intensity;
		_pos = transform.localPosition;

	}

	void Update()
	{
		_lightComp.intensity = _initialIntensity * lightCurve.Evaluate(Time.time * fireSpeed) * magnitude;

		transform.localPosition = _pos + transform.up * Mathf.Sin(lightCurve.Evaluate(Time.time * fireSpeed)) * magnitude;
	}
}
