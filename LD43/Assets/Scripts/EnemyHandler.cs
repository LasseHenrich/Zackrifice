using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour {

    public static EnemyHandler instance;

    public GameObject[] zombies;
    public BarHandler healthBar;
    public BarHandler altarBar;

    public Text waveText;

    public int currWave;
    bool inSpawning;
    float betweenTime;
    float currBetTime;
    int maxEnemys;
    int currEnemy;
    int deadEnemys;
    private float maxPauseTime;
    private float pauseTime;

    int additioner;
    Vector2 midScreen;
    private float angleTL, angleDL, angleTR, angleDR;

    // Use this for initialization
    public void Start () {

        instance = this;

        midScreen = Mess.instance.worldDim;

        angleTL = Mathf.Atan2(midScreen.y, -midScreen.x) * Mathf.Rad2Deg;
        angleDL -= angleTL;
        angleTR = Mathf.Atan2(midScreen.y, midScreen.x) * Mathf.Rad2Deg;
        angleDR -= angleTR;

        additioner = 10;
        currWave = -1;

        maxPauseTime = 3;

        NewWave();
	}
	
	// Update is called once per frame
	void Update () {
        if (inSpawning)
        {
            currBetTime += Time.deltaTime;
            if (currBetTime >= betweenTime)
                SpawnEnemy();
        }
        else if (pauseTime > 0)
        {
            pauseTime -= Time.deltaTime;
            int timeInt = (int)pauseTime;
            if (pauseTime != timeInt)
                timeInt++;
            waveText.text = "NEXT WAVE IN " + timeInt;
            if (pauseTime <= 0)
                NewWave();
        }
	}

    void NewWave()
    {
        inSpawning = true;
        currWave++;
        currEnemy = 0;
        deadEnemys = 0;

        waveText.text = "WAVE " + (currWave + 1);

        int multiplier = currWave + additioner;
        float variation = 0.15f * multiplier;

        maxEnemys = (int)Random.Range((0.35f * multiplier) + Random.Range(-variation, variation), (0.45f * multiplier) + Random.Range(-variation, variation));
        //maxEnemys = 1;
        betweenTime = NewBetweenTime();

        //Debug.Log(maxEnemys);
        SpawnEnemy();
    }

    float NewBetweenTime()
    {
        int multiplier = currWave + additioner;
        float variation = 0.05f * multiplier;
        float[] betweenTimeRange = new float[] { (35 / multiplier) + Random.Range(-variation, variation), (40 / multiplier) + Random.Range(-variation, variation) };
        return betweenTime = Random.Range(betweenTimeRange[0], betweenTimeRange[1]);
    }

    void SpawnEnemy()
    {
        currEnemy++;
        currBetTime = 0;

        int multiplier = (currWave / 2);
        float variation = 0.2f * multiplier;
        int type = (int)Random.Range((0.1f * multiplier) + Random.Range(-variation, variation), (0.3f * multiplier) + Random.Range(-variation, variation));
        if (type < 0)
            type = 0;
        if (type >= zombies.Length)
            type = zombies.Length - 1;
        GameObject e = Instantiate(zombies[type], EdgePosition(0, 0, -1), Quaternion.identity);
        e.GetComponent<Enemy>().healthBar = healthBar;
        e.GetComponent<Enemy>().altarBar = altarBar;

        if (currEnemy < maxEnemys)
            betweenTime = NewBetweenTime();
        else
            inSpawning = false;
    }

    public void EnemyDied()
    {
        deadEnemys++;
        if (deadEnemys >= maxEnemys)
        {
            if (altarBar.currPoints > 0)
            {
                pauseTime = maxPauseTime;
                healthBar.ChangeValue(15 + currWave);
            }
            else
                NewWave();
        }
    }

    public Vector2 EdgePosition(float offsetX, float offsetY, float angle) // angle -1 = random
    {
        if (angle == -1)
            angle = Random.Range(1, 360);
        //Debug.Log(angle);
        Vector2 vec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        //Debug.Log(vec);

        float factor = 0;

        float newDR = 360 - angleTR;
        float newDL = 360 - angleTL;

        if ((angle < angleTL && angle > angleTR) || (angle < newDR && angle > newDL))
        {
            //Debug.Log("y");
            factor = Mathf.Abs((midScreen.y + offsetY) / vec.y); /// 5 = units zum oberen bzw. unteren Bildschirmrand
            //Debug.Log("9");
            // Anmerkung: midScreen = mid2Ecke
        }
        else
        {
            //Debug.Log("x");
            factor = Mathf.Abs((midScreen.x + offsetX) / vec.x); /// 9 = units zum linken bzw. rechten Bildschirmrand
            //Debug.Log("10");
        }

        return new Vector2(vec.x * factor, vec.y * factor);
    }
}
