using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField] private Camera mainCamera;
	[SerializeField] private Canvas canvas;
	[SerializeField] private UIProgressBar progressBarPrefab;

	private Transform progressBarParent;


	public UIProgressBar CreateProgressBar(Vector3 worldPos) {
		if (progressBarParent == null) {
			progressBarParent = new GameObject("Progress Bars").transform;
			progressBarParent.SetParent(canvas.transform);
		}
		UIProgressBar progressBar = Instantiate(progressBarPrefab, progressBarParent);
		(progressBar.transform as RectTransform).anchoredPosition = GetCanvasPosition(worldPos);
		return progressBar;
	}

	private Vector2 GetCanvasPosition(Vector3 worldPos) {
		Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPos);
		return transform.InverseTransformPoint(screenPos);
	}

}
