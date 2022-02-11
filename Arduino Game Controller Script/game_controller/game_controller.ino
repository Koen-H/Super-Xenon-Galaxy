#include <Keyboard.h>

const int spacePin = 8;//spacebar

const int colorPinOne = 3;//Color =
const int colorPinTwo = 4;//Color = NOTE: CHANGE THIS PIN NUMBER TO 4 ON WORKING CONTROLLER!
const int colorPinThree = 5;//Color =
const int colorPinFour = 6;//Color =
boolean colorPinOnePressed, colorPinTwoPressed, colorPinThreePressed, colorPinFourPressed;
int colorButtonOne = 0;
int colorButtonTwo = 0;
int colorButtonThree= 0;
int colorButtonFour = 0; 

const int analogY = A0;//Analog stick Y   
const int analogX = A1;//Analog stick X                   

int spaceButton = 0;
boolean buttonIsPressed;

void setup() {
  // put your setup code here, to run once:
  pinMode(spacePin, INPUT_PULLUP );
  pinMode(colorPinOne, INPUT_PULLUP );
  pinMode(colorPinTwo, INPUT_PULLUP );
  pinMode(colorPinThree, INPUT_PULLUP );
  pinMode(colorPinFour, INPUT_PULLUP );


  Serial.begin(9600);

  Keyboard.begin();
}

void loop() {
  // put your main code here, to run repeatedly:

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


  spaceButton = !digitalRead(spacePin);
  
  //button 1
  if (spaceButton == HIGH && !buttonIsPressed) {
     buttonIsPressed = true;
     
  }
  if(spaceButton == LOW && buttonIsPressed){
    buttonIsPressed = false;
    Keyboard.write(' ');
  }

//All colors
  colorButtonOne = !digitalRead(colorPinOne);
  colorButtonTwo =  !digitalRead(colorPinTwo);
  colorButtonThree =  !digitalRead(colorPinThree);
  colorButtonFour = !digitalRead(colorPinFour);
  //color 1
  if (colorButtonOne == HIGH) {
     colorPinOnePressed = true;
  }
  if(colorButtonOne == LOW && colorPinOnePressed){
      colorPinOnePressed = false;
    Keyboard.write(KEY_LEFT_ARROW);
  }

  //color 2
  if (colorButtonTwo == HIGH) {
     colorPinTwoPressed = true;
  }
  if(colorButtonTwo == LOW && colorPinTwoPressed){
      colorPinTwoPressed = false;
    Keyboard.write(KEY_UP_ARROW);
  }

  //color 3
  if (colorButtonThree == HIGH) {
     colorPinThreePressed = true;
  }
  if(colorButtonThree == LOW && colorPinThreePressed){
      colorPinThreePressed = false;
    Keyboard.write(KEY_DOWN_ARROW);
  }

  // color 4
  if (colorButtonFour == HIGH) {
     colorPinFourPressed = true;
  }
  if(colorButtonFour == LOW && colorPinFourPressed){
      colorPinFourPressed = false;
    Keyboard.press(KEY_RIGHT_ARROW);
    Keyboard.release(KEY_RIGHT_ARROW);
  }
  

  
}
