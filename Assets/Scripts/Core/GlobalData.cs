using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static Health playerHealth;
    public SceneChanger sceneChanger;
    public string FirstScene = "BenScene";
    int waitTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<Health>();
        playerHealth.maxHP = 100;
        while (playerHealth.currentHP < 100) {
            playerHealth.Increment();
        }
        Debug.Log("Player health is: " + playerHealth.currentHP.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0) {
            waitTime -= 1;
        }
        else {
            sceneChanger.ChangeScene("BenScene");
        }
    }
}
