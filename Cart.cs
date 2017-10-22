using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
	public float speed = 0;
	float MAX_SPEED = 40;
	float acceleration = 0.5f;
	public bool stopping = false;
	bool initialized = false;

	public float wait_time;

	// Turning Stuff
	float distance_before_turn;
	public Vector3 old_pos;
	string next_dir = "";
	public bool moving_intersection = false;

	public int current_x = 1;
	public int current_y = 1;

	public string dir = "South";
	string old_dir = "South";

	public int goal_x;
	public int goal_y;
	public string next_move = null;
	public BoxCollider front_bumper;
	List<List<int>> map;

	class Coord
	{
		public int x;
		public int y;

		public Coord (int x_coord, int y_coord)
		{
			x = x_coord;
			y = y_coord;
		}
	}

	void Start()
	{
		// Initialize (1, 1, "South");
		wait_time = 0;
	}

	// Use this for initialization
	public void Initialize (int curr_x, int curr_y, string face_dir)
	{
		current_x = curr_x;
		current_y = curr_y;
		dir = face_dir;
		List<Coord> Goals = new List<Coord> ();
		//Coord start_pos = new Coord (1, 0);
		map = new List<List<int>> ();
		map.Add (new List<int> (new int[] { 0, 2, 0, 0, 0 }));
		map.Add (new List<int> (new int[] { 0, 1, 1, 2, 0 }));
		map.Add (new List<int> (new int[] { 2, 1, 1, 1, 0 }));
		map.Add (new List<int> (new int[] { 0, 1, 1, 1, 2 }));
		map.Add (new List<int> (new int[] { 2, 1, 1, 0, 0 }));
		map.Add (new List<int> (new int[] { 0, 2, 2, 0, 0 }));

		for (int i = 0; i < map.Count; i++) {
			for (int j = 0; j < map [0].Count; j++) {
				if (map [i] [j] == 2 && ((Mathf.Abs(current_y - i) + Mathf.Abs(current_x - j)) > 1)) {
					Goals.Add (new Coord (j, i));
				}
			}
		}
		int randnum = Random.Range (0, Goals.Count);
		goal_x = Goals [randnum].x;
		goal_y = Goals [randnum].y;
		SetNextMove ();
		initialized = true;
	}

	public void SetNextMove ()
	{	
		List<string> moves = new List<string> ();
		if (current_x - goal_x < 0 && map [current_y] [current_x + 1] != 0 && (map [current_y] [current_x + 1] != 2 || (current_y == goal_y && current_x + 1 == goal_x))) {
			moves.Add ("East");
		}
		if (current_x - goal_x > 0 && map [current_y] [current_x - 1] != 0 && (map [current_y] [current_x - 1] != 2 || (current_y == goal_y && current_x - 1 == goal_x))) {
			moves.Add ("West");
		}
		if (current_y - goal_y < 0 && map [current_y + 1] [current_x] != 0 && (map [current_y + 1] [current_x] != 2 || (current_y + 1 == goal_y && current_x == goal_x))) {
			moves.Add ("South");
		}
		if (current_y - goal_y > 0 && map [current_y - 1] [current_x] != 0 && (map [current_y - 1] [current_x] != 2 || (current_y - 1 == goal_y && current_x == goal_x))) {
			moves.Add ("North");
		}
		if (moves.Count == 0 && (current_x != goal_x || current_y != goal_y)) {
			Debug.LogError ("Cannot pick next move");
		} else if (current_x == goal_x && current_y == goal_y) {
			next_move = "Goal";
		} else {
			next_move = moves [Random.Range (0, moves.Count)];
		}
	}

	void OnTriggerStay (Collider colid)
	{
		if (!colid.isTrigger) {
			stopping = true;
		}
	}

	public void Turn (float p_distance_before_turn)
	{
		if (next_move == "East") {
			current_x++;
		}
		if (next_move == "West") {
			current_x--;
		}
		if (next_move == "North") {
			current_y--;
		}
		if (next_move == "South") {
			current_y++;
		}
		next_dir = next_move;
		distance_before_turn = p_distance_before_turn;
	}

	void OnTriggerExit (Collider colid)
	{
		stopping = false;
		if (colid.gameObject.tag == "Tunnel" && next_move == "Goal") {
			
			Destroy (this.gameObject);
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (stopping) {
			wait_time += Time.fixedDeltaTime;
		}
		if (this.transform.position.x < -400 || this.transform.position.x > 800 || this.transform.position.z > 300 || this.transform.position.z < -1200) {
			Destroy (this.gameObject);
		}
		if (moving_intersection) {
			if (Mathf.Abs (Vector3.Distance (old_pos, this.transform.position)) > distance_before_turn) {
				dir = next_dir;
			}
		}
		if (old_dir != dir) {
			if (dir == "South") {
				this.transform.localEulerAngles = new Vector3 (0, 90, 90);
			} else if (dir == "North") {
				this.transform.localEulerAngles = new Vector3 (0, 270, 90);
			} else if (dir == "East") {
				this.transform.localEulerAngles = new Vector3 (0, 0, 90);
			} else if (dir == "West") {
				this.transform.localEulerAngles = new Vector3 (0, 180, 90);
			}
			old_dir = dir;
		}

		if (!stopping && speed < MAX_SPEED && initialized) {
			speed += acceleration;
		} else if (stopping) {
			speed = 0;
		}
		if (dir == "South") {
			this.transform.position += Vector3.back * Time.deltaTime * speed;
		}
		if (dir == "North") {
			this.transform.position += Vector3.forward * Time.deltaTime * speed;
		}
		if (dir == "East") {
			this.transform.position += Vector3.right * Time.deltaTime * speed;
		}
		if (dir == "West") {
			this.transform.position += Vector3.left * Time.deltaTime * speed;
		}
	}


}
