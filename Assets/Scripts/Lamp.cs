using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Furniture {


	[SerializeField] private Material litMaterial;
	[SerializeField] private Material unlitMaterial;
	[SerializeField] private new Light light;
	[SerializeField] private Renderer[] lampObjects;

	public bool state = true;

	protected override void Start() {
		state = true;
		base.Start();
	}

	protected override void Update() {
		if (transform.up.y < 0.6f) {
			TurnOff();
		}

		base.Update();
	}

	protected override void Activate() {
		TurnOn();
	}

	public void TurnOn() {
		if (!uiManager.InventoryHasItem(fuel)) return;
		uiManager.RemoveFromInventory(fuel);
		uiInfoDisplay.Hide();
		state = true;
		transform.rotation = Quaternion.identity;
		Vector3 pos = transform.position; pos.y = 0;
		transform.position = pos;
		if (light) light.gameObject.SetActive(true);
		foreach (Renderer renderer in lampObjects) {
			renderer.material = litMaterial;
		}
	}

	public void TurnOff() {
		state = false;
		if (light) light.gameObject.SetActive(false);
		foreach (Renderer renderer in lampObjects) {
			renderer.material = unlitMaterial;
		}
	}


	protected override void ShowInfoBox() {
		if (state) return;
		if (!uiManager.InventoryHasItem(fuel)) return;
		base.ShowInfoBox();
	}

}
