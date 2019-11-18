using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CameraVersion
{
    CAMERA_VERSION_ONE,
    CAMERA_VERSION_TWO
}

public class PlayerInformation : MonoBehaviour
{
    public CameraVersion cameraversion;
    //지금이 어느 맵인지 확인
    public PortalArea ThisArea;

    //어느 맵에서 이동해왔는지를 체크
    public PortalArea PastPortal;
    public int Hp;
    public int Shield;
    public int Atk;
    public int Def;
    public int Money;

    //들어갈 맵이 전투맵인지 아닌지 판단
    public bool Battle;

    static PlayerInformation instance;
    public static PlayerInformation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerInformation>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayerInformation>();
                }
            }
            return instance;
        }
    }

    public void StartVilageMap()
    {
        //StartCoroutine(LoadScene("Viliage"));
        Battle = false;
        SceneManager.LoadScene("UI_SCENE");
        SceneManager.LoadScene("Viliage", LoadSceneMode.Additive);
        ThisArea = PortalArea.PORTAL_VIALGE;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartBattleMap(int MapNum)
    {
        Battle = true;
        SceneManager.LoadScene("UI_SCENE");
        if (MapNum == 1)
        {
            SceneManager.LoadScene("Level 01", LoadSceneMode.Additive);
            ThisArea = PortalArea.PORTAL_FIELDONE;
        }
        else if (MapNum == 2)
        {
            SceneManager.LoadScene("Level 02", LoadSceneMode.Additive);
            ThisArea = PortalArea.PORTAL_FIELDTWO;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if (scene.name == "Viliage")
        {
            Debug.Log("!!!!");
            SaveLoad.Instance.Load();
        }
    }

    //플레이어의 기본능력치와 장비를 포함해서 능력치를 계산해주는 코드
    public void CheckPlayerStat()
    {
        Def = 0;
        Atk = 0;
        //2는 기본능력치
        if (Equipment.Instance.WeaponSlot.use == true)
        {
            Atk = 2 + Equipment.Instance.WeaponSlot.InitItem.atk;
        }
        else
        {
            Atk = 2;
        }
        if (Equipment.Instance.BodySlot.use == true)
        {
            Def += Equipment.Instance.BodySlot.InitItem.def;
        }
        if (Equipment.Instance.HeadSlot.use == true)
        {
            Def += Equipment.Instance.HeadSlot.InitItem.def;
        }
        Equipment.Instance.EquipViewReset();
    }

    public void SetMoney()
    {
        Inventory.Instance.Money.text = Money.ToString() + "G";
    }
}
