using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{
    [Header("Bow Transform")]   
    public GameObject bow; // ����� Ȱ�� ��ġ
    public GameObject bowString; // ����� Ȱ�� �� ��ġ
    public GameObject bowShotpotion; // ���̹� ����� bowString�� �θ� �� ������Ʈ
    public GameObject[] arrow; // ����� ȭ�� ����
    public ArrowDamageScriptable[] arrowDamageScriptable;

    public AudioClip[] arrowSoundClip;

    AudioSource audioSource;
    Player player;


    Animator animator;

    bool isCharging = false;
    bool shotReady;

    Vector3 bowStringOriginOffset; // bowString�� �ʱ� ��ġ�� ��� ���� ����
    

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
            ArrowShot(); // ȭ�� �߻� �ִϸ��̼�            
        }
        player.Skill();



    }

    private void ArrowShot()
    {
        // ȭ�� �߻�
        if (Input.GetMouseButtonDown(0) && isCharging == true && shotReady == true)
        {
            arrow[player.skillComand].SetActive(false);
            StopCoroutine(StringDelay());

            //player.skillComand = 0;
            animator.SetBool("IsShot", true);
            animator.SetBool("IsShoted", true);            
            isCharging = false;
            animator.SetBool("IsCharging", false);
            // �ִϸ��̼� ����   IsShot, true
            //                   IsCharging, false

            bowString.transform.parent = bow.transform; // �ڽ��� ���� ��ġ�� �̵�
            bowString.transform.localPosition = bowStringOriginOffset;



        }
    }

    // BowAnimation ��ũ��Ʈ
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

    // bowString�� ��ġ�� �ڿ��������� ���� ������ 
    IEnumerator StringDelay()
    {
        ResetCoroutineState();
        if (player.skillComand >= 0)
        {
            yield return new WaitForSeconds(0.5f);            
            arrow[player.skillComand].SetActive(true);
            shotReady = true;
            yield return new WaitForSeconds(0.5f);
            bowString.transform.parent = bowShotpotion.transform; // bowString�� �θ� ������ �հ������� �̵�
            bowString.transform.localPosition = Vector3.zero;
        }

    }
    // �ڷ�ƾ �ʱ�ȭ �ż���
    void ResetCoroutineState()
    {

        // ȭ��� Ȱ ���� �ʱ� ���·� ����
        foreach (GameObject arr in arrow)
        {
            arr.SetActive(false);
        }
        bowString.transform.parent = bow.transform; // Ȱ ���� �θ� ������� ����
        bowString.transform.localPosition = bowStringOriginOffset; // Ȱ ���� ��ġ�� ���� ��ġ��
        shotReady = false;

    }

    //-------------------------------------
    // �ִϸ��̼� �̺�Ʈ
    public void ChargingSound()
    {
        audioSource.PlayOneShot(arrowSoundClip[0],0.5f);
    }
    public void ArrowSound()
    {
        // Ŭ���� ���� ����
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
