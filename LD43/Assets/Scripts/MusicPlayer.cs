using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour {

    public Sprite[] sprts;

    public AudioSource source;
    float volume;

    bool isOn;

    private void Start()
    {
        volume = source.volume;
        isOn = true;
    }

    public void Change()
    {
        if (isOn)
        {
            source.volume = 0;
            GetComponent<Image>().sprite = sprts[1];
        }
        else
        {
            source.volume = volume;
            GetComponent<Image>().sprite = sprts[0];
        }
        isOn = !isOn;
    }
}
