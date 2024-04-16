using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimation : MonoBehaviour
{

    public GameObject playerTransform;



    public GameObject bow; // ����� Ȱ�� ��ġ
    public GameObject bowString; // ����� Ȱ�� �� ��ġ
    public GameObject bowShotpotion; // ���̹� ����� bowString�� �θ� �� ������Ʈ
    public GameObject[] arrow; // ����� ȭ�� ����

    Animator animator;
    Player player;

    public bool isCharging = false;
    public bool shotReady;

    Vector3 bowStringOriginOffset; // bowString�� �ʱ� ��ġ�� ��� ���� ����

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
        Aiming(); // ���� �� ��� �ִϸ��̼�
        ArrowShot(); // ȭ�� �߻� �ִϸ��̼�
        player.Skill();



    }

    private void ArrowShot()
    {
        // ȭ�� �߻�
        if (Input.GetMouseButtonDown(0) && isCharging == true && shotReady == true)
        {
            arrow[player.skillComand].SetActive(false);
            StopCoroutine(StringDelay());
            
            player.skillComand = 0;
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

    private void Aiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // isCharging ���¸� ���
            isCharging = !isCharging;

            if (!isCharging)
            { // ���̹� ���
                ResetCoroutineState();
                StopCoroutine(StringDelay());  // �ڷ�ƾ ����
                arrow[player.skillComand].SetActive(false);
                animator.SetBool("IsCharging", false);
                bowString.transform.parent = bow.transform;  // Ȱ ���� ���� ��ġ��
                bowString.transform.localPosition = bowStringOriginOffset;
                arrow[player.skillComand].SetActive(false);  // ȭ�� ��Ȱ��ȭ
               
            }
            else if(isCharging && player.isLockOn)
            {
                // ���̹� ����
                shotReady = false;
                animator.SetBool("IsCharging", true);
                animator.SetBool("IsShoted", false);
                StartCoroutine(StringDelay());  // �ڷ�ƾ ����
            }
        }
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
}
