using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
public class arduino : MonoBehaviour
{
private SerialPort stream;
static public string arduinoValue;
private string newValue;
private float x;
static public double val;
// Use this for initialization
void Start ()
{
stream = new SerialPort ("COM3", 9600); //Set the port (com2) and the baud rate (9600, is standard on most devices)

//timeout van 300 gelijk aan arduino
stream.ReadTimeout = 50;
stream.Open (); //Open the Serial Stream.
newValue = "";

}
// Update is called once per frame
void Update ()
{
//arduinoValue
//stream.readline() ga wachten op /n en als die er niet komt => timeout
try
{
arduinoValue = stream.ReadLine(); //stream.ReadExisting();
}
catch (TimeoutException)
{
arduinoValue = Convert.ToString(setTimeToZero(arduinoValue));
// x = Convert.ToSingle(arduinoValue)/10;
// Vector3 pos = fiets.transform.position; // 'pos' is a copy
//
//
//
// pos.z +=x ; // modify the copy
//
//
//
// fiets.transform.position = pos; // Updates the original
}
}
void OnGUI ()
{
newValue = "Arduino says: " + arduinoValue;
GUI.Label (new Rect (10, 10, 300, 100), newValue); //Display new values
}
public string setTimeToZero(string value)
{
val = Convert.ToDouble(value);
if (val >= 2)
{
val -= 2;
}
else
{
val = 0;
}
return Convert.ToString(val);
}
}