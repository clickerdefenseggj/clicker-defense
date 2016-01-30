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

    [System.Serializable]
    public struct Sentence
    {
        public string Words;
        public Text OwningTextBox;
    }

    [System.Serializable]
    public struct Conversation
    {
        public Sentence[] Sentences;
        public float LetterDelay;
        public bool ShowAll;
    }

    public Type type;
    public Conversation HoarderConv;

    private Conversation Conv;
    private Sentence CurSentence;
    private int SentenceIndex;
    private int LetterIndex;
    private float TimeToNextLetter;
    private float LetterDelay;
    private bool bWaitForInput;

    public void BeginCinematic()
    {
        // Disable player input
        // Hide castle UI

        // TODO (zesty): Handle other enum types
        Conv = HoarderConv;

        LetterDelay = Conv.LetterDelay;
        TimeToNextLetter = LetterDelay;

        SentenceIndex = 0;
        LetterIndex = 0;
        CurSentence = Conv.Sentences[SentenceIndex];

        SetTextBoxVisible(CurSentence.OwningTextBox, true);
        bWaitForInput = false;
    }

    public void EndCinematic()
    {

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
	    if(TimeToNextLetter < 0 && bWaitForInput == false)
        {
            TimeToNextLetter = LetterDelay;
            ++LetterIndex;

            if (LetterIndex >= CurSentence.Words.Length)
            {
                bWaitForInput = true;
            }
            else
            {
                CurSentence.OwningTextBox.text = CurSentence.Words.Substring(0, LetterIndex);
            }
        }

        if(bWaitForInput == true)
        {
            CurSentence.OwningTextBox.text = CurSentence.Words;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(bWaitForInput == true)
            {
                ++SentenceIndex;
                if(SentenceIndex == Conv.Sentences.Length)
                {
                    print("(zesty) Cinematic complete...");
                    SetTextBoxVisible(CurSentence.OwningTextBox, false);
                    EndCinematic();
                }
                else
                {
                    // Next sentence time!
                    SetTextBoxVisible(CurSentence.OwningTextBox, false);

                    CurSentence = Conv.Sentences[SentenceIndex];
                    SetTextBoxVisible(CurSentence.OwningTextBox, true);
                    TimeToNextLetter = LetterDelay;
                    LetterIndex = 0;
                    bWaitForInput = false;
                }
            }
            else
            {
                bWaitForInput = true;
                CurSentence.OwningTextBox.text = CurSentence.Words;
            }
        }
	}

    private void SetTextBoxVisible(Text Box, bool Visible)
    {
        Box.transform.parent.gameObject.SetActive(Visible);
    }
}
