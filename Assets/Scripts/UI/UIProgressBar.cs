using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour {

	[SerializeField] private Image fillImage;
	[SerializeField, Range(0, 1)] private float progress;
	public float Progress {
		get { return progress; }
		set {
			if (value != progress) {
				progress = value;
				UpdateProgress();
			}
		}
	}

	private UIManager uiManager;
	private Transform target;
	private Vector3 targetOffset;

	public void Init(UIManager _uiManager, Transform _target, Vector3 _targetOffset) {
		uiManager = _uiManager;
		target = _target;
		targetOffset = _targetOffset;
		UpdateProgress();
	}

	private void Update() {
		if (uiManager) {
			UpdateProgress();
		}
	}

	private void UpdateProgress() {
		//(transform as RectTransform).anchoredPosition = uiManager.GetCanvasPosition(target.position + targetOffset);
		if (fillImage)
			fillImage.fillAmount = progress;
	}

}