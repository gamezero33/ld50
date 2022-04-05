using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LightSampler : MonoBehaviour {

	[SerializeField] private UIManager uiManager;
	[SerializeField] private float factor = 1;
	[SerializeField] private int minDistance = 2;
	[SerializeField] private Image samplerRenderDisplay;
	[SerializeField] private Camera samplerCamera;

	private RenderTexture renderTexture;
	public RenderTexture RenderTexture => renderTexture;
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
		int x = Mathf.Clamp(Mathf.RoundToInt(canvasPos.x / cellsize), 0, 32);
		int y = Mathf.Clamp(Mathf.RoundToInt(canvasPos.y / cellsize), 0, 18);
		return new Vector2Int(x, y);
	}
	

	public float GetLightLevel(Vector2Int gridPos) {
		//gridPos.y = renderTexture.height - gridPos.y - 1;
		if (gridPos.x < 0 || gridPos.x >= renderTexture.width) return 0;
		if (gridPos.y < 0 || gridPos.y >= renderTexture.height) return 0;
		int i = (gridPos.y * renderTexture.width) + gridPos.x;
		return GetColorMap()[i].r;
	}
	

	private Color[] GetColorMap() {
		RenderTexture.active = renderTexture;
		cacheTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
		cacheTex.Apply();
		return cacheTex.GetPixels();
	}

}
