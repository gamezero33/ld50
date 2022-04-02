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
	
	private void UpdateProgress() {
		if (fillImage)
			fillImage.fillAmount = progress;
	}

}