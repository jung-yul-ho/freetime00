using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEingine : MonoBehaviour
{
    //게임내 필수자료
    public bool GameOver = false;
    public PlayerControl PlayerControler;
    public GameObject PlayerPrefab;
    public EnermyControl[] Enermies;
    public GameObject Missile;
    public int EnermyCount;
    public int KillCount;
    public JoystickControler joystickcontroler;

    //게임종료시 필요한 자료들
    public Text Name;
    public Text GameOverText;
    public Button Restart;
    public Button MainMenu;

    //보스,몬스터의 리스폰 및 죽음 처리자료
    public int BossDieCount;
    public int RemainMonsterCount;
    public bool BossAppear;

    ////리스폰 관련 자료
    //public Transform PlayerTransform;
    //public Transform[] points;
    int CreateTime = 3;

    //광역기술사용할때 위치지정해주는것
    public GameObject skillarea;

    //게임유아이내부 자료를 사용하고있을시 npc와의 상호작요을 막기위해 만들어짐
    public bool UiUsing;

    public string PlayerName = "";

    static GameEingine instance;
    public static GameEingine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameEingine>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<GameEingine>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        UiUsing = false;
        KillCount = 0;
        //PlayerTransform = RespownControl.Instance.PlayerRespown;
        //for (int i = 0; i < 6; i++)
        //{
        //    points[i] = RespownControl.Instance.EnermyRespown[i].EnermyRespown;
        //}
        //기본필수코드들
        //플레이어 생성위치 세팅
        EnermyCount = 0;
        if(TitleEngine.Instance.PastPortal == PortalArea.PORTAL_VIALGE)
        {
            GameObject obj = Instantiate(PlayerPrefab, RespownControl.Instance.PlayerRespown[0].position, transform.rotation) as GameObject;
            PlayerControler = obj.GetComponent<PlayerControl>();
        }
        else if(TitleEngine.Instance.PastPortal == PortalArea.PORTAL_FIELDONE)
        {
            GameObject obj = Instantiate(PlayerPrefab, RespownControl.Instance.PlayerRespown[1].position, transform.rotation) as GameObject;
            PlayerControler = obj.GetComponent<PlayerControl>();
        }
        else if(TitleEngine.Instance.PastPortal == PortalArea.PORTAL_FIELDTWO)
        {
            GameObject obj = Instantiate(PlayerPrefab, RespownControl.Instance.PlayerRespown[2].position, transform.rotation) as GameObject;
            PlayerControler = obj.GetComponent<PlayerControl>();
        }
        GameUiEingine.Instance.setting(PlayerControler);
        PlayerInformation.Instance.Hp = TitleEngine.Instance.CharacterHp;
        Camera.main.GetComponent<FollowCamMyVer>().PlayerTr = Player.Instance;
        BossDieCount = 0;
        BossAppear = false;
        RemainMonsterCount = 20;
        if(TitleEngine.Instance.Battle == true)
        {
            StartCoroutine(CreateMonster());
        }
    }

    IEnumerator CreateMonster()
    {
        while(!GameOver)
        {
            var respon = RespownControl.Instance.EnermyRespown;
            if (RemainMonsterCount > 0 && EnermyCount < 5)
            {
                int idx = Random.Range(0, respon.Count);
                if(respon[idx].RespownPointUsingCheck == false)
                {
                    yield return new WaitForSeconds(CreateTime);
                    RemainMonsterCount--;
                    GameObject obj = Instantiate(Enermies[1].gameObject, respon[idx].EnermyRespown.position, respon[idx].EnermyRespown.rotation) as GameObject;
                    GameDataEngine.Instance.LoadEnermyData(obj);
                    if (TitleEngine.Instance.HpView == false)
                    {
                        obj.GetComponent<EnermyControl>().HpbarPos.SetActive(false);
                    }
                    EnermyCount++;
                    GameUiEingine.Instance.RemainMonsterCount.text = RemainMonsterCount.ToString();
                    GameUiEingine.Instance.RealMonsterCount.text = EnermyCount.ToString();
                    respon[idx].RespownPointUsingCheck = true;
                    obj.GetComponent<EnermyControl>().respownpoint = respon[idx];
                }
            }
            else if(RemainMonsterCount <= 0 && BossAppear == false && EnermyCount <= 0)
            {
                yield return new WaitForSeconds(CreateTime);
                Bosscontrol.Instance.BossAppear();
                BossAppear = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    void FixedUpdate()
    {
        //플레이어가 죽었때
        if(GameOver == true)
        {
            GameOverText.gameObject.SetActive(true);
            Restart.gameObject.SetActive(true);
            Name.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(true);
        }
        //적보스가 죽었을때
        else if(Bosscontrol.Instance.die == true)
        {
            NextLevel();
        }
        else
        {
            GameUiEingine.Instance.KillCountText.text = KillCount.ToString();
        }
        if(PlayerInformation.Instance.Hp <= 0)
        {
            GameOver = true;
            PlayerControl.Instance.Ani.SetBool("Die", true);
        }
    }
    
    public void GameRestart()
    {
        SubInterfaceControler.Instance.ResetAllSubInterface();
        KillCount = 0;
        SaveLoad.Instance.Save();
        TitleEngine.Instance.StartVilageMap();
    }

    public void NextLevel()
    {
        BossDieCount++;
        if(BossDieCount >= 100)
        {
            SceneManager.LoadScene("Level2");
        }
    }

    public void GoMainScence()
    {
        SubInterfaceControler.Instance.ResetAllSubInterface();
        KillCount = 0;
        SaveLoad.Instance.Save();
        Destroy(TitleEngine.Instance.gameObject);
        PlayerName = Name.text;
        //SaveLoad.Instance.SaveData(PlayerName, KillCount);
        SceneManager.LoadScene("Title");
    }
}