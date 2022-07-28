using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestrueixis : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}
}
