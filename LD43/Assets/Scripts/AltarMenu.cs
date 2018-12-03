using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltarMenu : MonoBehaviour {

    public static AltarMenu instance;

    #region OnScreen

    public Text[] wepBTexts;
    public GameObject[] bloods;

    #endregion

    Animator anim;
    public bool isOpen;

    int[] cost;
    bool[] bought;
    float[] maxReloadingTime;

    // Use this for initialization
    public void Start () {
        instance = this;
        anim = GetComponent<Animator>();
        isOpen = false;

        cost = new int[] { 0, 120, 150, 200, 250, 10 };
        bought = new bool[] { true, false, false, false, false};
        maxReloadingTime = new float[] { 1, 1.5f, 0.2f, 1.2f, 0.5f };

        for (int i = 1; i < wepBTexts.Length; i++)
        {
            wepBTexts[i].text = cost[i].ToString();
        }
        wepBTexts[0].text = "EQUIPPED";
        wepBTexts[0].fontSize = 8;

        Player.instance.SetWeapon(0, maxReloadingTime[0]);
    }

    public void Open()
    {
        if (Mess.instance.altarHealth > 0) {
            if (!isOpen)
            {
                anim.SetTrigger("Open");
                isOpen = true;
            }
        }
    }

    public void Close()
    {
        Debug.Log("close");
        if (isOpen)
        {
            anim.SetTrigger("Close");
            isOpen = false;
        }
    }

    public void ButtonPressed(int wep)
    {
        if (wep <= 4)
        {
            if (!bought[wep])
            {
                if (Player.instance.health > cost[wep])
                {
                    bought[wep] = true;
                    bloods[wep].SetActive(false);
                    ChangeHealth(cost[wep]);
                    EquipWeapon(wep);
                }
            }
            else
            {
                EquipWeapon(wep);
            }
        }
        else
        {
            if (Player.instance.health > cost[wep])
            {
                ChangeHealth(cost[wep]);
                Mess.instance.altarHealth += cost[wep]; // Muss nicht cost[wep] sein! Beliebiger Wert möglich
                EnemyHandler.instance.altarBar.ChangeValue(cost[wep]);
            }
        }
    }

    void EquipWeapon(int wep)
    {
        wepBTexts[Player.instance.currWeapon].text = "EQUIP";
        wepBTexts[Player.instance.currWeapon].fontSize = 10;
        Player.instance.SetWeapon(wep, maxReloadingTime[wep]);
        wepBTexts[wep].text = "EQUIPPED";
        wepBTexts[wep].fontSize = 8;
    }

    void ChangeHealth(int value)
    {
        Player.instance.health -= value;
        EnemyHandler.instance.healthBar.ChangeValue(-value);
    }
}
