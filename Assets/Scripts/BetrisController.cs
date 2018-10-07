using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BetrisController : MonoBehaviour {
	
	
	private const float MaxSpeed = .005f;
	private const float Acceleration = .1f;
	private const float FallDistance = .1f;
	private const float MoveDistance = .5f;

	[HideInInspector] public float Speed = .5f;
	[HideInInspector] public GameObject NextPiece;
	
	public GameObject[] Pieces;

	private bool _isDead;
	private Vector2 _startPosition;

	private void Start()
	{
		_startPosition = transform.position;
		NextPiece = Pieces[Random.Range(0,Pieces.Length)];
		StartCoroutine(Fall());
	}

	private void Update()
	{
		if (_isDead || CompareTag("Done")) return;
		if (Input.GetKeyDown(KeyCode.E))
		{
			transform.Rotate(Vector3.forward,-90);
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			transform.Rotate(Vector3.forward,90);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			transform.position = new Vector2(transform.position.x - MoveDistance, transform.position.y);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			transform.position = new Vector2(transform.position.x + MoveDistance, transform.position.y);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			transform.position = new Vector2(transform.position.x + MoveDistance, transform.position.y);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (_isDead || CompareTag("Done")) return;
		if (!other.gameObject.CompareTag("Done")) return;
		gameObject.tag = "Done";
		Drop();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_isDead) return;
		_isDead = true;
	}

	private void Drop()
	{
		GameObject _new = Instantiate(NextPiece,_startPosition,Quaternion.identity);
		_new.tag = "Untagged";
	}

	private IEnumerator Fall()
	{
		while (!_isDead && !CompareTag("Done"))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				yield return new WaitForSeconds(MaxSpeed);
			}
			else
			{
				yield return new WaitForSeconds(Speed);
			}
			transform.position = new Vector2(transform.position.x, transform.position.y - FallDistance);
		}
	}
	private void LevelUp()
	{
		Speed -= Acceleration;
	}
}
