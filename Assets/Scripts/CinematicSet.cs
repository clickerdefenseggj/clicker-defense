using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CinematicSet : Set 
{
    //public enum Type
    //{
    //    HoarderConv,
    //    GuardExclaim,
    //    EnemyExclaim
    //}

    public enum Speaker
    {
        King,
        Guard, 
        Enemy
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
        public bool Randomize;
        public bool PauseGame;
    }

    public enum Type
    {
        HoarderConversation,
        RandomExclamation
    }

    public Text GuardTextBox;
    public Text KingTextBox;
    public Text EnemyTextBox;
    public Text TapToContinueText;

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

        // Handle enum types
        switch(CinematicType)
        {
            case Type.HoarderConversation:
                // NOTE (zesty): For special types, always use index 0, the list is used for other types that have
                //  a randomization feature
                Conv = GameData.Cinematics[CinematicType][0];
                break;
            case Type.RandomExclamation:
                Conv = GameData.Cinematics[CinematicType][Random.Range(0, GameData.Cinematics[CinematicType].Count)];
                break;
        }

        // TESTING
        //Conv = GameData.Cinematics[Type.RandomExclamation][3];

        LetterDelay = Conv.LetterDelay;
        TimeToNextLetter = LetterDelay;

        SentenceIndex = 0;
        LetterIndex = 0;
        CurSentence = Conv.Sentences[SentenceIndex];

        if(CurSentence.OwningTextBox == Speaker.Enemy)
        {
            Enemy Talker = App.inst.SpawnController.GetOnScreenEnemy();

            // No good enemy was found for text
            if(Talker == null)
            {
                EndCinematic();
                return;
            }

            Vector3 ViewportLocation = Camera.main.WorldToViewportPoint(Talker.transform.position);
            EnemyTextBox.rectTransform.anchoredPosition = ViewportLocation;
            print("(zesty): VPL = " + ViewportLocation);
        }

        CurTextBox = GetTextBox(CurSentence.OwningTextBox);
        SetTextBoxVisible(CurTextBox, true);
        bSentenceComplete = false;

        if(Conv.PauseGame)
            App.inst.SpawnController.PauseEnemiesForCinematic();
    }

    public void EndCinematic()
    {
        if(Conv.PauseGame)
            App.inst.SpawnController.UnpauseEnemiesAfterCinematic();

        SetManager.CloseSet(this);
    }

	// Use this for initialization
	void Start () 
    {
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

                if(Conv.PauseGame)
                    TapToContinueText.gameObject.SetActive(true);
                else
                {
                    StartCoroutine(HideExclamation());
                }

            }
            else
            {
                CurTextBox.text = CurSentence.Words.Substring(0, LetterIndex);
            }
        }

        if(bSentenceComplete == true && CurTextBox)
        {
            CurTextBox.text = CurSentence.Words;
        }

        if(Input.GetMouseButtonDown(0) && Conv.PauseGame)
        {
            if(bSentenceComplete == true)
            {
                TapToContinueText.gameObject.SetActive(false);
                ++SentenceIndex;
                if(SentenceIndex == Conv.Sentences.Count)
                {
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
                TapToContinueText.gameObject.SetActive(true);
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
                // Commenting out the breaks to remove warnings about unreachable code
            case Speaker.Guard:
                return GuardTextBox;
                //break;
            case Speaker.King:
                return KingTextBox;
                //break;
            case Speaker.Enemy:
                return EnemyTextBox;
                //break;
        }

        Debug.LogError("(zesty): ERROR!  Data file has bad speaker enums set.");
        return null;
    }

    private IEnumerator HideExclamation()
    {
        yield return new WaitForSeconds(2.0f);
        SetTextBoxVisible(CurTextBox, false);
    }
}
