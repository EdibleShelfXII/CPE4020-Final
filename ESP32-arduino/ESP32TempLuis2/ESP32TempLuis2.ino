// Load Wi-Fi library
#include <WiFi.h>
#include <HTTPClient.h>

// Replace with your network credentials
const char* ssid = "North Gwinnett Robotics"; //Replace with your SSID
const char* password = "ngrobo1771"; //Replace with your PW

//Your Domain name with URL path or IP address with path
String serverName = "http://10.0.0.185:2000/";
String userID = "luis2";

// the following variables are unsigned longs because the time, measured in
// milliseconds, will quickly become a bigger number than can be stored in an int.
unsigned long lastTime = 0;
// Timer set to 10 seconds (10000)
//unsigned long timerDelay = 10000;
// Set timer to 5 seconds (5000)
unsigned long timerDelay = 5000;

// Set web server port number to 80
WiFiServer server(80);

// Variable to store the HTTP request
String header;


// Current time
unsigned long currentTime = millis();
// Previous time
unsigned long previousTime = 0; 
// Define timeout time in milliseconds (example: 2000ms = 2s)
const long timeoutTime = 2000;

unsigned char testVar = 1771;

const int tmpPin = 34;

int tmpVal;
float tmpPct;
float volts;
float mVolts;
float tempC;
float tempF;
char tempStr[10];


void setup() {
  Serial.begin(115200);

  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();
}
void loop() {
  // put your main code here, to run repeatedly:

  tmpVal = analogRead(tmpPin);
  volts = ((tmpVal * 3.8) / 4095) - 0.5;
  mVolts = volts * 1000;
  tempC = mVolts / 10;
  tempF = ((tempC * 9) / 5) + 32;
  Serial.printf("temp: %.2f deg F\n", tempF);
  sprintf(tempStr, "%.2f", tempF);

  //Send an HTTP POST request every 10 sec
  if ((millis() - lastTime) > timerDelay) {
    //Check WiFi connection status
    if(WiFi.status()== WL_CONNECTED){
      HTTPClient http;

      String serverPath = serverName + "?data=" + tempStr + "&user=" + userID;
      
      // Your Domain name with URL path or IP address with path
      http.begin(serverPath.c_str());

      // Send HTTP GET request
      int httpResponseCode = http.GET();
      
      if (httpResponseCode>0) {
        Serial.print("HTTP Response code: ");
        Serial.println(httpResponseCode);
        String payload = http.getString();
        Serial.println(payload);
      }
      else {
        Serial.print("Error code: ");
        Serial.println(httpResponseCode);
      }
      // Free resources
      http.end();
    }
    else {
      Serial.println("WiFi Disconnected");
    }
    lastTime = millis();
  }

}
