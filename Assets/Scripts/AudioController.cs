using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip audioFile;

	void OnTriggerEnter(Collider other) { 
		if (other.tag == "Player") {
			AudioSource.PlayClipAtPoint (audioFile, transform.position);
		}
	}
}
