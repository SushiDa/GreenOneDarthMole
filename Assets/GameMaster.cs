using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public int Score = 0;
    public float MaxEnergy;
    public float CurrentEnergy;
    public float EnergyIncrementPerSecond;
    public float EnergyPctDecrementPerSecond;
    public bool GameOver;

    public float BaseOverflowTimer;
    public float OverflowTimerDecrease;
    public float MinOverflowTimer;
    public int EnergyBaseScore;
    public int EnergyBaseIncrement;
    public int OverflowComboMultiplier;
    private float currentOverflowTimer;
    private int overflowCombo;

    public static int PreviousScore;

    public Slider EnergySlider;
    public Text ScoreText;

	// Use this for initialization
	void Start () {
        CurrentEnergy = MaxEnergy;
        Score = 0;
        GameOver = false;
        overflowCombo = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(currentOverflowTimer > 0 )
        {
            currentOverflowTimer -= Time.deltaTime;
            if(currentOverflowTimer <= 0)
            {
                overflowCombo = 0;
            }
        }

        float decrease = MaxEnergy * EnergyPctDecrementPerSecond * Time.deltaTime / 100;
        CurrentEnergy -= decrease;
        MaxEnergy += EnergyIncrementPerSecond * Time.deltaTime;
        UpdateUI();
        if(CurrentEnergy <= 0)
        {
            TriggerGameOver();
        }
    }

    public void DoScore()
    {
        Score += EnergyBaseScore;
        CurrentEnergy += EnergyBaseIncrement;

        if(CurrentEnergy > MaxEnergy)
        {
            //Overflow
            CurrentEnergy = MaxEnergy;
            currentOverflowTimer = Mathf.Max(MinOverflowTimer, BaseOverflowTimer - overflowCombo * OverflowTimerDecrease);
            overflowCombo++;
            Score += OverflowComboMultiplier * overflowCombo;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        ScoreText.text = ("" + Score).PadLeft(7, '0');
        EnergySlider.value = CurrentEnergy / MaxEnergy * 100;
    }

    void TriggerGameOver()
    {
        GameOver = true;
        PreviousScore = Score;
    }
    
}
