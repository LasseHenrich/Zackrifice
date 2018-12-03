using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mess : MonoBehaviour {

    public static Mess instance;

    public GameObject playerPref;
    public Transform wallLeft, wallRight, wallTop, wallBottom;
    public AltarMenu altarMenu;
    public Animator mainMenu;
    public Text mainMenuHead;
    public Text mainMenuPlay;
    public Tilemap altarWall;
    public Tilemap altarFront;
    public Tile[] destAltTiles; // Tiles from destroyed Altar
    public Tile[] newAltTiles; // Tiles from Altar
    public GameObject tutorial;
    public Text mainWaveText;

    public Vector2 worldDim;

    public float altarHealth;
    public bool isPlaying;

	// Use this for initialization
	void Start () {
        instance = this;

        worldDim = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float camScale = 14;
        float camTotal = worldDim.x + worldDim.y;
        Vector2 camRelation = new Vector2(worldDim.x / camTotal, worldDim.y / camTotal);
        Camera.main.orthographicSize = camRelation.y * camScale;

        worldDim = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        wallLeft.position = new Vector2(-worldDim.x, 0);
        wallRight.position = new Vector2(worldDim.x, 0);
        wallLeft.localScale = wallRight.localScale = new Vector2(1, worldDim.y * 2);
        wallTop.position = new Vector2(0, worldDim.y);
        wallBottom.position = new Vector2(0, -worldDim.y);
        wallTop.localScale = wallBottom.localScale = new Vector2(1, worldDim.x * 2); // y-Coord, weil um 90° gedreht

        isPlaying = false;
    }

    public void StartGame()
    {
        if (!isPlaying)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            GameObject[] es = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < es.Length; i++)
                Destroy(es[i]);


            if (p == null)
            {
                tutorial.SetActive(true);
            }
            else
            {
                p.GetComponent<Player>().Start();
                p.GetComponent<Animator>().Rebind();
                p.GetComponent<Animator>().SetTrigger("Right");
                GetComponent<EnemyHandler>().Start();
                GetComponent<EnemyHandler>().healthBar.Start();
                GetComponent<EnemyHandler>().altarBar.Start();
                altarMenu.Start();

                altarWall.SetTile(new Vector3Int(-1, -1, 0), newAltTiles[0]);
                altarWall.SetTile(new Vector3Int(0, -1, 0), newAltTiles[1]);
                altarFront.SetTile(new Vector3Int(-1, 0, 0), newAltTiles[2]);
                altarFront.SetTile(new Vector3Int(0, 0, 0), newAltTiles[3]);

                isPlaying = true;
            }
            mainMenu.ResetTrigger("In");
            mainMenu.SetTrigger("Out");
        }
    }

    public void StartFirstTime()
    {
        float radius = 2;
        float angle = Random.Range(0, 360);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        Vector2 pos = direction * radius;
        Instantiate(playerPref, pos, Quaternion.identity);

        GetComponent<EnemyHandler>().enabled = true;
        GetComponent<EnemyHandler>().healthBar.enabled = true;
        GetComponent<EnemyHandler>().altarBar.enabled = true;
        altarMenu.enabled = true;

        isPlaying = true;
    }

    public void EndGame(bool througPlayer)
    {
        isPlaying = false;
        if(througPlayer)
            Player.instance.GetComponent<Animator>().SetTrigger("Die");
        //Debug.Log("Bye World");
        mainMenu.SetTrigger("In");
        mainMenuHead.text = "GAME OVER";
        mainMenuHead.color = Color.red;
        mainMenuPlay.text = "RETRY";
        mainWaveText.text = "WAVE: " + (EnemyHandler.instance.currWave + 1).ToString();
    }

    public void AltarDestroyed()
    {
        altarWall.SetTile(new Vector3Int(-1, -1, 0), destAltTiles[0]);
        altarWall.SetTile(new Vector3Int(0, -1, 0), destAltTiles[1]);
        altarFront.SetTile(new Vector3Int(-1, 0, 0), destAltTiles[2]);
        altarFront.SetTile(new Vector3Int(0, 0, 0), destAltTiles[3]);
    }

    public void PlayClick()
    {
        GetComponent<AudioSource>().Play();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
