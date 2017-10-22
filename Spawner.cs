using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public int spawn_x;
	public int spawn_y;
	public string spawn_dir;

	public Transform Cart;

	void Start()
	{
		NewRound ();	
	}

	public void NewRound()
	{
		StartCoroutine (ContinuousSpawn());
	}

	// Use this for initialization
	void SpawnCart ()
	{
		Transform cart_obj = Instantiate (Cart, this.transform.position, this.transform.rotation);
		Cart cart = cart_obj.GetComponent<Cart> ();
		cart.Initialize (spawn_x, spawn_y, spawn_dir);
		if (cart.dir == "South" && cart.next_move == "South") {

		} else if (cart.dir == "South" && cart.next_move == "West") {
			cart.transform.position = new Vector3 (cart.transform.position.x - 9, cart.transform.position.y, cart.transform.position.z);
		} else if (cart.dir == "South" && cart.next_move == "East") {
			cart.transform.position = new Vector3 (cart.transform.position.x + 9, cart.transform.position.y, cart.transform.position.z);
		} else if (cart.dir == "North" && cart.next_move == "North") {

		} else if (cart.dir == "North" && cart.next_move == "West") {
			cart.transform.position = new Vector3 (cart.transform.position.x - 9, cart.transform.position.y, cart.transform.position.z);
		} else if (cart.dir == "North" && cart.next_move == "East") {
			cart.transform.position = new Vector3 (cart.transform.position.x + 9, cart.transform.position.y, cart.transform.position.z);

		} else if (cart.dir == "East" && cart.next_move == "South") {
			cart.transform.position = new Vector3 (cart.transform.position.x, cart.transform.position.y, cart.transform.position.z - 9);
		} else if (cart.dir == "East" && cart.next_move == "East") {

		} else if (cart.dir == "East" && cart.next_move == "North") {
			cart.transform.position = new Vector3 (cart.transform.position.x, cart.transform.position.y, cart.transform.position.z + 9);

		} else if (cart.dir == "West" && cart.next_move == "South") {
			cart.transform.position = new Vector3 (cart.transform.position.x, cart.transform.position.y, cart.transform.position.z - 9);
		} else if (cart.dir == "West" && cart.next_move == "West") {

		} else if (cart.dir == "West" && cart.next_move == "North") {
			cart.transform.position = new Vector3 (cart.transform.position.x, cart.transform.position.y, cart.transform.position.z + 9);
		}
	}

	IEnumerator ContinuousSpawn()
	{
		for (int i = 0; i < 10; i++) {
			SpawnCart ();
			yield return new WaitForSeconds (3f);
		}
	}
}
