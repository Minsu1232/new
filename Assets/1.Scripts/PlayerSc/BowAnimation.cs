using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{

    public GameObject playerTransform;



    public GameObject bow; // 사용할 활의 위치
    public GameObject bowString; // 사용할 활의 줄 위치
    public GameObject bowShotpotion; // 에이밍 기술시 bowString의 부모가 될 오브젝트
    public GameObject[] arrow; // 사용할 화살 변수

    Animator animator;
    Player player;

    public bool isCharging = false;
    public bool shotReady;

    Vector3 bowStringOriginOffset; // bowString의 초기 위치를 잡기 위한 변수

    int skillComand;

    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        bowStringOriginOffset = new Vector3(0f, 0.156f, 0f);

        isCharging = false;

        player = GetComponent<Player>();
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Aiming(); // 조준 및 취소 애니메이션
        ArrowShot(); // 화살 발사 애니메이션
        player.Skill();



    }

    private void ArrowShot()
    {
        // 화살 발사
        if (Input.GetMouseButtonDown(0) && isCharging == true && shotReady == true)
        {
            arrow[player.skillComand].SetActive(false);
            StopCoroutine(StringDelay());
            
            player.skillComand = 0;
            animator.SetBool("IsShot", true);
            animator.SetBool("IsShoted", true);

            isCharging = false;            
            animator.SetBool("IsCharging", false);
            // 애니메이션 조건   IsShot, true
            //                   IsCharging, false

            bowString.transform.parent = bow.transform; // 자식을 원래 위치로 이동
            bowString.transform.localPosition = bowStringOriginOffset;



        }
    }

    private void Aiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // isCharging 상태를 토글
            isCharging = !isCharging;

            if (!isCharging)
            { // 에이밍 취소
                ResetCoroutineState();
                StopCoroutine(StringDelay());  // 코루틴 중지
                arrow[player.skillComand].SetActive(false);
                animator.SetBool("IsCharging", false);
                bowString.transform.parent = bow.transform;  // 활 줄을 원래 위치로
                bowString.transform.localPosition = bowStringOriginOffset;
                arrow[player.skillComand].SetActive(false);  // 화살 비활성화
               
            }
            else if(isCharging && player.isLockOn)
            {
                // 에이밍 시작
                shotReady = false;
                animator.SetBool("IsCharging", true);
                animator.SetBool("IsShoted", false);
                StartCoroutine(StringDelay());  // 코루틴 시작
            }
        }
    }

    // bowString의 위치의 자연스러움을 위한 딜레이 
    IEnumerator StringDelay()
    {
        ResetCoroutineState();
        if (player.skillComand >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            arrow[player.skillComand].SetActive(true);
            shotReady = true;
            yield return new WaitForSeconds(0.5f);
            bowString.transform.parent = bowShotpotion.transform; // bowString의 부모를 오른손 손가락으로 이동
            bowString.transform.localPosition = Vector3.zero;
        }

    }
    // 코루틴 초기화 매서드
    void ResetCoroutineState()
    {
        
        // 화살과 활 줄을 초기 상태로 설정
        foreach (GameObject arr in arrow)
        {
            arr.SetActive(false);
        }
        bowString.transform.parent = bow.transform; // 활 줄의 부모를 원래대로 설정
        bowString.transform.localPosition = bowStringOriginOffset; // 활 줄의 위치를 원래 위치로
        shotReady = false;

    }
}
