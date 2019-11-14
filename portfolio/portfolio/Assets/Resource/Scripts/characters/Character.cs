using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public void Damage(float Damage)
    {
        int def = PlayerInformation.Instance.Def;
        int shield = PlayerInformation.Instance.Shield;
        if (Damage > def)
        {
            if (shield > 0)
            {
                shield -= (int)(Damage - def);
            }
            else
            {
                PlayerInformation.Instance.Hp -= (int)(Damage - def);
            }
        }
        else
        {
            if (shield > 0)
            {
                shield -= 1;
            }
            else
            {
                PlayerInformation.Instance.Hp -= 1;
            }
        }
    }
}
