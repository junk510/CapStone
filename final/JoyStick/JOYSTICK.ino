int VRX = A0;
int VRY = A1;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  char val_pass = 0x00;
  int val_x = analogRead(VRX);
  int val_y = analogRead(VRY);
  if(val_x > 768)
  {
    val_pass += 1;
  }
  else if(val_x < 256)
  {
    val_pass += 2;
  }
  if(val_y > 768)
  {
    val_pass += 4; 
  }
  else if(val_y < 256)
  {
    val_pass += 8;
  }
  if(Serial.available())
  {
    if(Serial.read() == 's') {
      Serial.write(val_pass);
      Serial.flush();
    }
  }
}
