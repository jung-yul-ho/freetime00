using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Animations;


public enum PlayerState
{
    PLAYER_IDLE,
    PLAYER_MOVE,
    PLAYER_ATTACK,
    //PLAYER_SHIELDATTACK,
    PLAYER_SHOT,
    PLAYER_UNDERATTACK,
    PLAYER_PROTECT,
}


public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 10;
    public PlayerState playerstate;
    public int ReloadCount;
    public bool ShottingPosition;
    public Animator Ani;
    public GameObject[] MissileFrefab;
    public int UsingMissileNumber;
    public GameObject gun;
    public GameObject Sword;
    public Player RealCharacter;

    //파이어볼날릴때 레이저 쏘아올리는것
    public LineRenderer linerenderer;

    //에어리어스킬 사용할때 필요한것
    public GameObject AreaSkillArea;
    public bool AreaSkillPrepare;

    //타게킹관련 
    public EnermyControl TargetEnermy;
    public float TargetDirection;

    //도입할까 고민중
    //이동한 시간(카메라 워크에 필요함)
    public float MoveCount;

    //콤보보조
    public bool _attackFlag;
    public float ComboRemainTime;

    static PlayerControl instance;

    void Start()
    {
        Ani = GetComponent<Animator>();
        UsingMissileNumber = 0;

        linerenderer = GetComponent<LineRenderer>();
        linerenderer.enabled = false;
        linerenderer.SetWidth(0.1f, 0.1f);
        ShottingPosition = false;
        _attackFlag = true;
    }


    public static PlayerControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayerControl>();
                }
            }
            return instance;
        }
    }

    void Update()
    {
        AniControl();
        //에어리어스킬 사용시 그 범위가 전방으로 계속 이동
        if (AreaSkillPrepare == true)
        {
            AreaSkillArea.transform.Translate(Vector3.forward * Time.deltaTime * 20f);
        }
        //사격관련 코드
        if (playerstate == PlayerState.PLAYER_SHOT)
        {
            if (ShottingPosition == false)
            {

            }
            else
            {

            }
            Ray ray = new Ray(gun.transform.position, gun.transform.forward);
            RaycastHit hit;

            linerenderer.enabled = true;
            linerenderer.SetPosition(0, ray.origin);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                linerenderer.SetPosition(1, hit.point);
            }
            else
            {
                linerenderer.SetPosition(1, ray.GetPoint(100.0f));
            }
        }
        else
        {
            linerenderer.enabled = false;
        }

        //타게팅관련 코드
        if(TargetEnermy != null && TargetEnermy.die == false)
        {
            //타게팅해제만 플레이어 컨트롤이 설정함 
            //타게팅설정은 enermycontrol
            TargetDirection = Vector3.Distance(RealCharacter.transform.position, TargetEnermy.transform.position);
            if (TargetDirection >= 30.0f || TargetEnermy.die == true)
            {
                TargetEnermy = null;
            }
        }
        else if(GameEingine.Instance.BossAppear == true)
        {
            TargetDirection = Vector3.Distance(transform.position, Bosscontrol.Instance.charObj.transform.position);
        }

        //공격방향조정코드
        if(playerstate == PlayerState.PLAYER_ATTACK)
        {
            if (TargetEnermy != null)
            {
                Player.Instance.transform.rotation = Quaternion.Lerp(Player.Instance.transform.rotation, Quaternion.LookRotation(TargetEnermy.transform.position - Player.Instance.transform.position), Time.deltaTime * 5);
            }
            else if (GameEingine.Instance.BossAppear == true && TargetDirection <= 40)
            {
                Player.Instance.transform.LookAt(Bosscontrol.Instance.charObj.transform);
            }
        }
    }

    //애니메이션작동
    void AniControl()
    {
        if(playerstate == PlayerState.PLAYER_ATTACK)
        {
            //Sword.tag = "Weapon";
            Ani.SetBool("Attack", true);
        }
        else if (playerstate == PlayerState.PLAYER_MOVE)
        {
            ResetAni();
            if(MoveCount < 5.0f)
            {
                MoveCount += 0.01f;
            }
            Ani.SetBool("Moving", true);
        }
        else if(playerstate == PlayerState.PLAYER_PROTECT)
        {
            Ani.SetBool("Protect", true);
        }
        //플레이어 상태를 리셋시키는 조건
        else if(playerstate != PlayerState.PLAYER_SHOT)
        {
            ResetAni();
            if(MoveCount >= 0)
            {
                MoveCount -= 0.01f;
            }
        }
    }

    //모든 애니메이션을 중지시키고 idle로 복귀시킴
    public void ResetAni()
    {
        Sword.tag = "Player";
        Ani.SetBool("Moving", false);
        Ani.SetBool("Attack", false);
        Ani.SetBool("Protect", false);
        Ani.SetBool("Attack2", false);
        Ani.SetFloat("Combo", 0.0f);
        playerstate = PlayerState.PLAYER_IDLE;
    }

    //일반공격
    public void Attack()
    {
        if(_attackFlag == true)
        {
            _attackFlag = false;
            if (Ani.GetFloat("Combo") < 3.0f)
            {
                if (Ani.GetFloat("Combo") == 0.0f)
                {
                    Ani.SetFloat("Combo", 1.0f);
                }
                else if (Ani.GetFloat("Combo") == 1.0f)
                {
                    Ani.SetFloat("Combo", 2.0f);
                }
                else if (Ani.GetFloat("Combo") == 2.0f)
                {
                    Ani.SetFloat("Combo", 3.0f);
                }
                playerstate = PlayerState.PLAYER_ATTACK;
                StartCoroutine(CheckForCombo(Ani.GetFloat("Combo")));
            }
        }
    }

    //주의
    //공격범위 재설정 필요 현제 플레이어 주위 그 자체를 공격범위로 생각중
    private void CreateAttackColliders()
    {
        //칼위치를 기준
        //Vector3 CharacterFront = new Vector3(Sword.transform.position.x, Sword.transform.position.y, Sword.transform.position.z);
        //플레이어 앞을 기준
        Vector3 CharacterFront = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, Player.Instance.transform.position.z);
        var Colliders = Physics.OverlapSphere(CharacterFront, 2.0f);

        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 17)
            {
                Colliders[i].GetComponent<EnermyControl>().Damage(PlayerInformation.Instance.Atk);
            }
        }
    }
    private void AttackFlagReset()
    {
        _attackFlag = true;
    }

    //콤보딜레이 조정 코드
    private IEnumerator CheckForCombo(float ComboLevel)
    {
        yield return new WaitForSeconds(1.5f);
        if(ComboLevel == Ani.GetFloat("Combo"))
        {
            Ani.SetFloat("Combo", 0.0f);
            Ani.SetBool("Attack", false);
            playerstate = PlayerState.PLAYER_IDLE;
        }
        yield break;

    }

    //private void PlayNextCombo()
    //{
    //    Ani.SetTrigger("AttackTrigger"[_nextComboAttackIndex]);
    //    _nextComboAttackIndex = (_nextComboAttackIndex + 1) % "AttackTrigger".Length;
    //}



    //쉴드씌우기
    public void Protect()
    {
        if(RealCharacter.ShieldDelay <= 0 && GameEingine.Instance.GameOver == false && (playerstate == PlayerState.PLAYER_IDLE || playerstate == PlayerState.PLAYER_MOVE))
        {
            playerstate = PlayerState.PLAYER_PROTECT;
            PlayerInformation.Instance.Shield = 30;
            RealCharacter.ShieldDelay = 10.0f;
        }
    }
}