using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Transform aimTarget;
    public Transform Ball;
    public Transform serve_position;              //서브시 공의 위치
    //public Transform racket2;
    float speed = 3f;
    //float force = 13;

    bool hit;

    public Transform[] targets;         //aimTarget 3개 불러오기




    public Animator animator;

    Vector3 aimTargetInitialPosition;           //사용자가 공을 치면 aimtarget을 제자리에 놓을떄 사용

    public ShotManeger shotManager2;                  //shotManager클래스 받아옴
    Shot currentShot;                        //shot클래스 받아옴

    void Start()
    {
        animator = GetComponent<Animator>();                //Animator  컴포넌트 받아옴
        aimTargetInitialPosition = aimTarget.position;     //사용자가 공을 치면 aimtarget을 제자리에 놓을떄 사용

        //////////////////////////////////
        shotManager2 = GetComponent<ShotManeger>();          //shotManager  컴포넌트 받아옴
        currentShot = shotManager2.topSpin;                  //
        //////////////////////////////////
    }

    void Update()
    {
        //animator.SetBool("leg_move", true);     //움직일떄 모션 추가

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentShot = shotManager2.topSpin;
            hit = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hit = false;
        }



        if (Input.GetKeyDown(KeyCode.E))
        {
            currentShot = shotManager2.flat;
            hit = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hit = false;
        }



        /*
        if (Input.GetKeyUp(KeyCode.Q))      //Q누르면 공 중력 멈춤
        {
            //Ball.transform.position = new Vector3(0, 0, 0);
            Ball.GetComponent<Rigidbody>().Velocity = new Vector3(0, 0, 0);
            Ball.GetComponent<Rigidbody>().useGravity = false; //공 멈추기
        }
        */



        if (Input.GetKeyDown(KeyCode.R))  //flat서브 준비상태
        {
            hit = true;
            currentShot = shotManager2.flatServe;           // flat서브 준비
            GetComponent<BoxCollider>().enabled = false;  //R키 누르면 박스콜리더 효과 없앰
            Ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // 공의 힘을 먼저 없앤다
            Ball.transform.position = serve_position.transform.position;   //서브준비상태로서 볼을 플레이어 쪽으로 가져온다
           
            //Ball.GetComponent<Rigidbody>().velocity = new UnityEngine.Vector3(0, 6, 0);            //공을 y축으로 3만큼 튀어오르게함
            Ball.GetComponent<Rigidbody>().useGravity = false; //공 멈추기(중력 없애기)
                                                               //StartCoroutine(ExampleCoroutine());                //1초 대기 함수


            animator.Play("serve-prepare5");                 //서브준비상태
                                                             // animator.SetBool("pause", false);
            animator.SetBool("leg_move", false);
        }

        if (Input.GetKeyDown(KeyCode.T))  //.kick서브 준비상태
        {
            hit = true;
            currentShot = shotManager2.kickServe;           // kick서브 준비
            GetComponent<BoxCollider>().enabled = false;  //R키 누르면 박스콜리더 효과 없앰
            Ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // 공의 힘을 먼저 없앤다
            Ball.transform.position = serve_position.transform.position; //서브준비상태로서 볼을 플레이어 쪽으로 가져온다
            Ball.GetComponent<Rigidbody>().useGravity = false; //공 멈추기

            animator.Play("serve-prepare5");               //서브준비상태
            animator.SetBool("leg_move", false);
        }


        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T))       //서브 날리기~
        {
            hit = false;
            GetComponent<BoxCollider>().enabled = true;                                                 //R키 떼면 박스콜리더 효과 살아남
            Ball.transform.position = serve_position.transform.position;                       ///공의 위치를 플레이어 근처로 보냄


            UnityEngine.Vector3 dir = aimTarget.position - transform.position;                         //공을 aimtarget쪽으로 보낼때 사용!!!!!!!!!!!!!!!!
                                                                                                       // Ball.GetComponent<Rigidbody>().useGravity = true; //공 활성화
            Ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);   //공이 부딪힐떄 힘과 높이 설정

            animator.Play("serve5");
            Ball.GetComponent<Rigidbody>().useGravity = true; //중력 되살리기


            animator.SetBool("leg_move", true);
            //animator.SetBool("serve", true);
            //animator.SetBool("forehand", false);
            //animator.SetBool("backhand", false);

            //animator.SetBool("enable", false);
            //animator.enabled = false;
            //서브실행
            //animator.SetBool("pause", false);
            //animator.SetBool("backhand", false);


        }









        if (hit) //F누른 상태로는 TarGet 움직임
        {
            aimTarget.Translate(new Vector3(-h, 0, 0) * speed * 2 * Time.deltaTime);
        }


        if ((h != 0 || v != 0) && !hit)
        { //F를 누르면 Player가 안움직여진다.
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);

            /*
            Vector3 ballDir = Ball.position - transform.position;       //공과 플레이어의 거리를 얻어와서

            if (ballDir.z <= 0)   //스윙 나누기
            {                    //공과 플레이어간의 y의 거리가 0보다 크면
                Debug.Log("forehand");
            }
            else
            {
                Debug.Log("backhand");
            }
            Debug.DrawRay(transform.position, ballDir);    //공과 플레이어 사이 줄표시
            */

        }
    }


    public Vector3 PickTarget2()        //aimtarget 10개중 어디로 갈지 위치 반환
    {
        int randomValue = Random.Range(0, targets.Length);  //aimtargets.Length는 10개
        return targets[randomValue].position;               //aimtarget 10개중에 하나의 위치를 반환

    }

    public Shot PickShot2()                               //aimtarget으로 공보낼떼 topspin이냐, flat이냐 설정
    {
        int randomValue = Random.Range(0, 6);       //한번 칠떄  6분의 1확률로 flat이 나간다.
        if (randomValue == 0)
            return shotManager2.flat;
        else
            return shotManager2.topSpin;
    }





    private void OnTriggerEnter(Collider other)                   //사용자가 공(태그)를 칠떄 불러오는 함수
    {
        if (other.CompareTag("Ball"))                                 //볼 태그를 얻어와서
        {
            Shot currentShot = PickShot2();

            //공을 aimtarget쪽으로 보낼때~~
            Vector3 dir = PickTarget2() - transform.position;                                      //공을 aimtarget쪽으로 보낼때 사용!!!!!!!!!!!!!!!!
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);   //사용자가 공치기 (공 높이, 힘 설정)



            //Ball스크립트에서 쓰임!!
            Ball ball_count2 = GameObject.Find("Ball").GetComponent<Ball>();    //공 컴포넌트 불러와서
            ball_count2.count2 = 0;                                             //충돌시 공의 카운트를 0으로 만든다.


            Vector3 ballDir = Ball.position - transform.position;       //공과 플레이어의 거리를 얻어와서

            if (ballDir.z <= 0)
            {                    //공과 플레이어간의 y의 거리가 0보다 크면
                animator.Play("forehand5");
            }
            else
            {
                animator.Play("backhand5");
            }

            aimTarget.position = aimTargetInitialPosition;      //사용자가 공을 치면 aimTarget을 원래 위치로 복귀

        }
    }
}
