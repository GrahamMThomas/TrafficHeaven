using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection_CarControl : MonoBehaviour
{

	Intersection intersection;
	// Use this for initialization
	void Start ()
	{
		intersection = this.GetComponent<Intersection> ();
	}

	void OnTriggerEnter (Collider colid)
	{
		Cart cart = colid.GetComponent<Cart> ();
		cart.old_pos = cart.transform.position;
		if (colid.gameObject.tag == "Cart" && !colid.isTrigger && !cart.moving_intersection) {
			cart.stopping = true;
		}
	}

	void OnTriggerStay (Collider colid)
	{
		Cart cart = colid.GetComponent<Cart> ();
		if (!cart.moving_intersection) {
			if (cart.dir == "South" && cart.next_move == "South" && intersection.n_str) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (22);
				cart.SetNextMove ();
			} else if (cart.dir == "South" && cart.next_move == "West" && intersection.n_right) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (22);
				cart.SetNextMove ();
			} else if (cart.dir == "South" && cart.next_move == "East" && intersection.n_left) {
				cart.stopping = false;
				cart.Turn (47);
				cart.SetNextMove ();
				cart.moving_intersection = true;




			} else if (cart.dir == "North" && cart.next_move == "North" && intersection.s_str) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (22);
				cart.SetNextMove ();
			} else if (cart.dir == "North" && cart.next_move == "West" && intersection.s_left) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (48);
				cart.SetNextMove ();
			} else if (cart.dir == "North" && cart.next_move == "East" && intersection.s_right) {
				cart.stopping = false;
				cart.Turn (27);
				cart.SetNextMove ();
				cart.moving_intersection = true;




			} else if (cart.dir == "East" && cart.next_move == "South" && intersection.w_right) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (27);
				cart.SetNextMove ();
			} else if (cart.dir == "East" && cart.next_move == "East" && intersection.w_str) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (22);
				cart.SetNextMove ();
			} else if (cart.dir == "East" && cart.next_move == "North" && intersection.w_left) {
				cart.stopping = false;
				cart.Turn (48);
				cart.SetNextMove ();
				cart.moving_intersection = true;




			} else if (cart.dir == "West" && cart.next_move == "South" && intersection.e_left) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (48);
				cart.SetNextMove ();
			} else if (cart.dir == "West" && cart.next_move == "West" && intersection.e_str) {
				cart.stopping = false;
				cart.moving_intersection = true;
				cart.Turn (22);
				cart.SetNextMove ();
			} else if (cart.dir == "West" && cart.next_move == "North" && intersection.e_right) {
				cart.stopping = false;
				cart.Turn (25);
				cart.SetNextMove ();
				cart.moving_intersection = true;
			} else {
			}
		}
	}
	void OnTriggerExit (Collider colid)
	{
		if (!colid.isTrigger) {
			Cart cart = colid.GetComponent<Cart> ();
			StartCoroutine (SetMoving (cart));
			if (cart.dir == "South" && cart.next_move == "South") {

			} else if (cart.dir == "South" && cart.next_move == "West") {
				cart.transform.position = new Vector3 ( cart.transform.position.x - 9, cart.transform.position.y, cart.transform.position.z);
			} else if (cart.dir == "South" && cart.next_move == "East") {
				cart.transform.position = new Vector3  ( cart.transform.position.x + 9, cart.transform.position.y, cart.transform.position.z);
			} else if (cart.dir == "North" && cart.next_move == "North") {

			} else if (cart.dir == "North" && cart.next_move == "West") {
				cart.transform.position = new Vector3  ( cart.transform.position.x - 9, cart.transform.position.y, cart.transform.position.z);
			} else if (cart.dir == "North" && cart.next_move == "East") {
				cart.transform.position = new Vector3  ( cart.transform.position.x + 9, cart.transform.position.y, cart.transform.position.z);

			} else if (cart.dir == "East" && cart.next_move == "South") {
				cart.transform.position = new Vector3  ( cart.transform.position.x, cart.transform.position.y, cart.transform.position.z - 9);
			} else if (cart.dir == "East" && cart.next_move == "East") {
			
			} else if (cart.dir == "East" && cart.next_move == "North") {
				cart.transform.position = new Vector3  ( cart.transform.position.x, cart.transform.position.y, cart.transform.position.z + 9);

			} else if (cart.dir == "West" && cart.next_move == "South") {
				cart.transform.position = new Vector3  ( cart.transform.position.x, cart.transform.position.y, cart.transform.position.z - 9);
			} else if (cart.dir == "West" && cart.next_move == "West") {
			
			} else if (cart.dir == "West" && cart.next_move == "North") {
				cart.transform.position = new Vector3  ( cart.transform.position.x, cart.transform.position.y, cart.transform.position.z + 9);
			}
		}
	}

	IEnumerator SetMoving (Cart cart)
	{
		yield return new WaitForSeconds (2f);
		cart.moving_intersection = false;
	}
}
