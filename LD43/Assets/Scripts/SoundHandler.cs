using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour {

    public Sprite[] sprts;
    public AudioSource musicSource;
    public Image img;

    AudioSource[] sources;

    bool isOn;

    // Use this for initialization
    void Start () {
        sources = FindObjectsOfType<AudioSource>();
        isOn = true;
    }

    private void Update()
    {
        if (!isOn)
        {
            sources = FindObjectsOfType<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != musicSource)
                    sources[i].volume = 0;
            }
        }
    }

    public void Change()
    {
        if (isOn)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != musicSource)
                    sources[i].volume = 0;
            }

            img.sprite = sprts[1];
        }
        else
        {
            for (int i = 0; i < sources.Length; i++)
            {
                if(sources[i] != musicSource)
                    sources[i].volume = 1;
            }

            img.sprite = sprts[0];
        }
        isOn = !isOn;
    }
}
