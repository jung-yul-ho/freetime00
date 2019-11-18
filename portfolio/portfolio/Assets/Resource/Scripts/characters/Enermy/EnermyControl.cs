using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnermyState
{
    ENERMY_IDLE,
    ENERMY_MOVE,
    ENERMY_ATTACK,
    ENERMY_DIE
}

public class EnermyControl : Character
{
    //캐릭터의 상태
    public EnermyState enermystate;

    //캐릭터의 위치정보
    public Transform monsterTr;
    public Transform playerTr;
    public NavMeshAgent nvAgent;
    public float Targeting = 25.0f;
    public float TraceDist = 20.0f;
    public float attackDist = 3.0f;

    //캐릭터의 애니메이션
    public Animator Ani;

    //캐릭터의 세부정보
    public int EnermyNumber;
    public string Enermyname;
    public float MaxHp;
    
    //제거고민중
    public float direction;
    public int NuckBackCount;
    public int diecount;
    public int DelayTime = 0;
    
    //캐릭터의 사망여부
    public bool die = false;

    //에너지바가 표시될 곳
    public GameObject HpbarPos;

    //리스폰된 위치정보
    public RespownPoint respownpoint;

    //적캐릭터의  능력치
    public int Hp;
    public int Atk;
    public int Def;

    void Start()
    {
        monsterTr = gameObject.GetComponent<Transform>();
        playerTr = Player.Instance.gameObject.transform;
        nvAgent = gameObject.GetComponent<NavMeshAgent>();

        Ani = GetComponent<Animator>();

        StartCoroutine(CheckEnermyState());

        StartCoroutine(EnermyAction());

        NuckBackCount = 0;
        diecount = 0;
    }

    void FixedUpdate()
    {
        if(die == true)
        {
            diecount++;
            if (diecount >= 100)
            {
                respownpoint.RespownPointUsingCheck = false;
                Destroy(gameObject);
            }
        }
        if (Hp <= 0)
        {
            Die();
        }
    }

    IEnumerator CheckEnermyState()
    {
        while(die == false)
        {
            yield return new WaitForSeconds(0.2f);
            direction = Vector3.Distance(playerTr.position, monsterTr.position);
            if (nvAgent.enabled == true)
            {
                //플레이어의 타게팅이 이 캐릭터를 향하게 한다
                if(PlayerControl.Instance.TargetEnermy == null && direction <= Targeting && die == false)
                {
                    PlayerControl.Instance.TargetEnermy = this;
                }
                if (direction <= attackDist)
                {
                    enermystate = EnermyState.ENERMY_ATTACK;
                    nvAgent.destination = playerTr.position;
                    transform.LookAt(playerTr.position);
                    //GameEingine.Instance.StartCinematic(this);
                }
                else if (direction <= TraceDist)
                {
                    enermystate = EnermyState.ENERMY_MOVE;
                    nvAgent.destination = playerTr.position;
                }
                else
                {
                    enermystate = EnermyState.ENERMY_IDLE;
                }
            }
            else if(nvAgent.enabled == false)
            {
                //nvAgent.Stop();
                Ani.SetBool("Moving", false);
                enermystate = EnermyState.ENERMY_IDLE;
            }
        }
    }
    
    IEnumerator EnermyAction()
    {
        while (die == false)
        {
            switch (enermystate)
            {
                case EnermyState.ENERMY_IDLE:
                    if (nvAgent.enabled == true)
                    {
                        //nvAgent.Stop();
                        Ani.SetBool("Moving", false);
                        Ani.SetBool("Attack", false);
                    }
                    break;
                case EnermyState.ENERMY_MOVE:
                    if (nvAgent.enabled == true)
                    {
                        //nvAgent.Resume();
                        Ani.SetBool("Moving", true);
                        Ani.SetBool("Attack", false);
                    }
                    break;
                case EnermyState.ENERMY_ATTACK:
                    if (nvAgent.enabled == true)
                    {
                        //nvAgent.Stop();
                        Ani.SetBool("Attack", true);
                        Ani.SetBool("Moving", false);
                    }
                    break;
            }
            yield return null;
        }
    }



    //모든 애니메이션세팅을 중지시키고 idle로 복귀시킴
    void ResetAni()
    {
        Ani.SetBool("Moving", false);
        Ani.SetBool("Attack", false);
        enermystate = EnermyState.ENERMY_IDLE;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            StartCoroutine(WaitForNvagent());
        }
    }

    IEnumerator WaitForNvagent()
    {
        yield return new WaitForSeconds(0.8f);
        nvAgent.enabled = true;
    }

    //스킬범위안에있을때 지속대미지 구현
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Magic" && die == false)
        {
            Damage(other.GetComponent<Effect>().EffectPower);
        }
    }

    public override void Damage(float Damage)
    {
        if (Damage > Def)
        {
            Hp -= (int)(Damage - Def);
        }
        else
        {
            Hp -= 1;
        }
    }

    void Die()
    {
        if(die == false)
        {
            GameEingine.Instance.EnermyCount--;
            ResetAni();
            die = true;
            nvAgent.Stop();
            enermystate = EnermyState.ENERMY_DIE;
            GameEingine.Instance.KillCount++;
            Ani.SetBool("Die", true);
            //내가 플레이어의 타겟일때 타겟 해제
            if (PlayerControl.Instance.TargetEnermy == this)
            {
                PlayerControl.Instance.TargetEnermy = null;
            }
            //사망시 플레이어에게 포션을 제공
            //숫자는 아이템 넘버
            int itemnumber = Random.Range(1, 8);
            //아이템주기
            Inventory.Instance.InItItem(itemnumber);
            GameUiEingine.Instance.RemainMonsterCount.text = GameEingine.Instance.RemainMonsterCount.ToString();
            GameUiEingine.Instance.RealMonsterCount.text = GameEingine.Instance.EnermyCount.ToString();
            if (PlayerControl.Instance.TargetEnermy == this)
            {
                PlayerControl.Instance.TargetEnermy = null;
            }
        }
    }
}
