using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGridDisplay : MonoBehaviour {

	private Image[] images;

	private void Awake() {
		images = GetComponentsInChildren<Image>();
	}

	public void Set(Vector2Int coords, Color color) {
		if (gameObject.activeSelf && images != null) {
			images[coords.y * 32 + coords.x].color = color;
		}
	}

}
