using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vitals;

// 인터페이스 사용해 피격 판정
public class Boss : MonoBehaviour, IDamageable
{
    [Header("Boss Info")]
    [SerializeField]
    BossScriptable bossScriptable;
    public int initialHealth; // 시작 체력
    public int remainHealth; // 남은 체력
    public int damage;
    public int neutralizeValue; // 시작 무력화
    public int remainNeutralizeVlaue; // 남은 무력화 
    public int destructionValue; // 시작 파괴 수치    
    public int walkSpeed;
    public int runSpeed;

    [Header("Boss Attributes")]
    [SerializeField]
    NavMeshAgent navMeshAgent;
    public GameObject[] gimmickWalls;
    public Transform gimmickStartPosition;
    public int gimmickCount; // 0이 되면 파훼    
    public ParticleSystem[] secondpahseParticle;
    public AudioClip attackSound;
    public AudioClip razeSound;
    public AudioClip walkSound;
    public AudioClip runSound;
    public Material[] counterColor;


    [Header("Monster UI")]
    public Text hp;
    public Image hpBar;
    public Image neutralizeBar;
    public Image destructionBar;
    public Image destructionImage;
    [Header("Player Attributes")]
    public Player player;

    [Header("Check Attributes")]
    public bool isgimmick; // 업데이트문 제약 조건    
    public bool isGimmicked; // 기믹 제약 조건
    public bool isAttack;
    public bool isKill;
    public bool isDie;
    public bool isGroggy;
    public bool isSecondPhase;
    public bool isSecondPhaseStart;
    public bool isGettingHit;  // 새로운 상태 변수 추가
    public bool isGimmickEnd;
    public bool firstGroggy;
    public bool isDestruction;

    public Animator animator;
    AudioSource audioSource;



    private Coroutine damageCoroutine; //기믹의 달리기 데미지 주기를 위한 변수

    // Start is called before the first frame update

