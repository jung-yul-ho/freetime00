using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CameraVersion
{
    CAMERA_VERSION_ONE,
    CAMERA_VERSION_TWO
}

public class TitleEngine : MonoBehaviour
{
    public CameraVersion cameraversion;
    public GameObject ranking;
    public GameObject Option;
    //HP볼것인지 안볼것인지 결정
    public Image HpImage;
    public Text HpText;

    //카메라버전 체인지
    public Text CameraText;

    //랭킹창을 보조하기 위해서 만듬 일단 놔두기
    public List<Text>  NameText;
    public List<Text> PointText;

    //게임상에서 hp를 표시할지 말것인지 결정
    public bool HpView = true;

    //참거짓을 나타내는 이미지
    public Sprite True;

    //타이틀 기초 인터페이스
    public GameObject TitleMenu;
    public Animator TitleMenuAni;
    public GameObject start;
    public GameObject OptionButton;
    public GameObject Exit;

    //들어갈 맵이 전투맵인지 아닌지 판단
    public bool Battle;

    //지금이 어느 맵인지 확인
    public PortalArea ThisArea;

    //어느 맵에서 이동해왔는지를 체크
    public PortalArea PastPortal;

    //내 캐릭터의 hp가 얼마인지 나타냄
    public int CharacterHp;

    static TitleEngine instance;
    public static TitleEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TitleEngine>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<TitleEngine>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        CharacterHp = 10;
        Screen.SetResolution(1920, 1080, true);
        DontDestroyOnLoad(gameObject);
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if(scene.name == "Viliage")
        {
            //SaveLoad.Instance.Load();
        }
    }

    public void StartBattleMap(int MapNum)
    {
        Battle = true;
        SceneManager.LoadScene("UI_SCENE");
        if(MapNum == 1)
        {
            SceneManager.LoadScene("Level 01", LoadSceneMode.Additive);
            ThisArea = PortalArea.PORTAL_FIELDONE;
        }
        else if(MapNum == 2)
        {
            SceneManager.LoadScene("Level 02", LoadSceneMode.Additive);
            ThisArea = PortalArea.PORTAL_FIELDTWO;
        }
    }

    public void ViewOption()
    {
        Option.SetActive(true);
    }

    public void CloseOption()
    {
        Option.SetActive(false);
    }

    public void ViewRanking()
    {
        ranking.SetActive(true);
        for(int i = 0; i<10; i++)
        {
            NameText[i].text = PlayerPrefs.GetString("RankString" + i).ToString();
            PointText[i].text = PlayerPrefs.GetInt("RankInt" + i).ToString();
        }
    }

    public void CloseRanking()
    {
        ranking.SetActive(false);
    }

    public void HpviewChange()
    {
        if(HpView == true)
        {
            HpView = false;
            HpImage.sprite = null;
            HpText.text = "hp표시 안함";
        }
        else
        {
            HpView = true;
            HpImage.sprite = True;
            HpText.text = "hp표시중";
        }
    }

    public void CameraverChange()
    {
        if(cameraversion == CameraVersion.CAMERA_VERSION_TWO)
        {
            cameraversion = CameraVersion.CAMERA_VERSION_ONE;
            CameraText.text = "이거기초버전 딴거해";
        }
        else
        {
            cameraversion = CameraVersion.CAMERA_VERSION_TWO;
            CameraText.text = "이거해";
        }
    }


    public void ActiveButtons()
    {
        TitleMenu.active = false;
        start.active = true;
        OptionButton.active = true;
        Exit.active = true;
    }

    public void quit()
    {
        SaveLoad.Instance.Save();
        Application.Quit();
    }
}