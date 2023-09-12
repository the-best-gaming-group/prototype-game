using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public enum CombatOptions
{
    SLAM = 2,
    FIRE_BALL = 3
}

public class BattleSystem : MonoBehaviour
{
    public float dialogWaitTime = 2f;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerTransform;
    public Transform enemyTransform;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleHUDD playerHUD;
    public BattleHUDD enemyHUD;

    public Text dialogText;

    public BattleState state;
    public bool isPlayerFirstTurn;

    bool playerDodged = false;
    List<CombatOptions> turnActions = new List<CombatOptions>();

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //add player to scene
        GameObject playerGO = Instantiate(playerPrefab, playerTransform);
        playerUnit = playerGO.GetComponent<Unit>();
        
        //add enemy to scene
        GameObject enemyGO = Instantiate(enemyPrefab, enemyTransform);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogText.text = "Fighting " + enemyUnit.unitName;

        //set HP info
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(dialogWaitTime);

        if (isPlayerFirstTurn)
            PlayerTurn();
        else
            EnemyTurn();
    }

    void PlayerTurn()
    {
        dialogText.text = "What you gonna do?";
        state = BattleState.PLAYER_TURN;
        //todo: set limit to what actions can be done based on energy and exp
    }

    /// <summary>
    /// combat buttons ability
    /// </summary>
    public void OnSlamButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            turnActions.Add(CombatOptions.SLAM);
        }
    }
    
    public void OnDodgeButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            playerDodged = !playerDodged;
            //for debugging
            if (playerDodged)
                dialogText.text += "dodging";
            else dialogText.text = dialogText.text.Replace("dodging", "");
        }
    }
    public void OnEndTurnButton()
    {
        if (state == BattleState.PLAYER_TURN)
        {
            StartCoroutine(ProcessTurn());
        }
    }

    IEnumerator ProcessTurn()
    {
        foreach (var action in turnActions)
        {
            int enemyNewHP = enemyUnit.TakeDamage(((int)action));
            enemyHUD.SetHP(enemyNewHP);
            dialogText.text = enemyUnit.unitName + " takes " + action.ToString();
            yield return new WaitForSeconds(dialogWaitTime);
            if (enemyNewHP == 0)
            {
                state = BattleState.WON;
                break;
            }
        }
        turnActions.Clear();

        if (enemyUnit.currentHP > 0)
        {
            state = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
        else
            EndBattle();
    }

    IEnumerator EnemyTurn()
    {
        dialogText.text = enemyUnit.unitName + " attacks!";

        //todo: if (playerDodged)
        const int enemySlamDamage = 5; //todo: add other attack abilities
        if (!playerDodged)
        {
            int playerNewHP = playerUnit.TakeDamage(enemySlamDamage);
            playerHUD.SetHP(playerNewHP);
            dialogText.text = "Alien slammed player";
        } else
        {
            dialogText.text = "Player dodged, Alien slam attack failed";
        }

        playerDodged = false;

        yield return new WaitForSeconds(dialogWaitTime);

        if (playerUnit.currentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.PLAYER_TURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogText.text = "You won against " + enemyUnit.unitName;
            //move on w/ quest
        } else
        {
            dialogText.text = "You lost against " + enemyUnit.unitName;
            //move back to checkpoint
        }
    }
}