    private void OnEnable()
    {
        counterColor[0].SetColor("_EmissionColor", Color.black); // 색 초기화
        counterColor[1].SetColor("_EmissionColor", Color.black);
        initialHealth = bossScriptable.health;
        damage = bossScriptable.damage;
        walkSpeed = bossScriptable.walkSpeed;
        neutralizeValue = bossScriptable.neutralizeValue;
        destructionValue = bossScriptable.destructionValue;
        runSpeed = bossScriptable.runSpeed;
        // 남은 value 체크용
        remainHealth = initialHealth;
        remainNeutralizeVlaue = neutralizeValue;
      
    }
    void Start()
    {
        isSecondPhaseStart = false;
        isSecondPhase = false;
        isGettingHit = false;
        isDie = false;
        isGroggy = false;
        isGimmickEnd = false;
        firstGroggy = false;
        isDestruction = false;
        gimmickCount = 4;

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        navMeshAgent.speed = walkSpeed;
        //시작 ui
        hp.text = $"{initialHealth}/{initialHealth}";
        hpBar.fillAmount = remainHealth / initialHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            // ui 실시간 업데이트
            hp.text = $"{remainHealth}/{initialHealth}";
            hpBar.fillAmount = (float)remainHealth / initialHealth;
            neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Groggy")) // 카운터 성공시
            {
                counterColor[0].SetColor("_EmissionColor", Color.black); // 색 초기화
                counterColor[1].SetColor("_EmissionColor", Color.black);
            }
            if (!isgimmick && !isSecondPhase)
            {
                Groggy();
                MonsterMove();
                Attack();                
                Gimmick();

            }
            // 체력이 30% 이하이고 두 번째 단계가 아직 시작되지 않았다면,
            if (remainHealth <= 90 && !isSecondPhaseStart)
            {
                SecondPhaseStart();
                // 2페이즈엔 데미지가 +10, 속도는 Run
            }
            if (!isDestruction)
            {
                Destruction();
            }
            GimmicDamage();
            GimmicEnd();






        }

    }
    /// <summary>
    /// 몬스터의 피격 함수 ArrowDamage에서 호출.
    ///F </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, int neutralizeValu,int destruction, bool shouldTriggerHitAnimation = true)
    {
        // 남은 체력이 0보다 크면 데미지 적용
        if (remainHealth > 0)
        {
            remainHealth -= damage;
        }
        // 남은 무력화 게이지가 0보다 크면 무력화데미지 적용
        
        
         remainNeutralizeVlaue -= neutralizeValu;
        

        // health값이 음수가 되지 않게 제어
        remainHealth = Mathf.Clamp(remainHealth, 0, initialHealth);
        hp.text = $"{remainHealth}/{initialHealth}";
        hpBar.fillAmount = (float)remainHealth / initialHealth;
        neutralizeBar.fillAmount = (float)remainNeutralizeVlaue / neutralizeValue;
        if (shouldTriggerHitAnimation)
        {// 보스는 도트데미지에 멈추지 않음
            StartCoroutine(Gethit());
        }
        if (isGroggy && !firstGroggy)
        {
            Debug.Log("파괴가능");
            destructionValue -= destruction;
        }
        if (remainHealth <= 0)
        {
            Die();
        }
    }
    void MonsterMove()
    {
        // 애니메이션 진행중일땐 움직임 x
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("SecondAttack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("CounterAttack") && isAttack == false && !isDie && player != null && !isgimmick)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("Move", true);
            
        }


    }
    // 몬스터의 공격함수 애니메이션 이벤트를 통해 데미지를 줌
    void Attack()
    {
        // 플레이어와의 거리 계산
        if (player != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

            // 거리가 3f 이하이고, 플레이어의 체력이 0보다 클 때
            if (distance <= 3f && player.remainHealth > 0)
            {
                // 플레이어를 공격
                if (!isKill)
                {
                    if (!isAttack) // 공격이 아직 시작되지 않았다면 랜덤 인덱스를 생성
                    {
                        int randomIndex = UnityEngine.Random.Range(0, 3);  // 0, 1, 또는 2
                        animator.SetInteger("AttackInt", randomIndex);
                        
                    }

                    animator.SetBool("Attack", true);
                    animator.SetBool("Move", false);
                    isAttack = true;
                    navMeshAgent.enabled = false;
                }
            }
            else if (player.remainHealth <= 0) // 플레이어의 체력이 0 이하면
            {
                // 플레이어가 죽었을 때의 행동 및 애니메이션
                if (!isKill)
                {
                    animator.SetTrigger("Kill");
                    isKill = true;
                }
            }
            else
            {
                // 거리가 멀어지면 다시 이동
                isAttack = false;
                animator.SetBool("Move", true);
                animator.SetBool("Attack", false);
            }
        }

    }
    // 애니메이션 이벤트
    // 카운터전조시 몬스터의 색이 변함
    void CounterColorRed()
    {
        // 밝기 조절
        float intensity = 1000.0f;
        Color emissionColor = new Color(1.0f, 0.0f, 0.0f); // 기본 빨간색
        Color intenseColor = new Color(emissionColor.r * intensity, emissionColor.g * intensity, emissionColor.b * intensity);

        counterColor[0].SetColor("_EmissionColor", intenseColor);
        counterColor[1].SetColor("_EmissionColor", intenseColor);
        
    }
    void CounterColorEnd()
    {
        counterColor[0].SetColor("_EmissionColor", Color.black);
        counterColor[1].SetColor("_EmissionColor", Color.black);
    }
    
    void Destruction()
    {
        if(destructionValue <= 0)
        {
            isDestruction = true;
            destructionImage.gameObject.SetActive(true);
            player.str += 2;
            
        }
    }
    private void Die()
    {
        if (!isDie)
        {
            Debug.Log("Monster died.");
            animator.SetTrigger("Die");
            isDie = true;
            navMeshAgent.enabled = false;
            Destroy(gameObject, 3f);
        }
    }
    // 애니메이션 작동 트리거
    public IEnumerator Gethit()
    {
        if (isGettingHit)  // 이미 피격 상태인 경우 더 이상 진행하지 않음
            yield break;

        isGettingHit = true;
        animator.SetBool("Gethit", true);
        navMeshAgent.speed = 0;  // 속도를 0으로 설정
        Debug.Log("GetHit");
        yield return new WaitForSeconds(1.3f);  // 피격 애니메이션 재생 시간 동안 대기

        navMeshAgent.speed = walkSpeed;  // 속도 복원
        animator.SetBool("Gethit", false);
        isGettingHit = false;  // 피격 상태 해제
    }
  
    // 그로기 관련 매서드
    void Groggy()
    {
        if (remainNeutralizeVlaue <= 0)
        {
            StartCoroutine(GroggyTime());
        }
    }
   
    IEnumerator GroggyTime()
    {
        
        if (!isGroggy)
        {
            animator.SetTrigger("Groggy");
            animator.SetBool("IsGroggy", true);
        }
        isGroggy = true;
        navMeshAgent.speed = 0;  // 속도를 0으로 설정

        yield return new WaitForSeconds(8f); // 피격 애니메이션 재생 시간 동안 대기
        firstGroggy = true;
        if(isGroggy)
        {
            remainNeutralizeVlaue = neutralizeValue;
        }
        isGroggy = false;  // 피격 상태 해제
        animator.SetBool("IsGroggy", false);
        navMeshAgent.speed = walkSpeed;  // 속도 복원        
       
        



    }

    // 기믹의 시작 조건인 매서드
    void Gimmick()
    {
        // 체력이 50퍼 아래일때
        if (remainHealth < initialHealth * 0.5)
        {
            if (!isGimmicked)
            {
                navMeshAgent.enabled = false;
                animator.SetTrigger("Rage");
                isgimmick = true;
                // 각 pillar에 대해 MovePillar 코루틴 시작
                foreach (GameObject gimmickWall in gimmickWalls)
                {
                    StartCoroutine(MovePillar(gimmickWall, 8f, 1f)); // 1초 동안 8로 이동
                }
                isGimmicked = true;
                StartCoroutine(Gimmicking());
            }

        }
    }
    void GimmicDamage()
    {
        bool isGimmickRunning = animator.GetBool("GimmickRun");
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (isGimmickRunning && distanceToPlayer < 5f)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(GimmickDamagePlayer());
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
    // 기믹 파훼 매서드
    void GimmicEnd()
    {
        if (gimmickCount == 0)
        {
            if (!isGimmickEnd)
            {
                isGimmickEnd = true;
                StartCoroutine(GimmickEndCorutine());
            }

        }
    }
    // 2페이즈 스타트
    void SecondPhaseStart()
    {
        secondpahseParticle[0].gameObject.SetActive(true);
        secondpahseParticle[1].gameObject.SetActive(true);
        isSecondPhaseStart = true; // 두 번째 단계 시작 플래그를 true로 설정
        StartCoroutine(SecondPhase());
    }
    // 2페이즈 스타트
    IEnumerator SecondPhase()
    {
        // 두 번째 단계 로직을 실행합니다.
        isSecondPhase = true;
        navMeshAgent.enabled = false;
        animator.SetBool("Move", false);
        animator.SetTrigger("Rage");
        yield return new WaitForSeconds(2.02f);
        animator.SetBool("Move", true);
        walkSpeed = runSpeed;
        isSecondPhase = false;
    }
    // 첫 기믹 하울링 후 기믹시작지점으로 이동 하는 코루틴
    IEnumerator Gimmicking()
    {
        yield return new WaitForSeconds(2f); // 포효 후 이동 위한 시간제약
        animator.SetBool("GimmickRun", true);
        Vector3 startPosition = gameObject.transform.position;  // 시작 위치
        Vector3 endPosition = gimmickStartPosition.position;    // 끝 위치
        float closeEnoughDistance = 0.5f; // 충분히 가까운 거리

        while (true) // 무한 루프를 사용하고 break로 루프를 종료
        {
            // 오브젝트 위치 변경
            transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * 10); // 10은 조정 가능한 속도 값

            // 오브젝트가 endPosition을 바라보도록 설정
            Vector3 direction = (endPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            // 현재 위치가 목표 위치에 충분히 가까운지 확인
            if (Vector3.Distance(transform.position, endPosition) < closeEnoughDistance)
            {
                Debug.Log("도착");
                animator.SetBool("GimmickRun", false);
                animator.SetTrigger("Observ"); // 애니메이션 시간만큼 두리번 거린 후
                yield return new WaitForSeconds(3.12f);
                walkSpeed = bossScriptable.runSpeed; // 기믹 시작시 스피드 증가
                animator.SetBool("GimmickRun", true); // 플레이어를 향해 달려오는 애니메이션
                isgimmick = false;

                break; // 목표에 도달하면 루프 종료
            }

            yield return null;
        }

        // 최종 위치 및 방향 설정
        transform.position = endPosition;
        transform.rotation = Quaternion.LookRotation((endPosition - startPosition).normalized);

    }

    //체력 50퍼시 기둥이 떨어지는 코루틴
    IEnumerator MovePillar(GameObject gimmickwall, float targetY, float duration)
    {
        float time = 0;
        Vector3 startPosition = gimmickwall.transform.position; // 시작 위치
        Vector3 endPosition = new Vector3(gimmickwall.transform.position.x, targetY, gimmickwall.transform.position.z); // 종료 위치

        while (time < duration)
        {
            // 러프함수를 통해 점점 위치 변경
            gimmickwall.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        gimmickwall.transform.position = endPosition; // 최종 위치 확정
    }
    // 초당 20의 데미지
    IEnumerator GimmickDamagePlayer()
    {
        while (true)
        {
            player.TakeDamage(20);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GimmickEndCorutine()
    {
        navMeshAgent.speed = 0;
        animator.SetBool("GimmickRun", false);
        animator.SetBool("IsGroggy", true);
        yield return new WaitForSeconds(5f);
        navMeshAgent.speed = walkSpeed; // 다시 원래 속도로 돌아감 현잰 왜인지 모르겠으나 안돌아감
        animator.SetBool("IsGroggy", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 기믹 시작 할때부터 부셔짐
        if (other.gameObject.tag == "GimmickWalls" && !isgimmick)
        {
            Destroy(other.gameObject);
            gimmickCount--;
        }
    }
    // 애니메이션 이벤트 
    void Hit()
    {
        
        if (!isSecondPhaseStart)
        {
            player.TakeDamage(damage);
            if (player.isInvincible == false)
            {// 무적판정 소리 안나게
                AttackSound();
            }
        }
        else
        {// 2페이즈엔 데미지가 +10
            player.TakeDamage(damage + 10);
        }
        
        
    }
    void AttackSound()
    {
        audioSource.PlayOneShot(attackSound, 0.5f);
    }
    void RazeSound()
    {
        audioSource.PlayOneShot(razeSound);
    }
}
