using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    public Transform Bot_serve_position;              //서브시 공의 위치
    public Transform Player_serve_position;              //서브시 공의 위치\
    Shot currentShot3;
    Shot currentShot2;


    public int count1;             //사용자쪽 count
    public int count2;             //봇쪽 count

    public Text playerScoreText;               //플레이어 점수판 텍스트
    public Text botScoreText;                  //봇 점수판 텍스트

    public int player_score_count;                     //사용자 승리시 점수판 count
    public int bot_score_count;                        //bot 승리시 점수판 count


    //봇과 사용자 오브젝트 불러오기(전역 변수로 불가능)
    // Bot bot = GameObject.Find("Bot").GetComponent<Bot>();    //봇 오브젝트 불러와서
    // Player2 player2 = GameObject.Find("player(2)").GetComponent<Player2>();



    // Start is called before the first frame update
    void Start()
    {

        //시작시 플레이어 서브/////////////////////////////////////////////////////////////////////////
        //TennisManager player2 = GameObject.Find("Racket").GetComponent<TennisManager>();    //봇 오브젝트 불러와서
        //player2.animator = player2.GetComponent<Animator>();

        GetComponent<Rigidbody>().useGravity = false;     //중력 없애기
        //player2.animator.SetBool("leg_move", false);     //움직임 멈추기
        //player2.animator.Play("serve-prepare5");           //서브준비상태

        //StartCoroutine("Player_serve_play");
        /////////////////////////////////////////////////////////////////////////////////////////////////

        //카운트 셀때 사용
        count1 = 0;
        count2 = 0;

        player_score_count = 0;
        bot_score_count = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {           //공이 콜리젼에 닿으면


        //사용자쪽에서 점수 이벤트 발생/////////////////////////////////////////

        if (collision.transform.CompareTag("Ground(P_Center)"))  //player 중앙 그라운드에 한번 튕기면 count+1
        {
            count1++;
        }

        if (count1 >= 2)
        {                             //같은 코트 두번 맞으면 
            StartCoroutine("Bot_serve_delay");
            bot_score_count++;
        }

        else if (count1 == 1 && collision.transform.CompareTag("Wall(P)"))   //count가 1이고 플레이어 벽에 맞으면 (  승)
        {
            StartCoroutine("Bot_serve_delay");
            bot_score_count++;
        }

        else if (count1 == 1 && collision.transform.CompareTag("Ground(P)"))
        { //count가 1이고 플레이어 그라운드밖에 맞으면 (  승)
            StartCoroutine("Bot_serve_delay");
            bot_score_count++;
        }

        else if (count1 == 0 && collision.transform.CompareTag("Wall(P)"))
        { //count가 0이고 벽에 맞으면
            StartCoroutine("Player_serve_delay");
            bot_score_count++;
        }

        else if (count1 == 0 && collision.transform.CompareTag("Ground(P)"))
        { //count가 0이고 플레이어 그라운드밖에 맞으면 
            StartCoroutine("Player_serve_delay");
            bot_score_count++;
        }
        else if (count1 == 0 && collision.transform.CompareTag("Net(B)"))
        {
            StartCoroutine("Player_serve_delay");
            bot_score_count++;
        }
        ////////////////////////////////////////////////////////////////////




        //봇쪽에서 점수 이벤트 발생//////////////////////////////////////////////////////

        if (collision.transform.CompareTag("Ground(B_Center)"))  //player 중앙 그라운드에 한번 튕기면 count+1
        {
            count2++;
        }

        if (count2 >= 2)
        {                             //코트 두번 맞으면 
            StartCoroutine("Player_serve_delay");
            player_score_count++;
        }

        else if (count2 == 1 && collision.transform.CompareTag("Wall(B)"))   //count가 1이고 플레이어 벽에 맞으면 (  승)
        {
            StartCoroutine("Player_serve_delay");
            player_score_count++;
        }

        else if (count2 == 1 && collision.transform.CompareTag("Ground(B)"))
        { //count가 1이고 플레이어 그라운드밖에 맞으면 (  승)
            StartCoroutine("Player_serve_delay");
            player_score_count++;
        }

        else if (count2 == 0 && collision.transform.CompareTag("Wall(B)"))
        { //count가 0이고 벽에 맞으면
            StartCoroutine("Bot_serve_delay");
            player_score_count++;
        }

        else if (count2 == 0 && collision.transform.CompareTag("Ground(B)"))
        { //count가 0이고 플레이어 그라운드밖에 맞으면 
            StartCoroutine("Bot_serve_delay");
            player_score_count++;
        }
        else if (count2 == 0 && collision.transform.CompareTag("Net(P)"))
        {
            StartCoroutine("Bot_serve_delay");
            player_score_count++;
        }
        ////////////////////////////////////////////////////////////////////





    }





    IEnumerator Bot_serve_delay()  //봇 승리시 시간 지연 함수!!!!! 코루틴
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0.1f, 0.11f, 0.1f);                      //공이 자연스럽게 흘러가게 해줌
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                      //공의 힘을 없앰
        transform.position = Bot_serve_position.transform.position;                  //공의 위치를 봇쪽으로 보냄



        //Bot 컴포넌트 불러와서 서브애니메이션 사용!!
        Bot bot = GameObject.Find("Bot").GetComponent<Bot>();    //봇 오브젝트 불러와서
        bot.animator3 = bot.GetComponent<Animator>();

        GetComponent<Rigidbody>().useGravity = false;     //중력 없애기
        bot.animator3.SetBool("leg_move", false);     //움직임 멈추기
        bot.animator3.Play("serve-prepare5");           //서브준비상태

        StartCoroutine("Bot_serve_play");
        updateScores();     //점수판 갱신

        count1 = 0;
        count2 = 0;
    }





        IEnumerator Player_serve_delay()  //사용자 승리시 시간 지연 함수!!!!! 코루틴
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0.1f, 0.11f, 0.1f);                      //공이 자연스럽게 흘러가게 해줌
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);                      //공의 힘을 없앰

        transform.position = Player_serve_position.transform.position;                  //공의 위치를 사용자쪽으로 보냄

        //StartCoroutine("Player_serve_play");
        GetComponent<Rigidbody>().useGravity = false;     //중력 없애기 (이게 플레이서 서브바로 전단계)
        updateScores();     //점수판 갱신

        count1 = 0;
        count2 = 0;
    }






    Vector3 picktarget;
    IEnumerator Bot_serve_play()  //봇 승리시 서브 함수
    {
        //Bot 컴포넌트 불러옴
        Bot bot = GameObject.Find("Bot").GetComponent<Bot>();    //봇 오브젝트 불러와서
        bot.animator3 = bot.GetComponent<Animator>();                      //봇의 애니메이션 변수 불러옴

        picktarget = GameObject.Find("Bot").GetComponent<Bot>().PickTarget(); // PickTarget함수 불러옴
        bot.shotManager = bot.GetComponent<ShotManeger>();                  //봇의 샷매니저 변수 불러옴

        currentShot3 = bot.shotManager.kickServe;       //곡선 서브준비

        yield return new WaitForSeconds(1f);              //1초지연

        Vector3 dir = picktarget - bot.transform.position;     //공을 봇에게 보낸다
        GetComponent<Rigidbody>().velocity = dir.normalized * currentShot3.hitForce + new Vector3(0, currentShot3.upForce, 0);   //공이 부딪힐떄 힘과 높이 설정


        bot.animator3.Play("serve5");           //서브시작
        GetComponent<Rigidbody>().useGravity = true;     //중력 생김


        //player3.animator3.SetBool("leg_move", true);     //움직임 재시작

    }


    void updateScores()     //점수 발생 함수
    {
        playerScoreText.text = "Player : " + player_score_count + "점";     //playerScore
        botScoreText.text = "Bot : " + bot_score_count + "점";               //BotScore
    }




    void Update()
    {

        Debug.Log("count1은" + count1);
        Debug.Log("count2은" + count2);
    }


}
