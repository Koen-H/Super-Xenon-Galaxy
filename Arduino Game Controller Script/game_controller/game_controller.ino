#include <Keyboard.h>

const int spacePin = 8;//spacebar

const int colorPinOne = 3;//Color =
const int colorPinTwo = 7;//Color = NOTE: CHANGE THIS PIN NUMBER TO 4 ON WORKING CONTROLLER! Mine is 7
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


const int       RIGHT_LED_PIN = 10;
const int      MIDDLE_LED_PIN = 11;
const int        LEFT_LED_PIN = 12;


void setup() {
  // put your setup code here, to run once:
  pinMode(spacePin, INPUT_PULLUP );
  pinMode(colorPinOne, INPUT_PULLUP );
  pinMode(colorPinTwo, INPUT_PULLUP );
  pinMode(colorPinThree, INPUT_PULLUP );
  pinMode(colorPinFour, INPUT_PULLUP );
  pinMode(RIGHT_LED_PIN, OUTPUT);
  pinMode(MIDDLE_LED_PIN, OUTPUT);
  pinMode(LEFT_LED_PIN, OUTPUT);
  digitalWrite( RIGHT_LED_PIN, LOW);
  digitalWrite( MIDDLE_LED_PIN, LOW);
  digitalWrite( LEFT_LED_PIN, LOW);

  Serial.begin(9600);

  Keyboard.begin();
  
}

void loop() {
  // put your main code here, to run repeatedly:
 String message;
  while(Serial.available() > 0)
  {
    char c = Serial.read();
    message.concat(c);
  }
  
  if(message.indexOf("CONNECT CONTROLLER") >= 0)
  {
    String s = "CONNECTED";
    Serial.println(s);
  }
  
  if(message.indexOf("SEND") >= 0)
  {
    String s;
    //Just concatenate everything you want to send, with a space in between
    s.concat(analogRead(A1)- 512);
    s.concat(" ");
    s.concat(analogRead(A0)- 512);
    //After everything is done just send the data
    Serial.println(s);
  }
  if(message.indexOf("LAMP") >= 0)
  {
    digitalWrite( RIGHT_LED_PIN, HIGH);
    digitalWrite( MIDDLE_LED_PIN, HIGH);
    digitalWrite( LEFT_LED_PIN, HIGH);
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

void Flush(){
  while(Serial.available())
   char c=Serial.read();
}
