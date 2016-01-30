using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CinematicSet : Set 
{
    public enum Type
    {
        HoarderConv,
        GuardExclaim,
        EnemyExclaim
    }

    public enum Speaker
    {
        King,
        Guard
    }

    [System.Serializable]
    public struct Sentence
    {
        public string Words;
        public Speaker OwningTextBox;
    }

    [System.Serializable]
    public struct Conversation
    {
        public List<Sentence> Sentences;
        public float LetterDelay;
    }

    public Text GuardTextBox;
    public Text KingTextBox;

    private Conversation Conv;
    private Sentence CurSentence;
    private Text CurTextBox;
    private int SentenceIndex;
    private int LetterIndex;
    private float TimeToNextLetter;
    private float LetterDelay;
    private bool bSentenceComplete;

    public void BeginCinematic(Type CinematicType)
    {
        // Disable player input
        // Hide castle UI

        // TODO (zesty): Handle other enum types
        Conv = GameData.Cinematics[Type.HoarderConv];

        LetterDelay = Conv.LetterDelay;
        TimeToNextLetter = LetterDelay;

        SentenceIndex = 0;
        LetterIndex = 0;
        CurSentence = Conv.Sentences[SentenceIndex];

        CurTextBox = GetTextBox(CurSentence.OwningTextBox);
        SetTextBoxVisible(CurTextBox, true);
        bSentenceComplete = false;
    }

    public void EndCinematic()
    {
        SetManager.CloseSet(this);
    }

	// Use this for initialization
	void Start () 
    {
        print("Begin cine...");
	}
	
	// Update is called once per frame
	void Update () 
    {
        TimeToNextLetter -= Time.deltaTime;
	    if(TimeToNextLetter < 0 && bSentenceComplete == false)
        {
            TimeToNextLetter = LetterDelay;
            ++LetterIndex;

            if (LetterIndex >= CurSentence.Words.Length)
            {
                bSentenceComplete = true;
            }
            else
            {
                CurTextBox.text = CurSentence.Words.Substring(0, LetterIndex);
            }
        }

        if(bSentenceComplete == true)
        {
            CurTextBox.text = CurSentence.Words;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(bSentenceComplete == true)
            {
                ++SentenceIndex;
                if(SentenceIndex == Conv.Sentences.Count)
                {
                    print("(zesty) Cinematic complete...");
                    SetTextBoxVisible(CurTextBox, false);
                    EndCinematic();
                }
                else
                {
                    // Next sentence time!
                    SetTextBoxVisible(CurTextBox, false);

                    CurSentence = Conv.Sentences[SentenceIndex];
                    CurTextBox = GetTextBox(CurSentence.OwningTextBox);
                    SetTextBoxVisible(CurTextBox, true);
                    TimeToNextLetter = LetterDelay;
                    LetterIndex = 0;
                    bSentenceComplete = false;
                }
            }
            else
            {
                bSentenceComplete = true;
            }
        }
	}

    private void SetTextBoxVisible(Text Box, bool Visible)
    {
        Box.transform.parent.gameObject.SetActive(Visible);

        Box.text = "";
    }

    private Text GetTextBox(Speaker TheSpeaker)
    {
        switch(TheSpeaker)
        {
            case Speaker.Guard:
                return GuardTextBox;
                break;
            case Speaker.King:
                return KingTextBox;
                break;
        }

        print("(zesty): ERROR!  Data file has bad speaker enums set.");
        return null;
    }
}
