using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMainControl : MonoBehaviour
{
    public void StartSelect()
    {
        TitleEngine.Instance.ActiveButtons();
    }
}