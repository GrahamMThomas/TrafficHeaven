using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour {
    public Transform red_light;
    public Transform yellow_light;
    public Transform green_light;

    bool is_yellow = false;
    // Use this for initialization
    void Start () {
        // Start light off red.
        red_light.gameObject.SetActive(true);
        yellow_light.gameObject.SetActive(false);
        green_light.gameObject.SetActive(false);
    }

    // Actually turns red after three seconds
    IEnumerator TurnYellow()
    {
        is_yellow = true;
        yield return new WaitForSeconds(3f);
        is_yellow = false;
        red_light.gameObject.SetActive(true);
        yellow_light.gameObject.SetActive(false);
        green_light.gameObject.SetActive(false);
    }

    //Actually turns yellow immediately, then calls the
    //  turn yellow method to actually turn it red. 
    public void TurnRed()
    {
        StartCoroutine(TurnYellow());
        red_light.gameObject.SetActive(false);
        yellow_light.gameObject.SetActive(true);
        green_light.gameObject.SetActive(false);
    }

    public void TurnGreen()
    {
        if (!is_yellow)
        {
            red_light.gameObject.SetActive(false);
            yellow_light.gameObject.SetActive(false);
            green_light.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Tried to turn green while light was yellow");
        }
    }
}
