using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{
    public InputField inputfield;

    public void test(Text text)
    {
        text.text = inputfield.text;
    }
}
