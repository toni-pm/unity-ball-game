using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JocControlador : MonoBehaviour {

	[HideInInspector]public int puntuacio = 0;


	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("PuntuacioMax", 0);
	}

	public void CarregaEscena (int nEscena) {
		gameObject.GetComponent<AudioSource>().Stop();
		SceneManager.LoadScene(nEscena);	
	}

	public void Sortir () {
		#if UNITY_STANDALONE 
			Application.Quit ();
		#endif

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
