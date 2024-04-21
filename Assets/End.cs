using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {

 //EventManager.TriggerEvent ("gameover");
 Debug.Log("Thanks for playing");
 }
}
