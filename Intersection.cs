using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{

	//2 Straight States
	//2 Left Turn States
	//4 All one side States

	// State 0: North & South Straight
	// State 1: East & West Straight
	// State 2: North and South move to turn Left
	// State 3: East & West move to turn Left
	// State 4: North all lanes move
	// State 5: South all lanes move
	// State 6: East all lanes move
	// State 7: West all lanes move

	public bool n_str, n_left, n_right, s_str, s_left, s_right, 
				e_str, e_left, e_right,w_str, w_left, w_right;

	public Transform north_light;
	public Transform south_light;
	public Transform east_light;
	public Transform west_light;

	int old_state = -1;
	public int current_state = 0;
	// Use this for initialization
	void Start ()
	{
		foreach (Transform child in transform) {
			if (child.name == "TrafficPole_North") {
				foreach (Transform child_in_child in child) {
					if (child_in_child.name == "TrafficLight") {
						north_light = child_in_child.transform;
					}
				}
			}
			if (child.name == "TrafficPole_South") {
				foreach (Transform child_in_child in child) {
					if (child_in_child.name == "TrafficLight") {
						south_light = child_in_child.transform;
					}
				}
			}
			if (child.name == "TrafficPole_West") {
				foreach (Transform child_in_child in child) {
					if (child_in_child.name == "TrafficLight") {
						west_light = child_in_child.transform;
					}
				}
			}
			if (child.name == "TrafficPole_East") {
				foreach (Transform child_in_child in child) {
					if (child_in_child.name == "TrafficLight") {
						east_light = child_in_child.transform;
					}
				}
			}
		}
		// StartCoroutine (SequentialSwitch());
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (current_state != old_state) {
			StartCoroutine (SwitchState (current_state));
			old_state = current_state;
		}
	}

	IEnumerator SequentialSwitch() 
	{
		while (true) {
			yield return new WaitForSeconds (5f);
			current_state = (current_state + 1) % 8;
		}
	}

	IEnumerator SwitchState (int state)
	{
		
		foreach (Transform light in new Transform[] {north_light, south_light, east_light, west_light}) {
			if (!light.GetComponent<TrafficLight> ().red_light.gameObject.activeSelf) {
				light.GetComponent<TrafficLight> ().TurnRed ();
			}
		}
		yield return new WaitForSeconds (3f);
		n_str = n_left = n_right = s_str = s_left = s_right = e_str = e_left = e_right = w_str = w_left = w_right = false;
		yield return new WaitForSeconds (1.5f);
		switch (state) 
		{
		case 0:
			north_light.GetComponent<TrafficLight> ().TurnGreen ();
			south_light.GetComponent<TrafficLight> ().TurnGreen ();
			n_str = n_right = s_str = s_right = true;
			break;
		case 1:
			east_light.GetComponent<TrafficLight> ().TurnGreen ();
			west_light.GetComponent<TrafficLight> ().TurnGreen ();
			e_str = e_right = w_str = w_right = true;
			break;
		case 2:
			north_light.GetComponent<TrafficLight> ().TurnGreen ();
			south_light.GetComponent<TrafficLight> ().TurnGreen ();
			n_left = w_right = s_left = e_right = true;
			break;
		case 3:
			east_light.GetComponent<TrafficLight> ().TurnGreen ();
			west_light.GetComponent<TrafficLight> ().TurnGreen ();
			w_left = n_right = e_left = s_right = true;
			break;
		case 4:
			north_light.GetComponent<TrafficLight> ().TurnGreen ();
			n_str = n_left = n_right = true;
			break;
		case 5:
			south_light.GetComponent<TrafficLight> ().TurnGreen ();
			s_str = s_left = s_right = true;
			break;
		case 6:
			east_light.GetComponent<TrafficLight> ().TurnGreen ();
			e_str = e_left = e_right = true;
			break;
		case 7:
			west_light.GetComponent<TrafficLight> ().TurnGreen ();
			w_str = w_left = w_right = true;
			break;
		}

	}
}
