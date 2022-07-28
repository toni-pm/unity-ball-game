using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControlador : MonoBehaviour {

	public GameObject jugador;
	private Vector3 distancia;

	// Use this for initialization
	void Start () {
		distancia = this.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = jugador.transform.position + distancia;
	}
}
