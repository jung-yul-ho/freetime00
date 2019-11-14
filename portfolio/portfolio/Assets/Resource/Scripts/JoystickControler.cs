using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickControler : MonoBehaviour
{
    public Transform Stick;
    //조이스틱커서
    public Vector3 StickFierstpos;

    public Vector3 JoyVec;
    public float Radius;
    public bool moveflag;
    //플레이어 캐릭터의 방향전환을 위한 기초 백터값
    public Vector3 BasicRotation;

    //플레이어 무브 체크
    public bool checkmove;

    static JoystickControler instance;
    public static JoystickControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoystickControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<JoystickControler>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFierstpos = Stick.transform.position;

        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        moveflag = false;

        BasicRotation = new Vector3(0, 0, 0);
    }

    public void Update()
    {
        if (moveflag == true && GameEingine.Instance.GameOver == false && (PlayerControl.Instance.playerstate == PlayerState.PLAYER_IDLE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_ATTACK))
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_MOVE;
            //기초방향전환
            if (PlayerControl.Instance.TargetEnermy == null)
            {
                //플레이어의 방향 전환
                Vector3 aa = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
                Player.Instance.transform.eulerAngles = BasicRotation + aa;
            }
            //타게팅이 되어있을때 방향전환
            else
            {
                Vector3 aa = new Vector3(0, (Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg) + FollowCamMyVer.Instance.transform.eulerAngles.y, 0);
                Player.Instance.transform.eulerAngles = aa;
            }
            //플레이어가 전방으로 이동
            Player.Instance.transform.Translate(Vector3.forward * Time.deltaTime * 10f);
        }
    }

    public void JoysticSetting()
    {
        StickFierstpos = Stick.transform.position;

        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        moveflag = false;
        BasicRotation = Player.Instance.transform.eulerAngles;
    }

    public void OnDragForMain(BaseEventData Data)
    {
        GameEingine.Instance.UiUsing = true;
        OnDragForStick(Data);
    }

    public void OnEndDragForMain()
    {
        GameEingine.Instance.UiUsing = false;
        if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_SHOT)
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        }
        BasicRotation = Player.Instance.transform.eulerAngles;
        //무브커서가 원상복귀
        Stick.position = StickFierstpos;
        //이동체크를 false
        moveflag = false;
        FollowCamMyVer.Instance.ResetCameraPosition();
    }

    public void OnDragForStick(BaseEventData Data)
    {
        GameEingine.Instance.UiUsing = true;
        PointerEventData data = Data as PointerEventData;
        Vector3 pos = data.position;
        moveflag = true;
        //조이스틱을 이동시킬 방향을 구함 4등분중 하나
        JoyVec = (pos - StickFierstpos).normalized;
        //조이스틱의 정 중앙에서 현제 터지하고있는곳의 거리를 구한다.
        float dis = Vector3.Distance(pos, StickFierstpos);

        //조이스틱ui조작
        if (dis < Radius)
        {
            Stick.position = StickFierstpos + JoyVec * dis;
        }
        //반지름 초과시의 코드이지만 초과할 일이 거의 없다 
        else
        {
            Stick.position = StickFierstpos + JoyVec * Radius;
        }
    }

    public void OnEndDragForStick()
    {
        GameEingine.Instance.UiUsing = false;
        if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_SHOT)
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        }
        BasicRotation = Player.Instance.transform.eulerAngles;
        //무브커서가 원상복귀
        Stick.position = StickFierstpos;
        //이동체크를 false
        moveflag = false;
        FollowCamMyVer.Instance.ResetCameraPosition();
    }
}
