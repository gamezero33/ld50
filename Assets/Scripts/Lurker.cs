using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lurker : MonoBehaviour {

	[SerializeField] private MinMaxFloat lifespanRange = new MinMaxFloat(3, 5);
	[Space]
	[SerializeField] private float lifeSpan;
	[SerializeField] private Vector2Int gridPos;
	[SerializeField] private float lightLevel;

	private PlayerController playerController;
	private NavMeshAgent agent;
	private new Renderer renderer;
	private float age;

	public void Init(PlayerController _playerController) {
		age = 0;
		lifeSpan = lifespanRange.Random();
		agent = GetComponentInChildren<NavMeshAgent>();
		renderer = GetComponentInChildren<Renderer>();
		playerController = _playerController;
	}

	private void Update() {
		gridPos = playerController.LightSampler.GetGridPos(transform.position);
		lightLevel = playerController.LightSampler.GetLightLevel(gridPos);

		age += Time.deltaTime * 2 * (1 - lightLevel);
		renderer.material.SetColor("_Color", renderer.material.GetColor("_Color").ToAlpha(1 - (age / lifeSpan)));
		if (age >= lifeSpan && gameObject) Destroy(gameObject);
			else agent.SetDestination(playerController.transform.position);
	}

}
