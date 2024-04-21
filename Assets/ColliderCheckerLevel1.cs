using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class ColliderCheckerLevel1 : MonoBehaviour {

	// public float fuseTime;

	

	public bool restartScene = false; //initially restart sceen false until no collision
	public GameObject carObject; //referencing the car

	void OnTriggerEnter(Collider collider) {
	
		restartScene = true; //If collided then restart scene set to true 
	}

	void FixedUpdate(){
		if (restartScene) {
			carObject.transform.position = new Vector3 (12, 1, 17); //start line position of the tank 
			carObject.transform.rotation = new Quaternion(0,0,0,0); //rotation manager

			restartScene = false;
			EventManager.TriggerEvent ("gameover");
		}
	}
	
		
}
