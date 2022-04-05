using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour {

	[SerializeField] private Image iconImage;
	[SerializeField] private TMPro.TextMeshProUGUI tmpQty;
	[HideInInspector] public InventoryItem itemData;

	private int qty;
	public int Qty {
		get {
			return qty;
		}
		set {
			qty = value;
			tmpQty.text = string.Format("x{0}", qty);
		}
	}

	public void Init(InventoryItem data, int qty) {
		itemData = data;
		iconImage.sprite = itemData.sprite;
		Qty = qty;
	}

}
