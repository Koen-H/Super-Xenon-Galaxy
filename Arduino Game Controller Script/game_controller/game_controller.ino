#include <Keyboard.h>

// 9 = yellow light
// 11 == cyan light

const int spacePin = 8;//spacebar
const int spacePinLed = 10;// Spacebar led

const int colorPinOne = 3;//Color = Orange
const int colorPinLedOne = 11;//
const int colorPinTwo = 6;//Color = NOTE: CHANGE THIS PIN NUMBER TO 4 ON WORKING CONTROLLER! Mine is 7
const int colorPinLedTwo = 9;
const int colorPinThree = 4;//Color =
const int colorPinLedThree = 12;
const int colorPinFour = 2;//Color = 
const int colorPinLedFour = 7;
boolean colorPinOnePressed, colorPinTwoPressed, colorPinThreePressed, colorPinFourPressed;
int colorButtonOne = 0;
int colorButtonTwo = 0;
int colorButtonThree= 0;
int colorButtonFour = 0; 

const int analogX = A2;//Analog stick X  
const int analogY = A3;//Analog stick Y   

                 

int spaceButton = 0;
boolean buttonIsPressed;


const int       RIGHT_LED_PIN = 10;
const int      MIDDLE_LED_PIN = 11;
const int        LEFT_LED_PIN = 12;


void setup() {
  // put your setup code here, to run once:
  pinMode(spacePin, INPUT_PULLUP );
  pinMode(spacePinLed, OUTPUT);
  pinMode(colorPinOne, INPUT_PULLUP );
  pinMode(colorPinLedOne, OUTPUT );
  pinMode(colorPinTwo, INPUT_PULLUP );
  pinMode(colorPinLedTwo, OUTPUT );
  pinMode(colorPinThree, INPUT_PULLUP );
  pinMode(colorPinLedThree, OUTPUT );
  pinMode(colorPinFour, INPUT_PULLUP );
  pinMode(colorPinLedFour, OUTPUT );
  pinMode(RIGHT_LED_PIN, OUTPUT);
  pinMode(MIDDLE_LED_PIN, OUTPUT);
  pinMode(LEFT_LED_PIN, OUTPUT);
  
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
    s.concat(analogRead(analogX)- 512);
    s.concat(" ");
    s.concat(analogRead(analogY)- 512);
    //After everything is done just send the data
    Serial.println(s);
  }
  
  if(message.indexOf("COLORS_OFF") >= 0){
    digitalWrite( colorPinLedOne, LOW);
    digitalWrite( colorPinLedTwo, LOW);
    digitalWrite( colorPinLedThree, LOW);
    digitalWrite( colorPinLedFour, LOW);
    
  }

  if(message.indexOf("LED_SPACE_ON") >= 0){
    digitalWrite( spacePinLed, HIGH);
  }
  if(message.indexOf("LED_ONE_ON") >= 0){
    digitalWrite( colorPinLedOne, HIGH);
  }
    if(message.indexOf("LED_TWO_ON") >= 0){
    digitalWrite( colorPinLedTwo, HIGH);
  }
    if(message.indexOf("LED_THREE_ON") >= 0){
    digitalWrite( colorPinLedThree, HIGH);
  }
    if(message.indexOf("LED_FOUR_ON") >= 0){
    digitalWrite( colorPinLedFour, HIGH);
  }


  if(message.indexOf("LED_SPACE_OFF") >= 0){
    digitalWrite( spacePinLed, HIGH);
  }
    if(message.indexOf("LED_ONE_OFF") >= 0){
    digitalWrite( colorPinLedOne, LOW);
  }
    if(message.indexOf("LED_TWO_OFF") >= 0){
    digitalWrite( colorPinLedTwo, LOW);
  }
    if(message.indexOf("LED_THREE_OFF") >= 0){
    digitalWrite( colorPinLedThree, LOW);
  }
    if(message.indexOf("LED_FOUR_OFF") >= 0){
    digitalWrite( colorPinLedFour, LOW);
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
