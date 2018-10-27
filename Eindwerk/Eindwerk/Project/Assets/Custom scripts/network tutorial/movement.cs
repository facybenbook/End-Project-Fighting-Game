using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	
	public float speed= 5.0f;
	public float gravity=5.0f;
	private float horizontal;
	private float vertical;
	private CharacterController cc;
	
	// Use this for initialization
	void Start () {
	cc= (CharacterController)GetComponent("CharacterController");
	}
	
	// Update is called once per frame
	void Update () {
	horizontal=Input.GetAxis("Horizontal1");
	vertical= Input.GetAxis("Vertical1");
	if(networkView.isMine)
		{
		cc.Move(new Vector3(horizontal*speed* Time.deltaTime, -gravity*Time.deltaTime,vertical*speed*Time.deltaTime));
		}
		else
		{
			enabled=false;
		}
	}
}
