using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TowerPreview : MonoBehaviour
{

    public GameObject minigun;
    public GameObject rifle;
    public GameObject sniper;
    public GameObject cannon;

    private GameObject currentTower;
    public TextMeshProUGUI currentText;

    public void SetTowerPreview(int value)
    {

        if (currentTower) // if (currentTower && currentText)
        {
            currentTower.SetActive(false);
            // set current text
        }

        switch (value)
        {

            case 0: //minigun
                minigun.SetActive(true);
                currentTower = minigun;
                currentText.text = "Miniguns are good against weak, fast enemies.";
                // current text = minigunText
                break;

            case 1: //rifle
                rifle.SetActive(true);
                currentTower = rifle;
                currentText.text = "Rifles are the bread-and-butter towers that are good against everything.";
                break;

            case 2: //sniper
                sniper.SetActive(true);
                currentTower = sniper;
                currentText.text = "Useful when facing tough enemies. Plug your ears.";
                break;

            case 3: //cannon
                cannon.SetActive(true);
                currentTower = cannon;
                currentText.text = "Powerful towers that deal splash damage. Great against groups.";
                break;

            default:
                break;
        }

    }
}