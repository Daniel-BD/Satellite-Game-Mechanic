using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScript : MonoBehaviour
{

    public GameObject[] powerBars;
    public GameObject[] jumpTokens;
    public GameObject victoryText;
    public GameObject victoryTextBackground;

    private int currentLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        resetPowerLevel();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void increasePowerLevel()
    {
        powerBars[currentLevel].SetActive(true);

        if (currentLevel < powerBars.Length - 1)
        {
            currentLevel++;
        }
        
    }

    public void resetPowerLevel()
    {
        currentLevel = 0;
        foreach (GameObject powerBar in powerBars)
        {
            powerBar.SetActive(false);
        }
    }

    public void decreaseJumpTokens(int index)
    {
        Debug.Log("decreaseJumpTokens index: " + index.ToString());
        jumpTokens[index].SetActive(false);
    }

    public void resetJumpTokens()
    {

        foreach (GameObject jumpToken in jumpTokens)
        {
            jumpToken.SetActive(true);
        }

    }

    public void activateVictory()
    {
        victoryText.SetActive(true);
        victoryTextBackground.SetActive(true);
    }

    public void restartedFromVictory()
    {
        victoryText.SetActive(false);
        victoryTextBackground.SetActive(false);
}

}
