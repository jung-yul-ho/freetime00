using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public enum NpcType
{
    NPC_TRADER,
    NPC_QUSET
}

public class NpcControl : MonoBehaviour/*, IPointerClickHandler*/
{
    NpcType npctype; 

    public Transform PlayerTr;
    public Transform NpcTr;
    public NavMeshAgent nvAgent;
    //말풍선을 넣을때 머리위에 말풍선을 놓기 위해 만듬
    public GameObject Head;

    //몇초동안 다이얼로그 상자를 놔둘것인가?
    public float bubbleTimeToLive = 3f;

    public float direction;

    [System.Serializable]
    public class DialogueLine
    {
        public float delay = 2;
        public string line;
        public SpeechControler.SpeechbubbleType speechBubbleType = SpeechControler.SpeechbubbleType.NORMAL;
    }

    public DialogueLine[] script;

    private void Start()
    {
        PlayerTr = Player.Instance.transform;
        NpcTr = gameObject.GetComponent<Transform>();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(CheckNpcState());


    }

    public void Update()
    {
        if(Input.GetMouseButton(0) && direction <= 10.0f && GameEingine.Instance.UiUsing == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                market.Instance.MarketOpen();
            }
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log("!!!!");
    //}

    //실패작
    //지우기로 결정했을때 npc고블린의 이벤트 트리거도 같이 지우기
    public void ClickNpc(BaseEventData Data)
    {
        PointerEventData data = Data as PointerEventData;
        Vector3 pos = data.position;

        market.Instance.MarketOpen();
    }
    

    IEnumerator CheckNpcState()
    {
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            direction = Vector3.Distance(PlayerTr.position, NpcTr.position);
            if(direction <= 10.0f)
            {
                SpeechControler.Instance.AddSpeechBubble(Head.transform, script[index].line, script[index].speechBubbleType, bubbleTimeToLive, Color.white, Vector3.zero);
            }
        }
    }
}
