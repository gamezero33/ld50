using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

	[SerializeField] private UIManager uiManager;
	[SerializeField] private LightSampler lightSampler;
	public LightSampler LightSampler => lightSampler;
	[SerializeField] private float moveMultiplier;
	[SerializeField] private Transform cameraPivot;

	[SerializeField] private Lurker lurkerPrefab;
	[SerializeField] private int maxLurkers = 8;
	[SerializeField] private float jeopardySampleRate = 2;
	[SerializeField, Range(0, 1)] private float lightLevel;
	[SerializeField] private Vector2Int gridPos;

	private NavMeshAgent agent;
	private Transform angleRefTF;
	private float jeopardyTimer;

	private List<Lurker> lurkers;

	private void Start() {
		lurkers = new List<Lurker>();
		agent = GetComponentInChildren<NavMeshAgent>();
		angleRefTF = new GameObject("AngleRefTF").transform;
		angleRefTF.rotation = cameraPivot.rotation;
	}

	private void Update() {
		for (int l = lurkers.Count - 1; l > 0; l--) {
			if (lurkers[l] == null) lurkers.RemoveAt(l);
		}

		Vector3 move = angleRefTF.TransformPoint(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		move.y = 0;
		if (move.magnitude > 0.01) {
			agent.SetDestination(transform.position + move.normalized * moveMultiplier);
		}

		jeopardyTimer += Time.deltaTime;
		if (jeopardyTimer >= jeopardySampleRate) {
			jeopardyTimer = 0;
			gridPos = lightSampler.GetGridPos(transform.position);
			lightLevel = lightSampler.GetLightLevel(gridPos);
			if (Random.Range(0f, 1f) > lightLevel) {
				SpawnLurker();
			}
		}
	}

	private void SpawnLurker() {
		// if there are not too many lurkers
		if (lurkers.Count < maxLurkers) {
			Lurker lurker = Instantiate(lurkerPrefab);
			// place lurker at world pos of nearest pitch black square
			lurker.transform.position = uiManager.GetWorldPosFromCanvasPos((Vector2)lightSampler.GetRandomNearestDarkCell(gridPos, lightLevel / 2));
			// initialise lurker
			lurker.Init(this);
			// add lurker to list
			lurkers.Add(lurker);
		}
	}

}
