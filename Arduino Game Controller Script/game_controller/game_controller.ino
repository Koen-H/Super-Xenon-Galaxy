#include <Keyboard.h>

const int spacePin = 8;//spacebar

const int analogY = A0;//Analog stick Y   
const int analogX = A1;//Analog stick X                   

int spaceButton = 0;
boolean buttonIsPressed, analogXPressed, analogYPressed;

void setup() {
  // put your setup code here, to run once:
  pinMode(spacePin, INPUT_PULLUP );//spacebar


  Serial.begin(9600);
  // initialize mouse control:
  Keyboard.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
  spaceButton = !digitalRead(spacePin);
  Serial.println(analogRead(analogX)); 
  

  //analogY
  if(analogRead(analogY)< 100){
    Keyboard.press('w');
  }
  else{
    Keyboard.release('w');
  }
  if(analogRead(analogY)> 900){
    Keyboard.press('s');
  }
  else{
    Keyboard.release('s');
  }


   //analogY
  if(analogRead(analogX)< 100){
    Keyboard.press('a');
  }
  else{
    Keyboard.release('a');
  }
  if(analogRead(analogX)> 900){
    Keyboard.press('d');
  }
  else{
    Keyboard.release('d');
  }


  
  //button 1
  if (spaceButton == HIGH && !buttonIsPressed) {
     buttonIsPressed = true;
     Serial.println("pressed"); 
  }
  if(spaceButton == LOW && buttonIsPressed){
    buttonIsPressed = false;
    Serial.println("not pressed"); 
    Keyboard.write(' ');
  }
}
