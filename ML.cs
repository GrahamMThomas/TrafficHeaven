using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ML : MonoBehaviour
{

	God god;
	public List<NeuralNetwork> nets;
	public float cost;
	Intersection int_controller;
	public int loc_x;
	public int loc_y;
	int old_genome;

	// Use this for initialization
	void Start ()
	{
		debug = false;
		old_genome = 0;
		god = GameObject.Find ("God").GetComponent<God> ();
		Time.timeScale = 10.0F;
		int_controller = this.GetComponent<Intersection> ();

		nets = new List<NeuralNetwork> ();

		for (int i = 0; i < god.MAX_GENOMES; i++) {
			NeuralNetwork net = new NeuralNetwork (new int[] { 12, 12, 11, 10, 8 });
			net.Mutate ();
			nets.Add (net);
		}
			
		StartCoroutine (ChangeState ());
	}

	void FixedUpdate ()
	{
		if (god.genome != old_genome) {
			old_genome = god.genome;
		}
	}

	void OnTriggerExit (Collider colid)
	{
		if (colid.gameObject.tag == "Cart") {
			nets [god.genome].AddCost (colid.GetComponent<Cart> ().wait_time);
			cost = nets [god.genome].GetCost ();
			colid.GetComponent<Cart> ().wait_time = 0;
		}
	}

	IEnumerator ChangeState ()
	{
		while (true) {
			List<float> inputs = new List<float> (new float[12]);
			List<GameObject> valid_carts = new List<GameObject> ();
			try {
				GameObject[] carts = GameObject.FindGameObjectsWithTag ("Cart");
				foreach (GameObject cart in carts) {
					if (cart.GetComponent<Cart> ().current_x == loc_x && cart.GetComponent<Cart> ().current_y == loc_y) {
						valid_carts.Add (cart);
					}
				}
			} catch {
			}
			foreach (GameObject cart_obj in valid_carts) {
				Cart cart = cart_obj.GetComponent<Cart> ();
				if (cart.dir == "South" && cart.next_move == "South") {
					inputs [0] += 0.1f;
				} else if (cart.dir == "South" && cart.next_move == "West") {
					inputs [1] += 0.1f;
				} else if (cart.dir == "South" && cart.next_move == "East") {
					inputs [2] += 0.1f;
				} else if (cart.dir == "North" && cart.next_move == "North") {
					inputs [3] += 0.1f;
				} else if (cart.dir == "North" && cart.next_move == "West") {
					inputs [4] += 0.1f;
				} else if (cart.dir == "North" && cart.next_move == "East") {
					inputs [5] += 0.1f;
				} else if (cart.dir == "East" && cart.next_move == "South") {
					inputs [6] += 0.1f;
				} else if (cart.dir == "East" && cart.next_move == "East") {
					inputs [7] += 0.1f;
				} else if (cart.dir == "East" && cart.next_move == "North") {
					inputs [8] += 0.1f;
				} else if (cart.dir == "West" && cart.next_move == "South") {
					inputs [9] += 0.1f;
				} else if (cart.dir == "West" && cart.next_move == "West") {
					inputs [10] += 0.1f;
				} else if (cart.dir == "West" && cart.next_move == "North") {
					inputs [11] += 0.1f;
				}
			}
			float highest_input = 0;
			foreach (float input in inputs) {
				if (input > highest_input) {
					highest_input = input;
				}
			}
			float scaling_factor = 1;
			if (highest_input != 0) {
				scaling_factor = 1 / highest_input;
			}
			for (int i = 0; i < inputs.Count; i++) {
				inputs [i] = inputs [i] * scaling_factor;
			}

			string input_out = "";
			foreach (float input in inputs) {
				input_out += input + " - ";
			}

			float[] outputs = nets [god.genome].FeedForward (inputs.ToArray ());
			int best_index = 0;
			for (int i = 1; i < outputs.Length; i++) {
				if (outputs [i] > outputs [best_index]) {
					best_index = i;
				}
			}
			// Best index between 0 and 7
			int_controller.current_state = best_index;
			yield return new WaitForSeconds (5f);
		}
	}

	public void NewGeneration ()
	{
		string cost_list = "";
		float cost_total = 0;
		nets.Sort ();
		foreach (NeuralNetwork net in nets) {
			cost_list += net.GetCost () + " - ";
			cost_total += net.GetCost ();
		}
		Debug.Log (cost_list + "    AVG: " + cost_total / god.MAX_GENOMES);

		string breed_net = "Breeding: ";
		int skip_zero = 0;
		foreach (NeuralNetwork net in nets) {
			if (net.GetCost () == 0) {
				skip_zero++;
			} else {
				break;
			}
		}

		for (int i = 0; i < god.MAX_GENOMES / 2; i++) {
			breed_net += nets [i + (god.MAX_GENOMES / 2)].GetCost () + " - ";
			nets [i] = new NeuralNetwork (nets [i + (god.MAX_GENOMES / 2)]);
			nets [i].Mutate ();

			nets [i + (god.MAX_GENOMES / 2)] = new NeuralNetwork (nets [i + (god.MAX_GENOMES / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
		}
		Debug.Log (breed_net);	
		for (int i = 0; i < god.MAX_GENOMES; i++) {
			nets [i].SetCost (0f);
		}
		nets = nets.OrderBy( x => Random.value ).ToList( );
	}
}
