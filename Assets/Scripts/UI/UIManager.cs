using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField] private Camera mainCamera;
	[SerializeField] private Canvas canvas;
	[SerializeField] private UIProgressBar progressBarPrefab;

	private Transform progressBarParent;


	public UIProgressBar CreateProgressBar(Transform target, Vector3 targetOffset) {
		if (progressBarParent == null) {
			progressBarParent = new GameObject("Progress Bars").transform;
			progressBarParent.SetParent(canvas.transform);
		}
		UIProgressBar progressBar = Instantiate(progressBarPrefab, progressBarParent);
		progressBar.Init(this, target, targetOffset);
		return progressBar;
	}

	public Vector2 GetCanvasPosition(Vector3 worldPos) {
		Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPos);
		return transform.InverseTransformPoint(screenPos);
	}
	
	public Vector3 GetWorldPosFromCanvasPos(Vector2 canvasPos) {
		if (canvasPos != null) {
			Vector3 pos = mainCamera.ViewportToWorldPoint(canvasPos);
			return mainCamera.WorldToViewportPoint(pos);
		} else
			return Vector3.zero;
	}

}
