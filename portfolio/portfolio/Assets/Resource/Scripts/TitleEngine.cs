using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleEngine : MonoBehaviour
{
    public GameObject ranking;
    public GameObject Option;

    //카메라버전 체인지
    public Text CameraText;

    //랭킹창을 보조하기 위해서 만듬 일단 놔두기
    public List<Text>  NameText;
    public List<Text> PointText;

    //타이틀 기초 인터페이스
    public GameObject TitleMenu;
    public Animator TitleMenuAni;
    public GameObject start;
    public GameObject OptionButton;
    public GameObject Exit;

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
        Screen.SetResolution(1920, 1080, true);
        DontDestroyOnLoad(PlayerInformation.Instance.gameObject);
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

    public void CameraverChange()
    {
        if(PlayerInformation.Instance.cameraversion == CameraVersion.CAMERA_VERSION_TWO)
        {
            PlayerInformation.Instance.cameraversion = CameraVersion.CAMERA_VERSION_ONE;
            CameraText.text = "이거기초버전 딴거해";
        }
        else
        {
            PlayerInformation.Instance.cameraversion = CameraVersion.CAMERA_VERSION_TWO;
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