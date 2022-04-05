using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField] private Camera mainCamera;
	[SerializeField] private Canvas canvas;
	public Canvas Canvas => canvas;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private TMPro.TextMeshProUGUI gameOverMessage;
	[SerializeField] private LayoutGroup inventoryLayout;
	[SerializeField] private TMPro.TextMeshProUGUI clock;
	[SerializeField] private Image brain;
	[SerializeField] private Image sanityMeter;
	[SerializeField] private Gradient sanityGradient;
	[SerializeField] private Tweener sanityPulsar;
	[Space]
	[SerializeField] private UIProgressBar progressBarPrefab;
	[SerializeField] private UIInfoDisplay infoDisplayPrefab;
	[SerializeField] private UIInventoryItem inventoryItemPrefab;

	private Transform infoDisplayParent;
	private Transform progressBarParent;
	private List<UIInventoryItem> inventoryItems;
	private bool gameOver;

	private float startTime;

	private void Start() {
		startTime = Time.realtimeSinceStartup;
	}

	private void Update() {
		if (!gameOver && clock) {
			clock.text = FormatTime(CurrentTime);
		} else {
			if (Input.GetMouseButtonUp(0)) {
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}
	}

	public float CurrentTime {
		get {
			return Time.realtimeSinceStartup - startTime;
		}
	}

	public string FormatTime(float time) {
		int sec = Mathf.FloorToInt(time % 60);
		int min = Mathf.FloorToInt(time / 60);
		int hrs = Mathf.FloorToInt(time / 60 / 60);
		string s = sec < 10 ? "0" + sec.ToString() : sec.ToString();
		string m = min < 10 ? "0" + min.ToString() : min.ToString();
		string h = hrs < 10 ? "0" + hrs.ToString() : hrs.ToString();
		return string.Format("{0}:{1}:{2}", h, m, s);
	}
		 
	public void GameOver(string message) {
		gameOver = true;
		gameOverMessage.text = message;
		gameOverScreen.SetActive(true);
	}

	public void UpdateSanity(float sanity) {
		brain.color = sanityGradient.Evaluate(sanity);
		sanityMeter.fillAmount = sanity;
		sanityMeter.color = sanityGradient.Evaluate(sanity);
		sanityPulsar.Duration = sanity * 3 + 0.5f;
	}

	public void AddToInventory(InventoryItem itemData, int qty = 1) {
		UIInventoryItem item = InventoryHasItem(itemData);
		if (!item) {
			item = Instantiate(inventoryItemPrefab, inventoryLayout.transform);
			item.transform.localScale = Vector3.one;
			item.Init(itemData, qty);
			inventoryItems.Add(item);
		} else {
			item.Qty += qty;
		}
	}

	public void RemoveFromInventory(InventoryItem itemData, int qty = 1) {
		UIInventoryItem item = InventoryHasItem(itemData);
		if (item) {
			item.Qty -= qty;
			if (item.Qty <= 0) {
				inventoryItems.Remove(item);
				if (item.gameObject)
					Destroy(item.gameObject);
			}
		}
	}

	public UIInventoryItem InventoryHasItem(InventoryItem itemData) {
		if (inventoryItems == null) inventoryItems = new List<UIInventoryItem>();
		if (inventoryItems.Count == 0) return null;
		foreach (UIInventoryItem item in inventoryItems) {
			if (item.itemData.name == itemData.name) return item;
		}
		return null;
	}

	public UIInfoDisplay CreateInfoDisplay(Furniture furniture, Sprite sprite) {
		if (infoDisplayParent == null) {
			infoDisplayParent = new GameObject("Info Displays").transform;
			infoDisplayParent.SetParent(canvas.transform);
			infoDisplayParent.localPosition = Vector3.zero;
			infoDisplayParent.localScale = Vector3.one;
		}
		UIInfoDisplay infoDisplay = Instantiate(infoDisplayPrefab, infoDisplayParent);
		infoDisplay.Init(this, furniture, sprite);
		return infoDisplay;
	}

	public UIProgressBar CreateProgressBar(Transform target, Vector3 targetOffset) {
		if (progressBarParent == null) {
			progressBarParent = new GameObject("Progress Bars").transform;
			progressBarParent.SetParent(canvas.transform);
			progressBarParent.localPosition = Vector3.zero;
			progressBarParent.localScale = Vector3.one;
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
			return mainCamera.ScreenToWorldPoint(new Vector3(canvasPos.x, canvasPos.y, mainCamera.transform.localPosition.z));
		} else
			return Vector3.zero;
	}

}
