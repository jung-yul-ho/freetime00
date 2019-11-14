using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PHASE
{
    PHASE_TARGETTING_ATTACK,
    PHASE_ICESPEAR,
    PHASE_DEFENSIVE,
    PHASE_OFFENSIVE,
    PHASE_EMERGENCY
}

public class BossActivity : Character
{
    public EnermyState enermystate;

    public PHASE phase;

    public Transform monsterTr;
    public Transform playerTr;
    public NavMeshAgent nvAgent;
    public Animator Ani;
    public Rigidbody rigidbody;

    public float TraceDist = 40.0f;
    public float attackDist = 20.0f;

    public bool die = false;

    public float AttackDelay;

    //보스 세부정보
    public int BossMaxHp;
    public string BossName;

    //보스무적상태 체크
    public bool unbeatable;
    public float NuckBackCount;

    //보스 넘버
    //보스와 관련된 hp, 공격력등 자료를 불러오는데 사용
    public int BossNumber;
    //보스방향
    public float direction;
    //보스사망시 몇초후에 시체가 삭제되는지 계산하는 자료
    public int diecount;
    //지속데미지 관련 자료
    public float ContinueDamageDelay = 0;

    //적캐릭터의  능력치
    public int Hp;
    public int Atk;
    public int Def;

    static BossActivity instance;
    public static BossActivity Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BossActivity>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<BossActivity>();
                }
            }
            return instance;
        }
    }

    void FixedUpdate()
    {
        //죽었을때 시체가 사라지는 카운트
        if (die == true)
        {
            diecount++;
            if (diecount >= 100)
            {
                Destroy(gameObject);
            }
        }
        //공격범위 내애 플레이어가 존재할때
        else if(enermystate == EnermyState.ENERMY_ATTACK)
        {
            if(unbeatable == false)
            {
                if (AttackDelay <= 0.0f)
                {
                    Attack();
                }
                else
                {
                    AttackDelay -= 0.1f;
                }
            }
        }
        else if(unbeatable == true)
        {
            NuckBackCount -= 0.1f;
            if (NuckBackCount <= 0)
            {
                BossReorganization();
            }
        }
        //지속데미지 관련 코드
        if (ContinueDamageDelay > 0)
        {
            ContinueDamageDelay -= 0.1f;
        }
    }

    void Start()
    {
        monsterTr = gameObject.GetComponent<Transform>();
        playerTr = Player.Instance.gameObject.transform;
        nvAgent = gameObject.GetComponent<NavMeshAgent>();

        nvAgent.enabled = false;

        Ani = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(CheckEnermyState());

        NuckBackCount = 0;
        diecount = 0;
        GameDataEngine.Instance.LoadBossData(gameObject);
        unbeatable = false;
    }

    IEnumerator CheckEnermyState()
    {
        while (die == false)
        {
            yield return new WaitForSeconds(0.2f);
            direction = Vector3.Distance(playerTr.position, monsterTr.position);
            if (nvAgent.enabled == true && unbeatable == false)
            {
                if (direction <= attackDist)
                {
                    enermystate = EnermyState.ENERMY_ATTACK;
                    //GameEingine.Instance.StartCinematic(this);
                }
                else if (direction <= TraceDist)
                {
                    enermystate = EnermyState.ENERMY_MOVE;
                    Ani.SetBool("Moving", true);
                    nvAgent.destination = playerTr.position;
                }
                else
                {
                    enermystate = EnermyState.ENERMY_IDLE;
                }
            }
            else if (nvAgent.enabled == true)
            {
                nvAgent.Stop();
                Ani.SetBool("Moving", false);
                enermystate = EnermyState.ENERMY_IDLE;
            }
        }
    }

    void ResetAni()
    {
        Ani.SetBool("Moving", false);
        Ani.SetBool("Attack", false);
        Ani.SetBool("Damage1", false);
        enermystate = EnermyState.ENERMY_IDLE;
    }

    //피격판정
    private void OnTriggerEnter(Collider other)
    {
        if (unbeatable == false)
        {
            if (other.gameObject.tag == "Weapon" && die == false)
            {
                Damage(PlayerInformation.Instance.Atk);
                PhaseChange();
                if (Hp <= 0)
                {
                    Bosscontrol.Instance.BossDie();
                }
            }
            else if (other.gameObject.tag == "Magic" && die == false && other.gameObject.layer != 17)
            {
                //getcomponent 안쓰는 방법 생각해보기
                Damage((int)other.gameObject.GetComponent<Effect>().EffectPower);
                PhaseChange();
                if (Hp <= 0)
                {
                    Bosscontrol.Instance.BossDie();
                }
            }
        }
    }

    //바닥에 닿였을때 애니메이션과 네비게이션 활성화
    private void OnCollisionEnter(Collision collision)
    {
        Ani.enabled = true;
        nvAgent.enabled = true;
    }

    private void PhaseChange()
    {
        if(phase == PHASE.PHASE_TARGETTING_ATTACK && Hp < 400)
        {
            BossDamaged();
            phase = PHASE.PHASE_ICESPEAR;
        }
        else if(phase == PHASE.PHASE_ICESPEAR && Hp < 300)
        {
            BossDamaged();
            phase = PHASE.PHASE_DEFENSIVE;
        }
        else if (phase == PHASE.PHASE_DEFENSIVE && Hp < 200)
        {
            BossDamaged();
            phase = PHASE.PHASE_OFFENSIVE;
        }
        else if (phase == PHASE.PHASE_OFFENSIVE && Hp < 100)
        {
            BossDamaged();
            phase = PHASE.PHASE_EMERGENCY;
        }
    }

    private void BossDamaged()
    {
        ResetAni();
        unbeatable = true;
        Ani.SetBool("Damage1", true);
        NuckBackCount = 20.0f;
        if (phase == PHASE.PHASE_TARGETTING_ATTACK)
        {


        }
    }

    private void BossReorganization()
    {
        NuckBackCount = 0;
        unbeatable = false;
        ResetAni();
    }

    private void Attack()
    {
        gameObject.transform.LookAt(playerTr);
        if(phase == PHASE.PHASE_TARGETTING_ATTACK)
        {
            SummonFairy();
        }
        else if(phase == PHASE.PHASE_ICESPEAR)
        {
            IceSpear();
        }
        else if (phase == PHASE.PHASE_DEFENSIVE)
        {
            IceShower();
        }
        else if (phase == PHASE.PHASE_OFFENSIVE)
        {
            SummonFairy();
            IceSpear();
        }
        else if (phase == PHASE.PHASE_EMERGENCY)
        {
            IceShower();
            SummonFairy();
            IceSpear();
        }
    }

    private void IceSpear()
    {
        Ani.SetBool("Attack", true);
        Ani.SetBool("Moving", false);
        Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[2]);
        neweffect.magiccontroler = MagicControler.CONTROLER_ENERMY;
        neweffect.summoneffect = 3;
        neweffect.transform.position = PlayerControl.Instance.transform.position;
        neweffect.gameObject.SetActive(true);
        AttackDelay += 20.0f;
    }

    private void SummonFairy()
    {
        Ani.SetBool("Attack", true);
        Ani.SetBool("Moving", false);
        AttackDelay += 10.0f;
        Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[4]);
        neweffect.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        neweffect.transform.rotation = transform.rotation;
        neweffect.transform.Rotate(+90, 0, 0);
        neweffect.transform.Translate(transform.forward, Space.World);
        neweffect.gameObject.SetActive(true);
        neweffect.GetComponent<MissileControler>().Player = gameObject;

        //PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        //GameEingine.Instance.Player.FireBallDelay = 10.0f;
    }

    private void IceShower()
    {
        Ani.SetBool("Attack", true);
        Ani.SetBool("Moving", false);
        AttackDelay += 10.0f;
        Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[5]);
        neweffect.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        neweffect.gameObject.transform.LookAt(playerTr);
        //neweffect.transform.rotation = transform.rotation;
        //neweffect.transform.Rotate(+90, 0, 0);
        neweffect.gameObject.SetActive(true);

        //PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        //GameEingine.Instance.Player.FireBallDelay = 10.0f;
    }

    //지속데미지가 들어가는가?
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Magic" && other.layer != 17)
        {
            if (other.gameObject.GetComponent<Effect>().EffectType == EffectType.EFFECT_AREA)
            {
                if (ContinueDamageDelay <= 0)
                {
                    ContinueDamageDelay = 5.0f;
                    Hp -= (int)(other.gameObject.GetComponent<Effect>().EffectPower);
                }
            }
        }
    }
}
