/*  PSX Controller Decoder Library (Psx.pde)
	Written by: Kevin Ahrendt June 22nd, 2008
	
	Controller protocol implemented using Andrew J McCubbin's analysis.
	http://www.gamesx.com/controldata/psxcont/psxcont.htm
	
	Shift command is based on tutorial examples for ShiftIn and ShiftOut
	functions both written by Carlyn Maw and Tom Igoe
	http://www.arduino.cc/en/Tutorial/ShiftIn
	http://www.arduino.cc/en/Tutorial/ShiftOut

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#include <Psx.h>                                          // Includes the Psx Library 

#define dataPin 2
#define cmndPin 3
#define attPin 4
#define clockPin 5
#define LEDPin 13


Psx Psx;                                                  // Initializes the library

unsigned int data = 0;                                    // data stores the controller response
char button;


void setup()
{
  Psx.setupPins(dataPin, cmndPin, attPin, clockPin, 10);  // Defines what each pin is used
                                                          // (Data Pin #, Cmnd Pin #, Att Pin #, Clk Pin #, Delay)
                                                          // Delay measures how long the clock remains at each state,
                                                          // measured in microseconds.
 Serial.begin(9600);
                                                         
}

void loop()
{
  data = Psx.read();                                     // Psx.read() initiates the PSX controller and returns
                                                          // the button data    
  if(data & psxLeft)
  {
  Serial.write("left");
  delay(300);
  }  
  else
  {
   if(data & psxRight)
    {
    Serial.write("right");
    delay(300);
    }
     else
     {
       if(data & psxUp)
        {
        Serial.write("up");
        delay(300);
        }
         else
         {
           if(data & psxSqu)
            {
            Serial.write("square");
            delay(300);
            }
             else
             {
               if(data & psxX)
                {
                Serial.write("X");
                delay(300);
                }
                else
               {
                 if(data & psxTri)
                  {
                  Serial.write("triangle");
                  delay(300);
                  }
                  else
                  {
                   if(data & psxDown)
                    {
                    Serial.write("down");
                    delay(300);
                    }
                    else
                    {
                     if(data & psxO)
                      {
                      Serial.write("O");
                      delay(3 00);
                      }
                   }
                 }
               }
             }
         }
     }
 }


}
