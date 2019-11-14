using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamMyVer : MonoBehaviour
{
    public Player PlayerTr;
    public float rotateSpeed = 5;
    Vector3 offset;

    private Transform tr;
    //public float rotateDamping = 10.0f;    //회전 속도 계수
    public float distance;     //추적 대상과의 거리
    public float height;     //추적 대상과의 높이
    //public float targetOffset = 2.0f;     //추적 좌표의 오프셋

    //캐릭터가 바라보는 방향을 의미
    public Vector3 TargetDirection;

    //카메라의 각도(캐릭터가 이동할때에 변동)
    public float vectorX;

    public bool Testbool;

    void Start()
    {
        ResetCamera();
    }

    static FollowCamMyVer instance;
    public static FollowCamMyVer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FollowCamMyVer>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<FollowCamMyVer>();
                }
            }
            return instance;
        }
    }

    void LateUpdate()
    {
        if (GameEingine.Instance.GameOver == false)
        {
            //개선전
            if (TitleEngine.Instance.cameraversion == CameraVersion.CAMERA_VERSION_ONE)
            {
                //보스등장시 화면
                if (GameEingine.Instance.BossAppear == true)
                {
                    if (PlayerControl.Instance.TargetDirection <= 40)
                    {
                        Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                        transform.position = camPos;
                        var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                        NewRotation.x = 10;
                        NewRotation.z = 0;
                        //transform.rotation = Quaternion.Euler(NewRotation);
                        //카메라를 추적 대상으로 Z축을 회전시킴
                        tr.LookAt(Bosscontrol.Instance.charObj.transform);
                    }
                    else
                    {
                        Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                        transform.position = camPos;
                        var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                        NewRotation.x = 10;
                        NewRotation.z = 0;
                        transform.rotation = Quaternion.Euler(NewRotation);
                        //카메라를 추적 대상으로 Z축을 회전시킴
                        //tr.LookAt(target.transform.position);
                    }
                }
                //기본화면
                else if (PlayerControl.Instance.TargetEnermy == null)
                {
                    Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                    transform.position = camPos;
                    var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                    NewRotation.x = 10;
                    NewRotation.z = 0;
                    transform.rotation = Quaternion.Euler(NewRotation);
                    //카메라를 추적 대상으로 Z축을 회전시킴
                    //tr.LookAt(target.transform.position);
                }
                //적포착시화면
                else
                {
                    Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                    transform.position = camPos;
                    var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                    NewRotation.x = 10;
                    NewRotation.z = 0;
                    transform.rotation = Quaternion.Euler(NewRotation);
                    //카메라를 추적 대상으로 Z축을 회전시킴
                    //tr.LookAt(target.transform.position);
                }
            }
            //개선후
            else if (TitleEngine.Instance.cameraversion == CameraVersion.CAMERA_VERSION_TWO)
            {
                //보스등장시 화면
                if (GameEingine.Instance.BossAppear == true)
                {
                    if (PlayerControl.Instance.TargetDirection <= 40)
                    {
                        height = 2.0f;
                        distance = 10.0f;
                        var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                        NewRotation.x = 10;
                        NewRotation.z = 0;
                        transform.rotation = Quaternion.Euler(NewRotation);
                        Vector3 camPos = PlayerTr.transform.position /*- (TargetDirection * distance)*/ + (PlayerTr.transform.up * height);
                        transform.position = camPos;
                        tr.LookAt(Bosscontrol.Instance.charObj.transform.position);
                        tr.Translate(Vector3.back * 9);
                    }
                    else
                    {
                        Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                        transform.position = camPos;
                        var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                        NewRotation.x = vectorX;
                        NewRotation.z = 0;
                        transform.rotation = Quaternion.Euler(NewRotation);
                        MoveAndCameraControl();
                    }
                }
                //기본화면
                else if (PlayerControl.Instance.TargetEnermy == null)
                {
                    Vector3 camPos = PlayerTr.transform.position - (TargetDirection * distance) + (PlayerTr.transform.up * height);
                    //transform.position = camPos;
                    transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * 10);
                    var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                    NewRotation.x = vectorX;
                    NewRotation.z = 0;
                    transform.rotation = Quaternion.Euler(NewRotation);
                    MoveAndCameraControl();
                }
                //적포착시화면
                else
                {
                    height = 3.0f;
                    distance = 10.0f;
                    var NewRotation = Quaternion.LookRotation(PlayerTr.transform.position - transform.position).eulerAngles;
                    NewRotation.x = 10;
                    NewRotation.z = 0;
                    transform.rotation = Quaternion.Euler(NewRotation);
                    Vector3 VecBack = PlayerControl.Instance.TargetEnermy.transform.position - PlayerTr.transform.position;
                    Vector3 camPos = PlayerTr.transform.position - (VecBack.normalized * 6) + (PlayerTr.transform.up * height);
                    transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * 10);
                }
            }
        }
    }

    public void ResetCamera()
    {
        offset = PlayerTr.transform.position - transform.position;
        tr = GetComponent<Transform>();
        ResetCameraPosition();
        distance = 7.0f;
        height = 5.0f;
        vectorX = 10.0f;
    }

    public void ResetCameraPosition()
    {
        TargetDirection = PlayerTr.transform.forward;
    }

    //케릭터가 계속 이동시 카메라가 방향과 위치를 조정하는 코드 (실험중)
    public void MoveAndCameraControl()
    {
        float movecount = PlayerControl.Instance.MoveCount;
        if (movecount > 0.0f)
        {
            height = movecount + 5.0f;
            distance = 7.0f + movecount;
            vectorX = 10.0f + (movecount * 5);
        }
        else
        {
            height = 5.0f;
            distance = 7.0f;
            vectorX = 10.0f;
        }
    }
}
