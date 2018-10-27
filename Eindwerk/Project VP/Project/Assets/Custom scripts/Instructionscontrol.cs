using UnityEngine;
using System.Collections;

public class Instructionscontrol : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("Punch1"))
		{
			Application.LoadLevel("Menu");
		}
	}
}
