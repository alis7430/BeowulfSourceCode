using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

//-----------------------------------------------------------
// Scripts\Character\EnemyBase.cs
//
// 적 캐릭터를 관리하는 클래스
// 1. Panda BT를 이용한 Behavior Tree로 동작
// 2. Perspective Sensor, Collider Sensor의 2가지를 이용하여 데이터를 받는다
// 3. BaseCharacter를 상속받아 이벤트를 사용
//-----------------------------------------------------------
public class Monster : BaseCharacter
{
    #region variables
    //-----------------------------------------------------------
    // Enemy의 행동로직에 필요한 변수들

    [HideInInspector]
    private float elapsedTime;
    private float deadDelayTime;

    public Transform spawnTransform;             // 몬스터의 시작 위치
    public Transform targetTransform;           // Player의 트랜스폼 참조
    private Transform modelTransform;           // 해당 스크립트의 모델 트랜스폼

    private Animator ani;

    private Vector3 destination;                 // 이동하고자 하는 방향

    public float moveSpeed;                     // 움직이는 속도
    public float rotateSpeed;                   // 회전 속도
    public float wanderRange;                   // 돌아다니는 범위
    public float attackRange;

    public int exp;

    private Perspective perspective;            // 시각 센서
    private NavMeshAgent navMeshAgent;          // 네비매시

    public BoxCollider AttackRange;

    public string deadsound;

    private bool hasEnemy = false;              // (공격하고자 하는) 적이 있는가
    private bool isOutOfBounds = false;         // 제한된 이동범위를 벗어났는가
    private bool is_attacked = false;           // 공격을 받았는가

    private bool doubleAttack = false;
    private bool is_usingSkill = false;
    #endregion

    #region methods
    //-----------------------------------------------------------
    // Use this for initialization
    protected override void Initialize()
    {
        base.Initialize();

        perspective = this.GetComponent<Perspective>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        spawnTransform = transform.parent;
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        modelTransform = transform.GetChild(0);
        ani = modelTransform.GetComponent<Animator>();

        AttackRange.enabled = false;
        is_dead = false;
        hasEnemy = false;
        is_attacked = false;

    }
    //-----------------------------------------------------------
    // Update per frame
    private void Update()
    {
        if (!hasEnemy)
            navMeshAgent.speed = moveSpeed;
        else
            navMeshAgent.speed = moveSpeed * 1.2f;
    }

    //-------------------------------------------------------
    //Called when events happen
    protected override void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        base.OnEvent(Event_Type, Sender, Param);

