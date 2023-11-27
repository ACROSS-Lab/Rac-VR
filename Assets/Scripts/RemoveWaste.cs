using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWaste : MonoBehaviour
{
	[SerializeField] private float durationSec = 120.0f;


	private float currentTime = 0.0f;
    	private bool timerOn = false;


	void reset() {
		GetComponent<BoxCollider>().enabled = true;
		GetComponent<Renderer>().enabled = true;
		GetComponent<PlayMakerFSM>().enabled = false;	
		timerOn = false;
	}
	public void remove() {
		GetComponent<BoxCollider>().enabled = false;
		GetComponent<Renderer>().enabled = false;	
		currentTime = durationSec;
		timerOn = true;
	} 

    // Update is called once per frame
    void Update()
    {
	if (timerOn) {

		if (currentTime > 0.0f) {
        		currentTime -= Time.deltaTime;
		} else {
			reset();
		}
	}
    }
}
