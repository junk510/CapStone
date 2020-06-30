using System.Collections;
using System.IO.Ports;
using UnityEngine;

public class joyStick : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    int val = 0;
    int acc = 5;
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();  //시리얼 포트를 연다.
        sp.ReadTimeout = 1; //시리얼 타임아웃 설정
    }

    // Update is called once per frame
    void Update()
    {
        if(sp.IsOpen)
        {
            try
            {
                sp.Write("s");  //데이터 보낸다
                val = sp.ReadByte();    //1바이트 읽어온다
                Debug.Log(val); //읽어온걸 체크한다.
                if((val & 0x01) == 0x01)    //Move val
                {
                    transform.Translate(Vector3.back * Time.deltaTime * acc);
                }
                else if((val & 0x02) == 0x02)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * acc);
                }
                else if((val & 0x04) == 0x04)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * acc);
                }
                else if((val & 0x08) == 0x08)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * acc);
                }
            } catch(System.Exception)
            {

            }
        }
    }
}
