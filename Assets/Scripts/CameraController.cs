using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private PlayerController playerController;
	[SerializeField] private float smoothTime = 1;

	private Vector3 velocity;

	private void Update() {
		transform.position = Vector3.SmoothDamp(transform.position, playerController.transform.position, ref velocity, smoothTime);
	}

}
