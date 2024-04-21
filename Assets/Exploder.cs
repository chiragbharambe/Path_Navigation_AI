using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {
 
 public GameObject explosion; 
 //public GameObject crater;
 
 
 
 void OnTriggerEnter(Collider  collider) {
 
 //if (other.gameObject.CompareTag("Player"))
//{

 Instantiate(explosion, transform.position, transform.rotation); 

 GetComponent<AudioSource>().Play (); 

//Destroy(other.gameObject);
 
 //Instantiate(crater, transform.position, transform.rotation);

 //}

 }
 
}