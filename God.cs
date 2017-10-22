using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {

	public UnityEngine.UI.Text generation_listing;
	public int generation;
	public int genome;

	public int MAX_GENOMES; // MUST BE EVEN
	bool detecting_round;
	bool detecting_deadlock;

	// Use this for initialization
	void Start () {
		generation = 0;
		MAX_GENOMES = 8;
		genome = 0;
		detecting_round = false;
		detecting_deadlock = false;
	}
	
	// Update is called once per frame
	void Update () {
		generation_listing.text = "Generation      # " + (generation + 1) + "\nGenome      # " + (genome + 1);
		if (!detecting_round) {
			StartCoroutine (DetectNewRound ());
		}
		if (!detecting_deadlock) {
			StartCoroutine (DetectDeadlock ());
		}
	}

	IEnumerator DetectNewRound()
	{
		detecting_round = true;
		if (GameObject.FindGameObjectsWithTag ("Cart").Length == 0) {
			yield return new WaitForSeconds (5f);
			if (GameObject.FindGameObjectsWithTag ("Cart").Length == 0) {
				NextGenome ();
			}
		} else {
			yield return new WaitForSeconds (5f);
		}
		detecting_round = false;

	}

	IEnumerator DetectDeadlock()
	{
		bool deadlock = true;
		detecting_deadlock = true;
		GameObject[] carts = GameObject.FindGameObjectsWithTag ("Cart");
		foreach (GameObject cart in carts) {
			if (!cart.GetComponent<Cart> ().stopping) {
				deadlock = false;
			}
		}
		yield return new WaitForSeconds (10f);

		GameObject[] carts_2 = GameObject.FindGameObjectsWithTag ("Cart");
		foreach (GameObject cart in carts_2) {
			if (!cart.GetComponent<Cart> ().stopping) {
				deadlock = false;
			}
		}

		if (deadlock == true) {
			try
			{
			GameObject[] intersections = GameObject.FindGameObjectsWithTag ("Intersection");
			foreach (GameObject intersection in intersections) {
				foreach (GameObject cart in carts) {
					if (intersection.GetComponent<ML> ().loc_x == cart.GetComponent<Cart> ().current_x &&
					    intersection.GetComponent<ML> ().loc_y == cart.GetComponent<Cart> ().current_y) {
						intersection.GetComponent<ML> ().nets [genome].AddCost (500);
					}
				}
			}
			}
			catch {}
			NextGenome ();

		}
		detecting_deadlock = false;

	}

	void NextGenome()
	{
		Random.InitState ((int)69);
		GameObject[] carts = GameObject.FindGameObjectsWithTag ("Cart");
		foreach (GameObject cart in carts) {
			Destroy (cart);
		}
		bool new_gene = false;
		Debug.Log ("Next Genome!");
		genome++;
		if (genome == MAX_GENOMES) {
			new_gene = true;
			generation++;
			genome = 0;
		}
		GameObject[] tunnels = GameObject.FindGameObjectsWithTag ("Tunnel");
		foreach (GameObject tunnel in tunnels) {
			Spawner spawner = tunnel.GetComponentInChildren<Spawner>();
			if (spawner != null) {
				spawner.NewRound();
			}
		}

		GameObject[] intersections = GameObject.FindGameObjectsWithTag ("Intersection");
		foreach (GameObject intersection in intersections) {
			ML ml = intersection.GetComponent<ML> ();
			if (ml != null) {
				ml.cost = 0;
				if (new_gene == true) {
					ml.NewGeneration ();
				}
			}
		}
		new_gene = false;
	}
}
