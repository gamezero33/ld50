using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

	[SerializeField] private UIGridDisplay uiGridDisplay;
	[SerializeField] private UIManager uiManager;
	[SerializeField] private LightSampler lightSampler;
	public LightSampler LightSampler => lightSampler;
	[SerializeField] private Animator controller; 
	[SerializeField] private float moveMultiplier;
	[SerializeField] private Transform cameraPivot;

	[SerializeField] private float darkThreshold = 0.2f;
	[SerializeField] private Lurker lurkerPrefab;
	[SerializeField] private MinMaxFloat lurkerRange = new MinMaxFloat(3, 5);
	[SerializeField] private int maxLurkers = 8;
	[SerializeField] private float jeopardySampleRate = 2;
	[SerializeField] private float sanityMultiplier = 0.05f;
	[SerializeField, Range(0, 1)] private float lightLevel;
	[SerializeField] private Vector2Int gridPos;

	public Lurker thrall;

	private NavMeshAgent agent;
	private Transform angleRefTF;
	private float jeopardyTimer;
	private float sanity = 1;

	private List<Lurker> lurkers;
	private Vector2Int[] sampleDirs = { Vector2Int.zero, Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };

	private bool gameOver;
	private MinMaxFloat idleTimerInterval = new MinMaxFloat(3, 10);
	private float idleTimer;
	private float targetIdleBlend;

	private void Start() {
		gameOver = false;
		lurkers = new List<Lurker>();
		agent = GetComponentInChildren<NavMeshAgent>();
		angleRefTF = new GameObject("AngleRefTF").transform;
		angleRefTF.rotation = cameraPivot.rotation;
	}

	private void Update() {
		if (gameOver) return;

		idleTimer += Time.deltaTime;
		if (idleTimer > idleTimerInterval.Random()) {
			idleTimer = 0;
			targetIdleBlend = Random.Range(0f, 1f);
		}

		controller.SetFloat("Idle", Mathf.Lerp(controller.GetFloat("Idle"), targetIdleBlend, Time.deltaTime));
		controller.SetBool("Walk", agent.velocity.magnitude > 0.1f);

		for (int l = lurkers.Count - 1; l > 0; l--) {
			if (lurkers[l] == null) lurkers.RemoveAt(l);
		}

		gridPos = lightSampler.GetGridPos(transform.position);
		lightLevel = lightSampler.GetLightLevel(gridPos);
		//lightLevel = 0;
		//for (int i = 0; i < 5; i++) {
		//	lightLevel += lightSampler.GetLightLevel(gridPos + sampleDirs[i]);
		//}
		//lightLevel /= 5;

		Vector3 move = angleRefTF.TransformPoint(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		move.y = 0;
		if (move.magnitude > 0.01) {
			agent.SetDestination(transform.position + move.normalized * moveMultiplier);
		}

		jeopardyTimer += Time.deltaTime;
		if (jeopardyTimer >= jeopardySampleRate * sanity) {
			jeopardyTimer = 0;
			if (uiGridDisplay.gameObject.activeSelf) {
				for (int y = 0; y < 18; y++) {
					for (int x = 0; x < 32; x++) {
						Vector2Int pos = new Vector2Int(x, y);
						uiGridDisplay.Set(pos, (Color.blue * lightSampler.GetLightLevel(pos)).ToAlpha(1));
					}
				}
			}
			uiGridDisplay.Set(gridPos, (Color.green * lightLevel).ToAlpha(1));
			SpawnLurker();
		}

		if (lightLevel >= 0.1f) {
			sanity += (lightLevel * lightLevel) * Time.deltaTime * sanityMultiplier;
		} else {
			sanity -= ((1 - lightLevel) * (1 - lightLevel)) * Time.deltaTime * sanityMultiplier;
		}
		sanity = Mathf.Clamp01(sanity);
		uiManager.UpdateSanity(sanity);

		if (thrall != null && Random.Range(0f, 1f) > sanity) {
			agent.SetDestination(thrall.transform.position);
		}

		if (sanity <= 0) GameOver();
	}


	private void GameOver() {
		gameOver = true;
		uiManager.GameOver(string.Format("You survived for: {0}", uiManager.FormatTime(uiManager.CurrentTime)));
	}

	Vector3 closePoint;

	private void SpawnLurker() {
		closePoint = transform.position.Random(lurkerRange.min, lurkerRange.max, true);
		float closePointLight = lightSampler.GetLightLevel(lightSampler.GetGridPos(closePoint));
		if (closePointLight <= darkThreshold) {
			uiGridDisplay.Set(lightSampler.GetGridPos(closePoint), Color.white);
			if (lurkers.Count < maxLurkers) {
				Lurker lurker = Instantiate(lurkerPrefab);
				lurker.Init(this, closePoint + (Vector3.forward * 1) + (Vector3.right * 1));
				lurkers.Add(lurker);
			}
		}
	}

	private void OnDrawGizmosSelected() {
		//Gizmos.DrawSphere(closePoint, 0.5f);
		//UnityEditor.Handles.color = Color.yellow.ToAlpha(0.02f);
		//UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, lurkerRange.max);
		//UnityEditor.Handles.color = Color.blue.ToAlpha(0.02f);
		//UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, lurkerRange.min);
	}

}
