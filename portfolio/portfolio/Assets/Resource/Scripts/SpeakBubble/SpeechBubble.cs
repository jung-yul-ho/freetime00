using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpechType
{
    SPEACH_NULL,
    SPEACH_HELLO,
    SPEACH_THANKS,
    SPEACH_BYE
}

public class SpeechBubble : MonoBehaviour
{
    //public Image BasicHpbarBack;
    //public Image BasicHpbarFront;
    //public EnermyControl enermy;
    SpechType speachtype;
    public Text NpcSpeach;
    Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Speach()
    {
        if(speachtype == SpechType.SPEACH_HELLO)
        {
            NpcSpeach.text = "안녕";
        }
        else if(speachtype == SpechType.SPEACH_NULL)
        {
            NpcSpeach.text = "없음";
        }
        else if (speachtype == SpechType.SPEACH_BYE)
        {
            NpcSpeach.text = "없음";
        }
        else if (speachtype == SpechType.SPEACH_THANKS)
        {
            NpcSpeach.text = "없음";
        }
    }

    private void Update()
    {

    }
}