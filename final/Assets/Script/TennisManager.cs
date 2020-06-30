using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisManager : MonoBehaviour
{
    public Transform aimTarget;
    public Transform Ball;
    public Transform serve_position;
    Vector3 aimTargetInitialPosition;           //사용자가 공을 치면 aimtarget을 제자리에 놓을떄 사용

    ShotManeger shotManager;                  //shotManager클래스 받아옴
    Shot currentShot;

    public Transform[] targets;         //aimTarget 3개 불러오기

    void Start()
    {               //Animator  컴포넌트 받아옴
        aimTargetInitialPosition = aimTarget.position;     //사용자가 공을 치면 aimtarget을 제자리에 놓을떄 사용

        //////////////////////////////////
        shotManager = GetComponent<ShotManeger>();          //shotManager  컴포넌트 받아옴
        currentShot = shotManager.topSpin;                  //
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
            return shotManager.flat;
        else
            return shotManager.topSpin;
    }

    //shot클래스 받아옴
    private void OnTriggerEnter(Collider other)                   //사용자가 공(태그)를 칠떄 불러오는 함수
    {
        if (other.CompareTag("Ball"))                                 //볼 태그를 얻어와서
        {
            //공을 aimtarget쪽으로 보낼때~~
            Vector3 dir = PickTarget2() - transform.position;                                      //공을 aimtarget쪽으로 보낼때 사용!!!!!!!!!!!!!!!!
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);   //사용자가 공치기 (공 높이, 힘 설정)

            Ball ball_count2 = GameObject.Find("Ball").GetComponent<Ball>();    //공 컴포넌트 불러와서
            ball_count2.GetComponent<Rigidbody>().useGravity = true;     //중력 생김
            ball_count2.count2 = 0;                                             //충돌시 공의 카운트를 0으로 만든다.

            Vector3 ballDir = Ball.position - transform.position;       //공과 플레이어의 거리를 얻어와서

            aimTarget.position = aimTargetInitialPosition;      //사용자가 공을 치면 aimTarget을 원래 위치로 복귀

        }
    }
}
