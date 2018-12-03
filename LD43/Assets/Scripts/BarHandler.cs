using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour {

    Image img;
    Text text;
    float maxPoints;
    public float currPoints;

	// Use this for initialization
	public void Start () {
        img = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        currPoints = maxPoints = 100;
        img.fillAmount = currPoints;
        text.text = currPoints.ToString();
        if (gameObject.name == "HealthBar")
            Player.instance.health = currPoints;
        else
            Mess.instance.altarHealth = currPoints;

    }

    public void ChangeValue(float value)
    {
        currPoints += value;
        if (currPoints > maxPoints)
            maxPoints = currPoints;

        img.fillAmount = currPoints / maxPoints;
        text.text = currPoints.ToString();

        if (gameObject.name == "HealthBar")
            Player.instance.health = currPoints;
        else
            Mess.instance.altarHealth = currPoints;

        if (currPoints <= 0)
        {
            if (gameObject.name == "HealthBar")
                Mess.instance.EndGame(true);
            else
            {
                Mess.instance.AltarDestroyed();
                if (AltarMenu.instance.isOpen)
                    AltarMenu.instance.Close();
                Mess.instance.EndGame(false);
            }
        }
        //Debug.Log(currPoints / maxPoints);
    }
}
