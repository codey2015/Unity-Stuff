using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicideButton : MonoBehaviour {
    public int hazardDamage = -100;
    public int staminaDamage = -50;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("k"))
        {
            TakeDamage();
        }
    }


    public void TakeDamage()
    {
        if (Invector.CharacterController.vThirdPersonController.instance != null)
        {
            print("Taking Damage");
            Invector.CharacterController.vThirdPersonController.instance.ChangeHealth(hazardDamage);
            Invector.CharacterController.vThirdPersonController.instance.ChangeStamina(staminaDamage);
        }
    }
}
