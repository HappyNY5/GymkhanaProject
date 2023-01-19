using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image multiplerImageFiller;
    [SerializeField] private TMP_Text multiplerText;

    private float multiplerDieTime = 5;



    private uint playerScore = 0;
    private uint playerMultiplerScore = 0;
    private uint scoreMultipler = 1;
    private float hightSpeedScore = 20;
    private float airTime = 0;
    private float dropMultiplerTime = 0;
    private float driftAngle = 0;

    void FixedUpdate()
    {
        if(dropMultiplerTime > 0)
        {
            dropMultiplerTime -= Time.fixedDeltaTime;
            multiplerImageFiller.fillAmount = dropMultiplerTime / multiplerDieTime;
        }else{
            playerScore += playerMultiplerScore * scoreMultipler;
            UpdateScoreText();

            playerMultiplerScore = 0;
            scoreMultipler = 1;
            MultiplerChecker();
        }
    }

    public void AddScore(uint score)
    {
        playerMultiplerScore += score * scoreMultipler;
        MultiplerChecker();
        UpdateMultiplerTimer();
    }


    private void MultiplerChecker()
    {
        if(playerMultiplerScore<500)
            scoreMultipler = 1;
        else if (playerMultiplerScore<1000)
            scoreMultipler = 2;
        else if (playerMultiplerScore<2000)
            scoreMultipler = 3;

        if(playerMultiplerScore == 0)
            multiplerText.text = "";
        else
            multiplerText.text = $"{scoreMultipler} X {playerMultiplerScore}";
    }

    public void UpdateMultiplerTimer()
    {
        dropMultiplerTime = multiplerDieTime;
    }


    private void UpdateScoreText()
    {
        scoreText.text = $"Score = {playerScore}";
    }
}
