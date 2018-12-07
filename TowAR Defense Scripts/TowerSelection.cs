using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerSelection
{
    private static GameObject tower0, tower1, tower2, tower3;


    public static GameObject Tower0
    {
        get
        {
            return tower0;
        }
        set
        {
            tower0 = value;
        }
    }

    public static GameObject Tower1
    {
        get
        {
            return tower1;
        }
        set
        {
            tower1 = value;
        }
    }

    public static GameObject Tower2
    {
        get
        {
            return tower2;
        }
        set
        {
            tower2 = value;
        }
    }

    public static GameObject Tower3
    {
        get
        {
            return tower3;
        }
        set
        {
            tower3 = value;
        }
    }


    public static void SetTower(int cardIndex, GameObject tower)
    {

        if (cardIndex == 0)
        {
            Tower0 = tower;
        }
        else if (cardIndex == 1)
        {
            Tower1 = tower;
        }
        else if (cardIndex == 2)
        {
            Tower2 = tower;
        }
        else if (cardIndex == 3)
        {
            Tower3 = tower;
        }

    }
}