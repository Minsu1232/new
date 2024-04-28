using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{
    [Header("Bow Transform")]   
    public GameObject bow; // 사용할 활의 위치
    public GameObject bowString; // 사용할 활의 줄 위치
    public GameObject bowShotpotion; // 에이밍 기술시 bowString의 부모가 될 오브젝트
    public GameObject[] arrow; // 사용할 화살 변수
    public ArrowDamageScriptable[] arrowDamageScriptable;

    public AudioClip[] arrowSoundClip;

    AudioSource audioSource;
    Player player;


    Animator animator;

    bool isCharging = false;
    bool shotReady;

    Vector3 bowStringOriginOffset; // bowString의 초기 위치를 잡기 위한 변수
    

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
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isLockOn)
        {            
            ArrowShot(); // 화살 발사 애니메이션            
        }
        player.Skill();



    }

    private void ArrowShot()
    {
        // 화살 발사
        if (Input.GetMouseButtonDown(0) && isCharging == true && shotReady == true)
        {
            arrow[player.skillComand].SetActive(false);
            StopCoroutine(StringDelay());

            //player.skillComand = 0;
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

    // BowAnimation 스크립트
    public void HandleLockOn(bool isLockedOn)
    {
        if (isLockedOn)
        {
            StartAiming();
        }
        else
        {
            StopAiming();
        }
    }

    void StartAiming()
    {
        isCharging = true;
        animator.SetBool("IsCharging", true);
        animator.SetBool("IsShoted", false);
        StartCoroutine(StringDelay());
    }

    void StopAiming()
    {
        isCharging = false;
        ResetCoroutineState();
        StopCoroutine(StringDelay());
        arrow[player.skillComand].SetActive(false);
        animator.SetBool("IsCharging", false);
        bowString.transform.parent = bow.transform;
        bowString.transform.localPosition = bowStringOriginOffset;
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

    //-------------------------------------
    // 애니메이션 이벤트
    public void ChargingSound()
    {
        audioSource.PlayOneShot(arrowSoundClip[0],0.5f);
    }
    public void ArrowSound()
    {
        // 클립별 사운드 제어
        if (arrowDamageScriptable[player.skillComand].name == "FireArrow")
        {
            audioSource.PlayOneShot(arrowDamageScriptable[1].arrowSound, 1f);
        }
        else
        {
            audioSource.PlayOneShot(arrowDamageScriptable[player.skillComand].arrowSound, 0.1f);
        }
    }
    public void ShotSound()
    {
        audioSource.PlayOneShot(arrowSoundClip[1],0.7f);
    }
}
