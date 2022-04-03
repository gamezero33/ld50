using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour {

	[SerializeField] private Vector3 progressBarOffset;
	[SerializeField] private MinMaxFloat age;
	[SerializeField] private MinMaxFloat flickerSpeed;
	[SerializeField] private MinMaxFloat flickerRate;
	[SerializeField] private bool useRange;
	[SerializeField] private MinMaxFloat flickerRange;
	[SerializeField] private bool useIntensity;
	[SerializeField] private MinMaxFloat flickerIntensity;
	[SerializeField] private bool useMotion;
	[SerializeField] private MinMaxFloat flickerMotion;
	[SerializeField] private bool useColors;
	[SerializeField] private Gradient flickerColors;
	[SerializeField] private Renderer effectRenderer;
	[SerializeField] private string effectRendererField;

	private UIManager uiManager;
	private UIProgressBar progressBar;
	private Light light;
	private float flickerTimer;
	private float flickerTimeout;
	private bool fading;
	private Vector3 origin;

	private void Awake() {
		light = GetComponentInChildren<Light>();
	}

	private void Start() {
		origin = transform.position;
		if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
		progressBar = uiManager.CreateProgressBar(transform, progressBarOffset);
	}

	private void Update() {
		float deltaTime = Time.deltaTime;
		flickerTimer += deltaTime;
		age.min += deltaTime;
		progressBar.Progress = 1 - (age.min / age.max);
		if (flickerTimer >= flickerTimeout) {
			flickerTimer = 0;
			flickerTimeout = flickerRate.Random();
			if (!fading) {
				StartCoroutine(fade());
			}
		}
	}

	private IEnumerator fade() {
		fading = true;
		float startIntensity = light.intensity;
		float endIntensity = flickerIntensity.Random() * progressBar.Progress;
		Vector3 startPos = transform.position;
		Vector3 endPos = origin.Random(flickerMotion.Random());
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime * flickerSpeed.Random();
			if (useIntensity) {
				light.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
			}
			if (useMotion) {
				transform.position = Vector3.Lerp(startPos, endPos, t);
			}
			if (effectRenderer) {
				effectRenderer.material.SetFloat(effectRendererField, light.intensity);
			}

			yield return null;
		}
		fading = false;
	}

}
