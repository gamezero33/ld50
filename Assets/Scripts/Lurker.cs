using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lurker : MonoBehaviour {

	[SerializeField] private MinMaxFloat sizeRange = new MinMaxFloat(0.5f, 2);
	[SerializeField] private MinMaxFloat lifespanRange = new MinMaxFloat(3, 5);
	[SerializeField] private float naturalLifeDrain = 0.2f;
	[SerializeField] private MinMaxFloat speedRange = new MinMaxFloat(1, 4);
	[Space]
	[SerializeField] private float lifeSpan;
	[SerializeField] private Vector2Int gridPos;
	[SerializeField] private float lightLevel;
	[SerializeField] private Vector3 velocity;

	private PlayerController playerController;
	private NavMeshAgent agent;
	private new Renderer renderer;
	private float age;
	private Vector3 spawnPos;
	private bool returning = false;

	public void Init(PlayerController _playerController, Vector3 pos) {
		age = 0;
		lifeSpan = lifespanRange.Random();
		spawnPos = pos;
		returning = false;
		transform.position = pos;
		transform.localScale = Vector3.one * sizeRange.Random();
		agent = GetComponentInChildren<NavMeshAgent>();
		renderer = GetComponentInChildren<Renderer>();
		playerController = _playerController;
		agent.speed = speedRange.Random();
	}

	private void OnCollisionEnter(Collision collision) {
		PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
		if (pc) {
			age -= lifeSpan / 2;
			returning = true;
			pc.thrall = this;
		}
	}

	private void Update() {
		velocity = agent.velocity;
		gridPos = playerController.LightSampler.GetGridPos(transform.position);
		lightLevel = playerController.LightSampler.GetLightLevel(gridPos);

		age += Time.deltaTime * 2 * (lightLevel + naturalLifeDrain);
		renderer.material.SetColor("_Color", renderer.material.GetColor("_Color").ToAlpha(1 - (age / lifeSpan)));
		if (age >= lifeSpan && gameObject) Destroy(gameObject);
		else if (returning) {
			if (agent.velocity.magnitude <= 0.2f && lightLevel > 0) {
				spawnPos += transform.forward;
			}
			agent.SetDestination(spawnPos);
		} else agent.SetDestination(playerController.transform.position);
	}

}
