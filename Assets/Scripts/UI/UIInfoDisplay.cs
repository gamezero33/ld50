using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoDisplay : MonoBehaviour {

	[SerializeField] private Animator controller;
	[SerializeField] private Image iconImagePrefab;
	[SerializeField] private LayoutGroup layoutGroup;
	public LayoutGroup LayoutGroup => layoutGroup;
	[SerializeField] private UIProgressBar progressBar;
	public UIProgressBar ProgressBar => progressBar;
	

	private UIManager uiManager;
	private Furniture furniture;
	private bool showing = false;
	

	public void Init(UIManager _uiManager, Furniture _furniture, Sprite sprite) {
		uiManager = _uiManager;
		furniture = _furniture;
		Image image = Instantiate(iconImagePrefab, layoutGroup.transform);
		image.sprite = sprite;
	}

	public void Show() {
		showing = true;
		controller.SetTrigger("Show");
	}

	public void Hide() {
		showing = false;
		controller.SetTrigger("Hide");
	}

	public void HideAndDestroy() {
		Hide();
		Invoke("Destroy", 2);
	}

	private void Destroy() {
		if (gameObject) Destroy(gameObject);
	}

	private void Update() {
		if (showing) {
			(transform as RectTransform).anchoredPosition = uiManager.GetCanvasPosition(furniture.transform.position + furniture.UIInfoOffset);
		}
	}


}
