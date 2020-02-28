using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerManager player;
    public PlayerAnimation playerAnimation;
    public GameObject gameOverPanel;
    public Text rountFireCountText;

    public Text gameOverRountFireCountText;
    public Text roundDistanceText;

    public Text bestFireCountText;
    public Text bestDistanceText;

    private int bestShootCount=-1;
    private float bestDistance=0;
    private int fireCountRound;
    private float distanceCountRound;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ResetGame()
    {
        rountFireCountText.text = fireCountRound.ToString();
        gameOverPanel.SetActive(false);
        fireCountRound = 0;
        distanceCountRound = 0;
        player.PlayerReset();
    }

    public void GameOver()
    {
        if (bestShootCount < 0)
        {
            bestDistance = distanceCountRound;
            bestShootCount = fireCountRound;
        }
        else 
        {
            if (distanceCountRound > bestDistance ) 
            {
                bestDistance = distanceCountRound;
                bestShootCount = fireCountRound;
            }
        }

        gameOverPanel.SetActive(true);
        bestDistanceText.text = bestDistance.ToString("F2") + " mts";
        bestFireCountText.text = bestShootCount.ToString()+ " shoots";
        roundDistanceText.text = distanceCountRound.ToString("F2") + " mts";
        gameOverRountFireCountText.text = fireCountRound.ToString() + " shoots"; ;
    }

    public void ShootCount(int fireSuccessfulCount)
    {
        fireCountRound = fireSuccessfulCount;
        rountFireCountText.text = fireCountRound.ToString();
    }

    public void SetDistanceCount(float distance)
    {
        if (distance > distanceCountRound)
            distanceCountRound = distance;

    }
}
