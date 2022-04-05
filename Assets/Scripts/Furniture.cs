using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

	public static Furniture selection;

	[SerializeField] protected UIManager uiManager;
	[SerializeField] private Animator animator;
	//[SerializeField] private Sprite[] uiInfoSprites;
	//public Sprite[] UIInfoSprites => uiInfoSprites;
	[SerializeField] private Vector3 uiInfoOffset;
	public Vector3 UIInfoOffset => uiInfoOffset;
	[SerializeField] private FireLight fireLight;
	[SerializeField] protected InventoryItem fuel;
	[SerializeField] private float refuelAmount = 25;
	[Header("Breakable Settings")]
	[SerializeField] private bool breakable;
	[SerializeField] private bool collectable;
	[SerializeField] private string breakAnimTrigger;
	[SerializeField] private InventoryItem breakItem;
	[SerializeField] private bool destroyOnPickUp;
	[SerializeField] private MinMaxInt itemsYield;

	protected UIInfoDisplay uiInfoDisplay;
	private cakeslice.Outline[] outlines;

	private void Awake() {
		outlines = GetComponentsInChildren<cakeslice.Outline>();
	}
	protected virtual void Start() {
		SetOutline(false);
	}

	private void Select(bool selected, bool force = false) {
		if (selected) {
			if (force) {
				if (selection) selection.Select(false);
				selection = null;
			}
			if (selection == null) {
				selection = this;
				ShowInfoBox();
				SetOutline(true, 1);
			}
		} else {
			if (selection == this) {
				selection = null;
				HideInfoBox();
				SetOutline(false);
			}
		}
	}

	private void SetOutline(bool enabled, int color = 0) {
		foreach (cakeslice.Outline outline in outlines) {
			outline.enabled = enabled;
			outline.color = color;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponentInParent<PlayerController>()) {
			Select(true, true);
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.GetComponentInParent<PlayerController>()) {
			Select(true);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponentInParent<PlayerController>()) {
			Select(false);
		}
	}

	protected virtual void Activate() {
		if (fireLight && uiManager.InventoryHasItem(fuel)) {
			fireLight.Refuel(refuelAmount);
			uiManager.RemoveFromInventory(fuel);
			//if (!uiManager.InventoryHasItem(fuel)) HideInfoBox();
		}
		if (breakable)
			animator.SetTrigger(breakAnimTrigger);
		if (collectable)
			PickUpItem();
	}

	protected virtual void Update() {
		if (selection == this && uiInfoDisplay && Input.GetKeyDown(KeyCode.E)) {
			Activate();
		}
	}

	protected virtual void ShowInfoBox() {
		if (breakable || fuel || collectable) {
			if (uiInfoDisplay == null) {
				uiInfoDisplay = uiManager.CreateInfoDisplay(this, fuel != null ? fuel.sprite : breakItem ? breakItem.sprite : null);
				if (fireLight) {
					fireLight.infoDisplay = uiInfoDisplay;
				} else {
					uiInfoDisplay.ProgressBar.gameObject.SetActive(false);
				}
			}
			if (fireLight)
				uiInfoDisplay.LayoutGroup.gameObject.SetActive(fuel && uiManager.InventoryHasItem(fuel));
			uiInfoDisplay.Show();
		}
	}

	private void HideInfoBox() {
		if (uiInfoDisplay) uiInfoDisplay.Hide();
	}

	public void PickUpItem() {
		uiManager.AddToInventory(breakItem, itemsYield.Random());
		if (destroyOnPickUp && gameObject) {
			if (uiInfoDisplay)
				uiInfoDisplay.HideAndDestroy();
			Destroy(gameObject);
		}
	}

}
