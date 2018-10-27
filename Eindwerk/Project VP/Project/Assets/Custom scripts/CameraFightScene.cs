using UnityEngine;
using System.Collections;

public class CameraFightScene : MonoBehaviour {
	
	public Transform player1; //player 1 that we have to follow
	public Transform player2; //player 2 that we have to follow
	private float Xpos1;
	private float Xpos2;
	private float Xmiddelpunt;
	private float Ypos1;
	private float Ypos2;
	private float Ymiddelpunt;
	private float Overstaande;
	private float Zpos;
	public float minZpos;
	public float maxZpos;
	public float smoothtime;
	private Vector3 newPos;
	private Vector3 thisPos;
	private Transform myT;
	
	
	
	
	// Use this for initialization
	void Start () {
	//transform cachen
		myT= this.transform;
		CalculateCamera();
		myT.position = newPos;
		
	}
	
	// Update is called once per frame
	void Update () {
		CalculateCamera();
		
		Xmiddelpunt= Mathf.Lerp(myT.position.x, Xmiddelpunt,smoothtime*Time.deltaTime);
		Zpos= Mathf.Lerp(myT.position.z, Zpos, smoothtime*Time.deltaTime);
		newPos= new Vector3(Xmiddelpunt,transform.position.y,Zpos);
		
		myT.position = newPos;
	}
	
	void CalculateCamera()
	{
		Xpos1= player1.position.x;
		Xpos2= player2.position.x;
		Xmiddelpunt= (Xpos1+Xpos2)/2;
		if(Xpos1>Xpos2)
			Overstaande=Xmiddelpunt-Xpos2;
		else
			Overstaande=Xmiddelpunt-Xpos1;
		
		Zpos=player1.position.z-(Overstaande/(Mathf.Tan((transform.camera.fieldOfView*180/Mathf.PI)/2)));
		
		if(Zpos>minZpos)
		{
			Zpos=minZpos;
		}
		else
		{
			if(Zpos<maxZpos)
			{
				Zpos=maxZpos;
			}
		}
		newPos= new Vector3(Xmiddelpunt,transform.position.y,Zpos);
		
	}
}
