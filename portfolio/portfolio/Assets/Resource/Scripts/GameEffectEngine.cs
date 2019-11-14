using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectEngine : MonoBehaviour
{
    public List<Effect> Skill;

    static GameEffectEngine instance;
    public static GameEffectEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameEffectEngine>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<GameEffectEngine>();
                }
            }
            return instance;
        }
    }

    //public void BurningStrom(Transform tr)
    //{
    //    Effect obj = Instantiate<Effect>(Skill[0]);
    //    obj.transform.position = tr.position;
    //}
}
