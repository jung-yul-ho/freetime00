using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUiEingine : MonoBehaviour
{
    //게임 인터페이스 보조
    public Text PlayerHpText;
    public Text KillCountText;
    public Text RemainMonsterCount;
    public Text RealMonsterCount;
    public Slider PlayerHpSlider;
    public Image PlayerHpSliderFill;
    public Text BossHpText;
    public Slider BossHpSlider;
    public Image BossHpSliderFill;

    public Button Attack;
    public Image AttackSupport;
    public Button protect;
    public Image protectsupport;

    //섬세한 상호작용이 필요한 스킬
    public GameObject FireBall;
    public Image FireballSupport;
    public GameObject FireField;
    public Image FireFieldSupport;
    public GameObject Consume;
    public Text ConsumeCount;
    public Image ConsumeSupport;
    public Text ConsumeCountTextUi;

    //빠른물약 이미지 보조
    public Sprite Postion;
    public Sprite DisablePostion;

    static GameUiEingine instance;
    public static GameUiEingine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameUiEingine>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<GameUiEingine>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        //파이어볼을 세팅해주는 코드들
        EventTrigger FireBallEvent = FireBall.AddComponent<EventTrigger>();

        EventTrigger.Entry FireBall_PointerDown = new EventTrigger.Entry();
        FireBall_PointerDown.eventID = EventTriggerType.PointerDown;
        FireBall_PointerDown.callback.AddListener((data) => { FireBallStart((PointerEventData)data); });
        FireBallEvent.triggers.Add(FireBall_PointerDown);

        EventTrigger.Entry FireBall_Drag = new EventTrigger.Entry();
        FireBall_Drag.eventID = EventTriggerType.Drag;
        FireBall_Drag.callback.AddListener((data) => { ShotPositionChange((PointerEventData)data); });
        FireBallEvent.triggers.Add(FireBall_Drag);

        EventTrigger.Entry FireBall_PointerUp = new EventTrigger.Entry();
        FireBall_PointerUp.eventID = EventTriggerType.PointerUp;
        FireBall_PointerUp.callback.AddListener((data) => { FireBallEnd((PointerEventData)data); });
        FireBallEvent.triggers.Add(FireBall_PointerUp);

        //불지옥을 세팅해주는 코드들
        EventTrigger FlareFieldEvent = FireField.AddComponent<EventTrigger>();

        EventTrigger.Entry FlarField_PointerDown = new EventTrigger.Entry();
        FlarField_PointerDown.eventID = EventTriggerType.PointerDown;
        FlarField_PointerDown.callback.AddListener((data) => { AreaStart((PointerEventData)data); });
        FlareFieldEvent.triggers.Add(FlarField_PointerDown);

        EventTrigger.Entry FlarField_PointerUp = new EventTrigger.Entry();
        FlarField_PointerUp.eventID = EventTriggerType.PointerUp;
        FlarField_PointerUp.callback.AddListener((data) => { AreaActive((PointerEventData)data); });
        FlareFieldEvent.triggers.Add(FlarField_PointerUp);
    }

    public void Update()
    {
        //플레이어hp바 조정
        if (PlayerInformation.Instance.Shield <= 0)
        {
            PlayerHpSliderFill.color = Color.white;
            PlayerHpText.text = PlayerInformation.Instance.Hp.ToString();
            float hp = PlayerInformation.Instance.Hp / 100f;
            PlayerHpSlider.value = hp;
        }
        else
        {
            PlayerHpSliderFill.color = Color.blue;
            PlayerHpText.text = PlayerInformation.Instance.Shield.ToString();
            float hp = PlayerInformation.Instance.Shield / 30f;
            PlayerHpSlider.value = hp;
        }

        //보스 hp바 조정
        if(GameEingine.Instance.BossAppear == true)
        {
            KillCountText.gameObject.SetActive(false);
            RemainMonsterCount.gameObject.SetActive(false);
            RealMonsterCount.gameObject.SetActive(false);
            BossHpSlider.gameObject.SetActive(true);
            //
            if (BossActivity.Instance.Hp > 400)
            {

            }
            else if(BossActivity.Instance.Hp > 300)
            {

            }
            else if (BossActivity.Instance.Hp > 200)
            {

            }
            else if (BossActivity.Instance.Hp > 100)
            {

            }
            else
            {

            }
            float hp = BossActivity.Instance.Hp / BossActivity.Instance.BossMaxHp;
            BossHpSlider.value = hp;
            BossHpText.text = BossActivity.Instance.Hp.ToString();
        }

        //스킬버튼 딜레이 조정
        //공격버튼
        if (Player.Instance.AttackDelay != 0)
        {
            //AttackSupport.color = Color.red;
            AttackSupport.fillAmount = (1.0f - Player.Instance.AttackDelay) / 1.0f;
        }
        else
        {
            AttackSupport.color = Color.white;
        }
        //불지옥버튼
        if (Player.Instance.FireFieldDelay != 0)
        {
            //AttackSupport.color = Color.red;
            FireFieldSupport.fillAmount = (10.0f - Player.Instance.FireFieldDelay) / 10.0f;
        }
        else
        {
            FireFieldSupport.color = Color.white;
        }
        //파이어볼버튼
        if (Player.Instance.FireBallDelay != 0)
        {
            //AttackSupport.color = Color.red;
            FireballSupport.fillAmount = (10.0f - Player.Instance.FireBallDelay) / 10.0f;
        }
        else
        {
            FireballSupport.color = Color.white;
        }
        //보호막버튼
        if (Player.Instance.ShieldDelay != 0)
        {
            //ShieldSupport.color = Color.red;
            protectsupport.fillAmount = (10.0f - Player.Instance.ShieldDelay) / 10.0f;
        }
        else
        {
            protectsupport.color = Color.white;
        }
    }

    //플레이어와 게임내 버튼을 연결하는 코드들
    public void setting(PlayerControl player)
    {
        Attack.onClick.AddListener(delegate () { player.Attack(); });
        protect.onClick.AddListener(delegate () { player.Protect(); });
    }

    //파이어볼 생성코드
    void FireBallStart(PointerEventData data)
    {
        if(PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE)
        {
            PlayerControl.Instance.ResetAni();
        }
        if(GameEingine.Instance.GameOver == false && (PlayerControl.Instance.playerstate == PlayerState.PLAYER_IDLE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE) && Player.Instance.FireBallDelay <= 0.0f)
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_SHOT;
            Vector3 screenPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        }
    }

    void FireBallEnd(PointerEventData data)
    {
        if (GameEingine.Instance.GameOver == false && PlayerControl.Instance.playerstate == PlayerState.PLAYER_SHOT && Player.Instance.FireBallDelay <= 0.0f)
        {
            Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[1]);
            neweffect.transform.position = PlayerControl.Instance.gun.transform.position;
            neweffect.transform.rotation = PlayerControl.Instance.gun.transform.rotation;
            neweffect.transform.Rotate(+90, 0, 0);
            neweffect.transform.Translate(PlayerControl.Instance.gun.transform.forward, Space.World);
            neweffect.gameObject.SetActive(true);
            neweffect.GetComponent<MissileControler>().Player = gameObject;
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
            Player.Instance.FireBallDelay = 10.0f;
        }

    }

    //에어리어기술 생성코드
    void AreaStart(PointerEventData data)
    {
        if (GameEingine.Instance.GameOver == false && (PlayerControl.Instance.playerstate == PlayerState.PLAYER_IDLE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE) && Player.Instance.FireFieldDelay <= 0.0f)
        {
            PlayerControl.Instance.AreaSkillPrepare = true;
            PlayerControl.Instance.AreaSkillArea.SetActive(true);
            if(PlayerControl.Instance.TargetEnermy != null)
            {
                PlayerControl.Instance.AreaSkillArea.transform.LookAt(PlayerControl.Instance.TargetEnermy.transform);
            }
            else if(GameEingine.Instance.BossAppear == true)
            {
                PlayerControl.Instance.AreaSkillArea.transform.LookAt(Bosscontrol.Instance.charObj.transform);
            }
        }
    }

    void AreaActive(PointerEventData data)
    {
        if (GameEingine.Instance.GameOver == false && Player.Instance.FireFieldDelay <= 0.0f)
        {
            Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[0]);
            neweffect.transform.position = PlayerControl.Instance.AreaSkillArea.transform.position;
            neweffect.gameObject.SetActive(true);
            PlayerControl.Instance.AreaSkillPrepare = false;
            PlayerControl.Instance.AreaSkillArea.transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 0.4f, Player.Instance.transform.position.z);
            PlayerControl.Instance.AreaSkillArea.SetActive(false);
            Player.Instance.FireFieldDelay = 10.0f;
        }
    }

    public void UseFastPotion()
    {
        if(Equipment.Instance.ConsumeSlot.InitItem == null)
        {
            MessageBox.Instance.ShowMessage("빠른물약이 설정되어있지 않습니다");
        }
        else
        {
            Equipment.Instance.ConsumeSlot.InitItem.UsingItem();
            Equipment.Instance.CheckForConsumeUi();
        }
    }

    //마우스의 움직임에따라 플레이어캐릭터이 바라보는 방향이 달라짐
    void ShotPositionChange(PointerEventData data)
    {
        if (GameEingine.Instance.GameOver == false)
        {
            float horizontal = Input.GetAxis("Mouse X") * 5;
            Player.Instance.transform.Rotate(0, horizontal, 0);
        }
    }

    public void Quit()
    {
        SaveLoad.Instance.Save();

        Application.Quit();
    }
}