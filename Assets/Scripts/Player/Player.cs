using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            if (money != value)
            {
                money = value;
            }
        }
    }




}





