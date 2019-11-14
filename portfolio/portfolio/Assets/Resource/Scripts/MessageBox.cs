using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text MyText;
    float fadingRate = 0.007f;
    public Color MyOrignalColor;

    static MessageBox instance;
    public static MessageBox Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageBox>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<MessageBox>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        MyText = GetComponent<Text>();
        MyOrignalColor = MyText.color;
        MyText.text = "";
    }

    public void ShowMessage(string MessageToShow)
    {
        MyText.color = MyOrignalColor;
        MyText.text = MessageToShow;
        StopAllCoroutines();
        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        while (MyText.color.a >= 0.1f)
        {
            yield return null;
            Color TextColor = MyText.color;
            TextColor.a -= fadingRate;
            if (TextColor.a <= 0.1f)
            {
                TextColor.a = 0;
            }
            MyText.color = TextColor;
        }
    }
}
