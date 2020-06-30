using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    //float force = 10;
    float speed = 2;
    public Animator animator3;
    public Transform Ball;
    public Transform serve_position2;              //서브시 공의 위치
                                                   // public Transform aimTarget;

    //public Transform Net_B;               //안쓸것 같다.

    public Transform[] targets;         //aimTarget 3개 불러오기

    Vector3 targetPosition;
    public ShotManeger shotManager;            //ShotManager 스크립트 불러오기
    Shot currentShot3;


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;    //봇의 위치
        animator3 = GetComponent<Animator>();
        /////////////////////////////////////////
        shotManager = GetComponent<ShotManeger>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Y))  //flat서브 준비상태
        {
            //hit = true;
            currentShot3 = shotManager.flatServe;           // flat서브 준비
            GetComponent<BoxCollider>().enabled = false;  //R키 누르면 박스콜리더 효과 없앰
            Ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // 공의 힘을 먼저 없앤다
            Ball.transform.position = serve_position2.transform.position;  //서브준비상태로서 볼을 플레이어 쪽으로 가져온다



            //Ball.GetComponent<Rigidbody>().velocity = new UnityEngine.Vector3(0, 6, 0);            //공을 y축으로 3만큼 튀어오르게함
            Ball.GetComponent<Rigidbody>().useGravity = false; //공 멈추기(중력 없애기)
                                                               //StartCoroutine(ExampleCoroutine());                //1초 대기 함수


            animator3.Play("serve-prepare5");                 //서브준비상태
                                                             // animator.SetBool("pause", false);
            animator3.SetBool("leg_move", false);
        }

        if (Input.GetKeyDown(KeyCode.U))  //.kick서브 준비상태
        {
            //hit = true;
            currentShot3 = shotManager.kickServe;           // kick서브 준비
            GetComponent<BoxCollider>().enabled = false;  //R키 누르면 박스콜리더 효과 없앰
            Ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // 공의 힘을 먼저 없앤다
            Ball.transform.position = serve_position2.transform.position;//서브준비상태로서 볼을 플레이어 쪽으로 가져온다
            Ball.GetComponent<Rigidbody>().useGravity = false; //공 멈추기(중력없애기)

            animator3.Play("serve-prepare5");               //서브준비상태
            animator3.SetBool("leg_move", false);
        }


        if (Input.GetKeyUp(KeyCode.Y) || Input.GetKeyUp(KeyCode.U))       //서브 날리기~
        {
            //hit = false;
            GetComponent<BoxCollider>().enabled = true;                                                 //R키 떼면 박스콜리더 효과 살아남
            Ball.transform.position = serve_position2.transform.position;         //공의 위치를 플레이어 근처로 보냄
            UnityEngine.Vector3 dir = PickTarget() - transform.position;                         //공을 aimtarget쪽으로 보낼때 사용!!!!!!!!!!!!!!!!
                                                                                                       // Ball.GetComponent<Rigidbody>().useGravity = true; //공 활성화
            Ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot3.hitForce + new Vector3(0, currentShot3.upForce, 0);   //공이 부딪힐떄 힘과 높이 설정

            animator3.Play("serve5");
            Ball.GetComponent<Rigidbody>().useGravity = true; //중력 되살리기

            animator3.SetBool("leg_move", true);
            //animator.SetBool("serve", true);
            //animator.SetBool("forehand", false);
            //animator.SetBool("backhand", false);

            //animator.SetBool("enable", false);
            //animator.enabled = false;
            //서브실행
            //animator.SetBool("pause", false);
            //animator.SetBool("backhand", false);


        }

    

        



    }

    void Move()                                  //봇이 공을 따라다니는 함수
    {
        targetPosition.z = Ball.position.z;     //공을 봇의 축과 맞추고           


        //player(2)스크립트 불러옴
        //Player2 player2 = GameObject.Find("Player(2)").GetComponent<Player2>();    //공 컴포넌트 불러와서
        //player2.count1 = 0;
        //if ()
        //targetPosition.x = Net_B.transform.position.x;
        


        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);  //공의 위치를 봇이 따라간다.

        animator3.SetBool("leg_move",true);     //움직일떄 모션 추가
        //animator3.Play("leg_move");
        //animator3.Play("default_moshion");
    }


    public Vector3 PickTarget()        //aimtarget 10개중 어디로 갈지 위치 반환
    {
        int randomValue = Random.Range(0, targets.Length);  //aimtargets.Length는 10개
        return targets[randomValue].position;               //aimtarget 10개중에 하나의 위치를 반환

    }

    public Shot PickShot()                               //aimtarget으로 공보낼떼 topspin이냐, flat이냐 설정
    {
        int randomValue = Random.Range(0, 6);       //한번 칠떄  6분의 1확률로 flat이 나간다.
        if (randomValue == 0)
            return shotManager.flat;
        else
            return shotManager.topSpin;
    }



    private void OnTriggerEnter(Collider other)       //애니메이션 함수
    {
        if (other.CompareTag("Ball"))   //볼 태그를 얻어와서
        {
            Shot currentShot = PickShot();         //aimtarget으로 보내는 공의 힘과 높이 설정

            //공을 aimtarget쪽으로 보낼때~~
            Vector3 dir = PickTarget() - transform.position;                                      //공을 aimtarget쪽으로 보낼때 사용
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);   //사용자가 공치기(공 높이,힘 설정)
            
            //Ball스크립트에서 쓰임!!
            Ball ball_count1 = GameObject.Find("Ball").GetComponent<Ball>();    //공 컴포넌트 불러와서
            ball_count1.count1 = 0;                                             //충돌시 공의 카운트를 0으로 만든다.





            //백핸드5, 포핸드5로 나눠치는 코드
            Vector3 ballDir = Ball.position - transform.position;       //공과 플레이어의 거리를 얻어와서



            if (ballDir.z >= 0)
            {                    //공과 플레이어간의 y의 거리가 0보다 크면
                animator3.Play("forehand5");
                
            }
            else
            {
                animator3.Play("backhand5");
                
            }

            

        }
    }


}

