using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{

    public GameObject playerTransform;
    public GameObject firstCamera; // 평상시 카메라
    public GameObject secondCamera; // 에임 카메라

    public GameObject bow; // 사용할 활의 위치
    public GameObject bowString; // 사용할 활의 줄 위치
    public GameObject bowShotpotion; // 에이밍 기술시 bowString의 부모가 될 오브젝트
    public GameObject[] arrow; // 사용할 화살 변수

    Animator animator;
    Player player;

    public bool isCharging = false;
    public bool isShot;

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
        if (Input.GetMouseButtonDown(0) && isCharging == true)
        {
            if (secondCamera.activeSelf)
            {
                secondCamera.SetActive(false);

            }
            StopCoroutine(StringDelay());
            arrow[player.skillComand].SetActive(false);
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

            if (isCharging)
            {
                // 에이밍 시작
                animator.SetBool("IsCharging", true);
                animator.SetBool("IsShoted", false);
                StartCoroutine(StringDelay());  // 코루틴 시작
            }
            else
            {
                // 에이밍 취소
                StopCoroutine(StringDelay());  // 코루틴 중지
                arrow[player.skillComand].SetActive(false);
                animator.SetBool("IsCharging", false);
                bowString.transform.parent = bow.transform;  // 활 줄을 원래 위치로
                bowString.transform.localPosition = bowStringOriginOffset;
                arrow[player.skillComand].SetActive(false);  // 화살 비활성화
            }
        }
    }

    // bowString의 위치의 자연스러움을 위한 딜레이 
    IEnumerator StringDelay()
    {
        if (player.skillComand >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            arrow[player.skillComand].SetActive(true);
            yield return new WaitForSeconds(0.7f);

            bowString.transform.parent = bowShotpotion.transform; // bowString의 부모를 오른손 손가락으로 이동
            bowString.transform.localPosition = Vector3.zero;
        }


    }
}
