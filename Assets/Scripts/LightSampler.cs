using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LightSampler : MonoBehaviour {

	[SerializeField] private UIManager uiManager;
	[SerializeField] private float factor = 1;
	[SerializeField] private Image samplerRenderDisplay;
	[SerializeField] private Camera samplerCamera;

	private RenderTexture renderTexture;
	private Image[] sampleDisplay;
	private Texture2D cacheTex;

	private void Start() {
		Init();
	}

	private void Init() {
		renderTexture = new RenderTexture(Mathf.RoundToInt(16 * factor), Mathf.RoundToInt(9 * factor), 8, RenderTextureFormat.R8);
		renderTexture.Create();
		renderTexture.filterMode = FilterMode.Point;
		renderTexture.wrapMode = TextureWrapMode.Clamp;
		cacheTex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R8, false, true);
		samplerCamera.targetTexture = renderTexture;
		samplerRenderDisplay.material.mainTexture = renderTexture;
	}


	
	public Vector2Int GetGridPos(Vector3 worldPos) {
		float cellsize = Screen.width / (16 * factor);
		Vector2 canvasPos = uiManager.GetCanvasPosition(worldPos);
		return new Vector2Int(Mathf.RoundToInt(canvasPos.x / cellsize), Mathf.RoundToInt(canvasPos.y / cellsize));
	}

	public float GetLightLevel(Vector2Int gridPos) {
		return GetColorMap()[Mathf.RoundToInt(gridPos.x + (renderTexture.height - gridPos.y - 1) * renderTexture.width)].r;
	}

	public Vector2Int? GetRandomNearestDarkCell(Vector2Int gridPos, float darkThreshold = 0.2f) {
		Color[] cmap = GetColorMap();
		List<int> validCells = new List<int>();
		List<int> distances = new List<int>();
		int closestDistance = renderTexture.width + renderTexture.height;
		for (int i = 0; i < cmap.Length; i++) {
			int x = i % renderTexture.width;
			int y = renderTexture.height - (i / renderTexture.width);
			if (gridPos.x == x && gridPos.y == y) continue;
			if (cmap[i].r <= darkThreshold) {
				int d = Mathf.Abs(gridPos.x - x) + Mathf.Abs(gridPos.y - y);
				if (d < closestDistance) closestDistance = d;
				distances.Add(d);
				validCells.Add(i);
			}
		}
		if (distances.Count == 0) return null;
		List<int> shortList = new List<int>();
		for (int d = 0; d < distances.Count; d++) {
			if (distances[d] == closestDistance) shortList.Add(validCells[d]);
		}
		if (shortList.Count == 0) return null;
		int choice = shortList.Random();
		string list = "(" + (choice % renderTexture.width) + ", " + (choice / renderTexture.width) + "), ";
		foreach (var i in shortList) {
			list += "(" + (i % renderTexture.width) + ", " + (i / renderTexture.width) + "), ";
		}
		Debug.Log(list);
		return new Vector2Int(choice % renderTexture.width, choice / renderTexture.width - 1);
	}

	private Color[] GetColorMap() {
		RenderTexture.active = renderTexture;
		cacheTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
		cacheTex.Apply();
		return cacheTex.GetPixels();
	}

}
