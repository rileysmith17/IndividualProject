using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    private bool doublePoints;
    private bool safeMode;
    private bool powerupActive;
    private float powerupDurationCounter;
    private ScoreManager theScoreManager;
    private PlatformGenerator thePlatformGenerator;
    private float normalPointsPerSecond;
    private float spikeRate;
    private PlatformDestroyer[] spikeList;
    private GameManager theGameManager;

    // Start is called before the first frame update
    void Start()
    {
        theScoreManager = FindObjectOfType<ScoreManager>();
        thePlatformGenerator = FindObjectOfType<PlatformGenerator>();
        theGameManager = FindObjectOfType <GameManager> ();
    }

    // Update is called once per frame
    void Update()
    {
        if(powerupActive)
        {
            powerupDurationCounter -= Time.deltaTime;

            if(theGameManager.powerupReset)
            {
                powerupDurationCounter = 0;
                theGameManager.powerupReset = false;
            }

            if (doublePoints) 
            {
                theScoreManager.pointsPerSecond = normalPointsPerSecond * 3;
                theScoreManager.shouldDouble = true;
            }

            if (safeMode)
            {
                thePlatformGenerator.randomSpikeThreshold = 0;
            }


            if(powerupDurationCounter <= 0)
            {
                theScoreManager.pointsPerSecond = normalPointsPerSecond;
                theScoreManager.shouldDouble = false;
                thePlatformGenerator.randomSpikeThreshold = spikeRate;
                powerupActive = false;
            }
        }
    }

    public void ActivatePowerup(bool points, bool safe, float time)
    {
        doublePoints = points;
        safeMode = safe;
        powerupDurationCounter = time;

        normalPointsPerSecond = theScoreManager.pointsPerSecond;
        spikeRate = thePlatformGenerator.randomSpikeThreshold;

        if(safeMode)
        {
            spikeList = FindObjectsOfType<PlatformDestroyer>();

            for (int i = 0; i < spikeList.Length; i++)
            {
                if (spikeList[i].gameObject.name.Contains("Spikes"))
                {
                    spikeList[i].gameObject.SetActive(false);
                }
            }
        }


        powerupActive = true;
    }

}
