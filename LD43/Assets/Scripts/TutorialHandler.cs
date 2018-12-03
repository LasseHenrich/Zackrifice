using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{
    public Text text;
    public Text nextText;

    bool write;
    string newText;
    int chara;
    float charTime;
    float maxCharTime;
    int currIndex;

    // Use this for initialization
    void Start()
    {
        write = false;
        newText = "";
        chara = 0;
        charTime = 0;
        maxCharTime = 0.03f;
        currIndex = 0;

        SetTutText();
    }

    private void Update()
    {
        if (write)
        {
            charTime += Time.deltaTime;
            if (charTime > maxCharTime)
            {
                charTime = 0;
                text.text += newText[chara].ToString();
                chara++;
                if (chara >= newText.Length)
                    write = false;
            }
        }
    }

    public void Skip()
    {
        currIndex = 8;
        Next();
    }

    public void Next()
    {
        currIndex++;
        if (currIndex <= 8)
            SetTutText();
        else if(!Mess.instance.isPlaying)
        {
            GetComponent<Animator>().SetTrigger("Out");
            Mess.instance.StartFirstTime();
        }
    }

    void SetTutText()
    {
        switch (currIndex)
        {
            case 0: ReTut("HEY!", "Yes?"); break;
            case 1: ReTut("ZACK, SOME ZOMBIES ARE ATTACKING MY ALTAR!", "OKAY"); break;
            case 2: ReTut("NOT OKAY, THEY DESTROY IT! PLEASE PROTECT IT!", "WHY?"); break;
            case 3: ReTut("OK FINE. IF YOU DO SO, YOU WILL GET SOME HEALTH POINTS AFTER EACH WAVE", "HM..."); break;
            case 4: ReTut("YOU CAN EVEN GET SOME NEW WEAPONS OUT OF THE ALTAR.", "HOW?"); break;
            case 5: ReTut("Just stand next to it and press Space", "Ok"); break;
            case 6: ReTut("BUT THAT WILL COST YOU SOME OF YOUR HEALTH POINTS.", "HM..."); break;
            case 7: ReTut("COME ON ZACK, SACRIFICES MUST BE MADE!", "DEAL"); break;
            case 8: ReTut("THANK YOU ZACK! I WILL KILL YOU IF THEY DESTROY IT!", "START"); break;
        }
        text.text = "";
        chara = 0;
        write = true;
    }

    void ReTut(string text, string next)
    {
        newText = text;
        nextText.text = next;
    }

    public void DisableMe()
    {
        gameObject.SetActive(false);
    }
}