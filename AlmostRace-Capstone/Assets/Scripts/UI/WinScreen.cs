﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{

    DataManager players;
    VehicleInput playerNum;
    VehicleHypeBehavior hypeBonus;
    HypeManager playerHype;

    string[] awardList = {"Spray n' Pray", "Speed Demon", "Shields Up", "Slippery"};

    int[] awardWinners;
    public float smallHypeAward,mediumHypeAward,largeHypeAward;

    int winningPlayer;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectOfType<DataManager>();
        playerNum = FindObjectOfType<VehicleInput>();
        awardWinners = new int[awardList.Length];
        winningPlayer = 0;
        hypeBonus = GetComponent<VehicleHypeBehavior>();
        playerHype = GetComponent<HypeManager>();
    }

    public void chooseWinners()
    {
        

        for (int i = 0; i<awardList.Length; i++)
        {
            maxValueWinner(awardList[i], i);
        }

        
    }

    private void maxValueWinner(string awardName, int award)
    {
        switch (awardName)
        {
            case "Spray n' Pray":
                awardWinners[award] = offensiveAbilityMax();
                break;
            case "Speed Demon":
                awardWinners[award] = boostAbilityMax();
                break;
            case "Shields Up":
                awardWinners[award] = defensiveAbilityMax();
                break;
            case "Slippery":
                awardWinners[award] = driftAbilityMax();
                break;
        }
        
    }

    private int offensiveAbilityMax()
    {
        for (int i = 0; i<players.playerInfo.Length; i++)
        {
            if(players.playerInfo[i].offensiveAbilityUsed>players.playerInfo[winningPlayer].offensiveAbilityUsed)
            {
                winningPlayer = i;
            }
        }
        addHype(winningPlayer, smallHypeAward);

        return winningPlayer;
    }

    private int defensiveAbilityMax()
    {
        for (int i = 0; i < players.playerInfo.Length; i++)
        {
            if (players.playerInfo[i].defenseAbilityUsed > players.playerInfo[winningPlayer].defenseAbilityUsed)
            {
                winningPlayer = i;
            }
        }
        addHype(winningPlayer, smallHypeAward);
        return winningPlayer;
    }

    private int boostAbilityMax()
    {
        for (int i = 0; i < players.playerInfo.Length; i++)
        {
            if (players.playerInfo[i].boostAbilityUsed > players.playerInfo[winningPlayer].boostAbilityUsed)
            {
                winningPlayer = i;
            }
        }
        addHype(winningPlayer, smallHypeAward);
        return winningPlayer;
    }

    private int driftAbilityMax()
    {
        for (int i = 0; i < players.playerInfo.Length; i++)
        {
            if (players.playerInfo[i].driftTimer > players.playerInfo[winningPlayer].driftTimer)
            {
                winningPlayer = i;
            }
        }
        addHype(winningPlayer, smallHypeAward);
        return winningPlayer;
    }

    private void addHype(int playerNum, float amount)
    {
        VehicleInput input;

        //Get the correct cars VehicleHypeBehavior
        foreach (GameObject car in playerHype.vehicleList)
        {
            input = car.GetComponent<VehicleInput>();
            if(input != null)
            {
                if(input.playerNumber == playerNum)
                {
                    input.gameObject.GetComponent<VehicleHypeBehavior>().AddHype(amount, "Award");
                }
            }
        }

    }
}
