using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject[] Pieces;
	
	private void Start () {
	
		Instantiate(Pieces[Random.Range(0,Pieces.Length)]);
	}
}
