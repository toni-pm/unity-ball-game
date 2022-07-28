using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugadorControlador : MonoBehaviour {
    
	public int nivell;
	public float velocitat = 10F;
	public float salt = 5F;
	private bool potSaltar = true;
	private Rigidbody rb;
	private GameObject ctrlJoc;
	private GameObject ctrlPlayer;
	private JocControlador scriptCtrlJoc;
    private GameObject[] coleccionables1;
    private GameObject[] coleccionables2;
    private int nColeccionables;
    private Color[] arrayColors = { new Color32(255,0,0,1), new Color32(0,255,0,1), new Color32(0,0,255,1), new Color32(0,0,0,1), new Color32(255,255,255,1), 
		new Color32(255,0,255,1), new Color32(128,0,128,1), new Color32(139,69,19,1), new Color32(0,255,255,1), new Color32(0,250,154,1), new Color32(0,100,0,1),
		new Color32(124,252,0,1), new Color32(255,255,0,1), new Color32(255,69,0,1), new Color32(128,0,0,1), new Color32(255,192,203,1), new Color32(128,128,128,1)};

	private int puntuacioMax;
	public Text txtMarcador;
    public AudioClip audioFileSalt;
    public AudioClip audioFileMultiplicar;

    private GameObject prefab1;
	private GameObject prefab2;
	private int nColeccionables1;
	private int nColeccionables2;
	private float temps;
	private bool perdut = false;
	private bool guanyat = false;
	private float grandariaJugador = 0;
	private bool doblarJugador = false;
    Random rnd;

	void Update() {
		// Si esta al terra
		if (potSaltar) {
			if (Input.GetKeyDown ("space")) {
				AudioSource.PlayClipAtPoint (audioFileSalt, transform.position);
				rb.AddForce (new Vector3 (0, salt, 0), ForceMode.Impulse);
			}
		} 
		temps -= Time.deltaTime;
		if (!guanyat)
        {
            txtMarcador.text = "Marcador: " + scriptCtrlJoc.puntuacio + "/" + puntuacioMax + " \nTemps: " + (int)temps;
			if (temps < 0)
			{
				GameOver();
			}
			if (nivell == 4)
            {
                CrearColeccionables();
                if (rb.transform.position.y < 0) {
					GameOver ();
				} 
			}
		}
	}

	// Use this for initialization
	void Start () {
		switch (nivell) {
			case 1:
				nColeccionables1 = 20;
				nColeccionables2 = 5;
				temps = 50;
				break;
			case 2:
				nColeccionables1 = 40;
				nColeccionables2 = 10;
				temps = 70;
			break;
			case 3: 
				nColeccionables1 = 60;
				nColeccionables2 = 20;
				temps = 80;
				break;
			case 4: 
				nColeccionables1 = 2;
				nColeccionables2 = 2;	
				temps = 200;
				break;
		}

		rb = GetComponent<Rigidbody>();
		ctrlJoc = GameObject.FindGameObjectWithTag ("GameController");
		ctrlPlayer = GameObject.FindGameObjectWithTag ("Player");

		if (ctrlJoc != null) {
            perdut = false;
			guanyat = false;
			scriptCtrlJoc = ctrlJoc.GetComponent<JocControlador>();
			scriptCtrlJoc.puntuacio = 0;
			rnd = new Random ();

			CrearColeccionables();

            if (nivell == 4) {
                puntuacioMax = 1000;
            }
            else
            {
                puntuacioMax = nColeccionables1 + nColeccionables2 * 5;
            }
            txtMarcador.text = "Marcador: 0/" + puntuacioMax + " \nTemps: " + (int)temps;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float movVertical = Input.GetAxis("Vertical");
		float movHorizontal = Input.GetAxis("Horizontal");
		movVertical *= Time.deltaTime;
		movHorizontal *= Time.deltaTime;

		Vector3 moviment = new Vector3 (movHorizontal, 0f, movVertical) * velocitat;
		rb.AddForce(moviment);
	}

	void OnTriggerEnter (Collider other) {
		Destroy (other.gameObject);
		string tag = other.gameObject.tag;

		int nColor = Random.Range(0, arrayColors.Length);
		ctrlPlayer.transform.GetComponent<Renderer>().material.color = arrayColors[nColor];

		if (nivell == 4) {
			grandariaJugador = ctrlPlayer.gameObject.transform.localScale.x;

			if (grandariaJugador > 0.5)
            {
                ctrlPlayer.gameObject.transform.localScale -= new Vector3(0.007f, 0.007f, 0.007f);
            } 
		}

		if (tag == "Coleccionable1") {
			scriptCtrlJoc.puntuacio++;
        }
		else if (tag == "Coleccionable2") {
			scriptCtrlJoc.puntuacio = scriptCtrlJoc.puntuacio + 5;
        }
		if (!perdut) {
			if (scriptCtrlJoc.puntuacio >= puntuacioMax) {
				guanyat = true;

				if (nivell == 3) {
					txtMarcador.text = "HAS GUANYAT! Passant al nivell INFERNAL";
                    StartCoroutine(Esperar(5.0f, nivell + 1));
				} 
                else if (nivell == 4)
                {
                    txtMarcador.text = "HAS GUANYAT! Tornant al menu principal";
                    StartCoroutine(Esperar(5.0f, 0));
                }
				else {
					txtMarcador.text = "HAS GUANYAT! Passant al nivell " + (nivell + 1);
                    StartCoroutine(Esperar(5.0f, nivell + 1));
                }
            }
            else
            {
                txtMarcador.text = "Marcador: " + scriptCtrlJoc.puntuacio + "/" + puntuacioMax + " \nTemps: " + (int)temps;
            }
        }
	}

	void OnCollisionEnter (Collision other) 
	{
		if (other.gameObject.tag == "Terra") {
			potSaltar = true;
		}
	}

	void OnCollisionExit (Collision other) 
	{
		if (other.gameObject.tag == "Terra") {
			potSaltar = false;
		}
	}

	private IEnumerator Esperar(float seconds, int nivellSeguent)
	{
		yield return new WaitForSeconds(seconds);
		gameObject.GetComponent<AudioSource>().Stop();
		SceneManager.LoadScene (nivellSeguent);	
	}

	private void GameOver () {
		perdut = true;
		txtMarcador.text = "HAS PERDUT!";
		StartCoroutine(Esperar(5.0f, 0));
	}

	void CrearColeccionables ()
    {
        coleccionables1 = GameObject.FindGameObjectsWithTag("Coleccionable1");
        coleccionables2 = GameObject.FindGameObjectsWithTag("Coleccionable2");
        nColeccionables = coleccionables1.Length + coleccionables2.Length;
        if (nColeccionables < 200)
        {
            prefab1 = (GameObject)Instantiate(Resources.Load("Coleccionable1"));
            prefab2 = (GameObject)Instantiate(Resources.Load("Coleccionable2"));
            for (int i = 1; i <= nColeccionables1; i++)
            {
                float x = Random.Range(-8f, 8f);
                float y = Random.Range(0.5f, 2.2f);
                if (nivell == 4) y = Random.Range(2f, 3.0f);
                float z = Random.Range(-8, 8f);
                Vector3 pos = new Vector3(x, y, z);
                Instantiate(prefab1, pos, Quaternion.identity);
            }

            for (int i = 1; i <= nColeccionables2; i++)
            {
                float x = Random.Range(-8f, 8f);
                float y = Random.Range(0.5f, 2.2f);
                if (nivell == 4) y = Random.Range(2.0f, 3.0f);
                float z = Random.Range(-8, 8f);
                Vector3 pos = new Vector3(x, y, z);
                Instantiate(prefab2, pos, Quaternion.identity);
            }
        }
    }
}