        switch (Event_Type)
        {
            case EVENT_TYPE.DEAD:
                perspective.targetMask = 0;
                hasEnemy = false;
                break;
            default:
                break;
        }
    }
    // ----------------------------------------------------------
    // 공격하는 무기의 OnTriggerEnter에서 호출한다.
    public override void CollisionDetected(Weapon weapon, Collider other)
    {
        int damage = (int)Random.Range(DAMAGE - DAMAGE / 4, DAMAGE + DAMAGE / 4);

        if (other.tag == "Player")
            other.gameObject.SendMessage("OnAttacked", damage);
    }
    //-------------------------------------------------------
    //Called when attacked by player
    protected override void OnAttacked(object param)
    {
        base.OnAttacked(param);

        if (!is_dead)
        {
            SoundManager.instance.PlaySFX("Damage_s01");
            if(this.ID != 301)
                ani.Play("Damage");
        }
        is_attacked = true;
    }
    //-------------------------------------------------------
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        bool INF = true;
        NavMeshHit navHit;

        do
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            if (navHit.hit)
                INF = false;

        } while (INF);

        return navHit.position;
    }
    //-------------------------------------------------------
    public void ActiveAttackRange()
    {
        AttackRange.enabled = true;
    }
    //-------------------------------------------------------
    private void HideAttackRange()
    {
        AttackRange.enabled = false;
    }
    //-------------------------------------------------------
    private void Respawn()
    {
        if (spawnTransform == null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        if (this.gameObject.GetComponent<UIShower>() != null)
        {
            this.gameObject.GetComponent<UIShower>().is_active = true;
        }

        this.gameObject.SetActive(true);
        Initialize();

        this.transform.position = spawnTransform.position + new Vector3(
            Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
    }

    #endregion
    #region tasks
    //-------------------------------------------------------
    [Task]
    bool IsDead()
    {
        return HEALTH <= 0;
    }
    //-------------------------------------------------------
    [Task]
    bool IsAttacked()
    {
        return is_attacked;
    }
    //-------------------------------------------------------
    [Task]
    bool IsVisibleTarget()
    {
        return perspective.IsPlayerVisible();
    }

    //-------------------------------------------------------
    [Task]
    bool HasEnemy()
    {
        //수정필요
        return hasEnemy;
    }

    //-------------------------------------------------------
    [Task]
    bool IsOutOfBounds()
    {
        hasEnemy = false;

        //수정필요
        return isOutOfBounds;
    }
    //-------------------------------------------------------

    // Vector3.Distance로 공격 가능 범위인지 판단
    [Task]
    bool IsTargetInAttackRange()
    {
        //보이지 않으면 공격범위로 두지 않는다.
        if (!IsVisibleTarget()) return false;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("d = {0}", Vector3.Distance(transform.position,
                targetTransform.position));

        return Vector3.Distance(transform.position, targetTransform.position) <= attackRange;
    }

    //-------------------------------------------------------
    [Task]
    void LookAtDestination()
    {
        Vector3 targetDelta = destination - this.transform.position;
        Vector3 targetDir = targetDelta.normalized;
        
        Quaternion targetRot = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
               rotateSpeed * Time.deltaTime);

        if (Vector3.Angle(targetDir, this.transform.forward) < 10.0f)
        {
            Task.current.Succeed();
        }

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("angle = {0}", Vector3.Angle(targetDir,
                this.transform.forward));

    }
    //-------------------------------------------------------
    [Task]
    void IsTargetThreaten(float duration)
    {
        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
        }

        ani.Play("Threaten");

        elapsedTime += Time.deltaTime;

        var t = duration - elapsedTime;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            hasEnemy = true;

            Task.current.Succeed();
        }
    }
    //-------------------------------------------------------
    [Task]
    public bool SetDestination(Vector3 p)
    {
        navMeshAgent.isStopped = true;

        destination = p;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("{0}, {1}, {2}",
                destination.x, destination.y, destination.z);

        return true;
    }

    //-------------------------------------------------------
    [Task]
    bool SetDestination()
    {
        return true;
    }

    //-------------------------------------------------------
    [Task]
    bool SetRandomDestination()
    {
        Vector3 randomPos = RandomNavSphere(transform.position,wanderRange, -1);

        SetDestination(randomPos + new Vector3(0.0f, 1.0f, 0.0f));

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("{0}, {1}, {2}",
                destination.x, destination.y, destination.z);

        return true;
    }

    //-------------------------------------------------------
    [Task]
    bool SetDestination_Enemy()
    {
        if (targetTransform != null)
        {
            SetDestination(targetTransform.position);
            return true;
        }
        else
            return false;
    }
    //-------------------------------------------------------
    [Task]
    void SetDestination_ChaseTarget()
    {
        if (targetTransform != null)
        {
            SetDestination(targetTransform.position);

            Task.current.Succeed();
        }
    }

    //-------------------------------------------------------
    [Task]
    void MoveToDestination()
    {
        if (navMeshAgent != null)
            navMeshAgent.SetDestination(destination);

        float dist = Vector3.Distance(destination, transform.position);
        
        if (dist <= 2.0f)
        {
            Task.current.Succeed();
            navMeshAgent.isStopped = true;
        }
        else
        {
            if (navMeshAgent.speed <= moveSpeed)
                ani.Play("Walk");
            else
                ani.Play("Run");

            navMeshAgent.isStopped = false;
        }
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("dist={0}", dist);
    }
    //-------------------------------------------------------
    [Task]
    void MoveToDestination(float duration)
    {
        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
        }

        if (navMeshAgent != null)
            navMeshAgent.SetDestination(destination);
        elapsedTime += Time.deltaTime;

        float dist = Vector3.Distance(destination, transform.position);

        if (dist <= 2.0f)
        {
            Task.current.Succeed();
            navMeshAgent.isStopped = true;
        }
        else
        {
            if (navMeshAgent.speed <= moveSpeed)
                ani.Play("Walk");
            else
                ani.Play("Run");

            navMeshAgent.isStopped = false;
        }
        elapsedTime += Time.deltaTime;

        var t = duration - elapsedTime;

        if (t <= 0)
        {
            Task.current.Succeed();
        }

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("dist={0}", dist);
    }

    //-------------------------------------------------------
    [Task]
    void Idle(float duration)
    {
        navMeshAgent.isStopped = true;

        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
        }

        ani.Play("Idle");

        elapsedTime += Time.deltaTime;

        var t = duration - elapsedTime;
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
        }

    }

    //-------------------------------------------------------
    [Task]
    void AttackTarget(float AttackSpeed)
    {
        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
        }

        ani.Play("Attack");

        elapsedTime += Time.deltaTime;


        if (elapsedTime > 0.5f && elapsedTime < 0.7f)
            AttackRange.enabled = true;
        if (elapsedTime > 0.7f)
            AttackRange.enabled = false;


        var t = AttackSpeed - elapsedTime;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
            AttackRange.enabled = false;
        }
    }    
    //-------------------------------------------------------
    [Task]
    void WaitIdle(float duration)
    {
        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
        }

        ani.Play("Wait");

        elapsedTime += Time.deltaTime;

        var t = duration - elapsedTime;
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
        }
    }
    //-------------------------------------------------------
    [Task]
    void AttackedImpact()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            if (!hasEnemy)
                hasEnemy = true;

            is_attacked = false;
            Task.current.Succeed();
        }
    }
    //-------------------------------------------------------
    [Task]
    void DeadAndDestroy(float duration)
    {
        if (Task.current.isStarting)
        {
            deadDelayTime = -Time.deltaTime;
            ani.Play("Dead");
            SoundManager.instance.PlaySFX(deadsound);
            EventManager.Instance.PostNotification(EVENT_TYPE.ENEMY_KILLED, this, this.ID);
            LevelManager.instance.AddExperience(this.exp);

            if (this.gameObject.GetComponent<UIShower>() != null)
            {
                this.gameObject.GetComponent<UIShower>().is_active = false;
            }
            is_dead = true;
        }

        deadDelayTime += Time.deltaTime;

        var t = duration - deadDelayTime;
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
            this.gameObject.SetActive(false);
            Invoke("Respawn", 0.5f);
        }
    }
    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------
    [Task]
    void AttackDouble(float AttackSpeed)
    {

        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
            doubleAttack = false;
        }

        ani.Play("Attack");

        elapsedTime += Time.deltaTime;


        if (elapsedTime > 0.5f && elapsedTime < 0.7f && doubleAttack == false)
            AttackRange.enabled = true;
        if (elapsedTime > 0.7f && doubleAttack == false)
        {
            AttackRange.enabled = false;
            doubleAttack = true;
        }

        if (elapsedTime > 1.1f && elapsedTime < 1.3f && doubleAttack == true)
            AttackRange.enabled = true;
        if (elapsedTime > 1.3f && doubleAttack == true)
        {
            AttackRange.enabled = false;
        }


        var t = AttackSpeed - elapsedTime;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
            AttackRange.enabled = false;
        }
    }
    //-------------------------------------------------------
    [Task]
    void BossAttack(float duration)
    {
        if (Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;
            Invoke("BossAttackSound", 1.1f);
        }

        ani.Play("Attack");

        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1.0f && elapsedTime < 1.45f)
            AttackRange.enabled = true;
        if (elapsedTime > 1.45f)
            AttackRange.enabled = false;


        var t = duration - elapsedTime;

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", t);

        if (t <= 0)
        {
            Task.current.Succeed();
            AttackRange.enabled = false;
        }
    }
    //-------------------------------------------------------
    [Task]
    void BossSkill(int skillNum)
    {
        if (this.ID == 301 && Task.current.isStarting)
        {
            elapsedTime = -Time.deltaTime;

            switch (skillNum)
            {
                case 1:
                    ani.Play("Skill1");
                    Invoke("BossSkill1", 1.3f);
                    SoundManager.instance.PlaySFX("GroundSmash", 0.4f);
                    SoundManager.instance.PlaySFX("Spire", 0.8f);
                    break;
            }
        }

        elapsedTime += Time.deltaTime;


        if (elapsedTime >= 2.5f)
        {
            Task.current.Succeed();
        }
    }
    //-------------------------------------------------------
    [Task]
    void Roaring()
    {
        if (Task.current.isStarting)
        {
            SoundManager.instance.StopSFX("BossRoaring");
            SoundManager.instance.PlaySFX("BossRoaring");
        }

        Task.current.Succeed();
    }
    //-------------------------------------------------------
    [Task]
    void NotificationGameEnd()
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.GAME_END, this, null);
    }
    //-------------------------------------------------------
    [Task]
    bool IsUsingSkill()
    {
        return is_usingSkill;
    }
    //-------------------------------------------------------
    void BossSkill1()
    {
        this.transform.GetComponent<BossSkill>().ActiveSkill(1);
    }
    //-------------------------------------------------------
    private void BossAttackSound()
    {
        SoundManager.instance.PlaySFX("BossAttack");
    }
    #endregion

}

