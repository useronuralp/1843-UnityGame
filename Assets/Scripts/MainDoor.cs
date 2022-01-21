using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class MainDoor : Clickable
{
    private bool m_IsPlayerCandleLit;
    private TextMeshProUGUI m_Text;
    private GameObject m_ItemPopUpWindow;
    private GameObject m_LockPrompt;
    private bool m_IsInTextScreen;
    private bool m_IsDisplayingEnding;

    private TextMeshProUGUI m_DigitOne;
    private TextMeshProUGUI m_DigitTwo;
    private TextMeshProUGUI m_DigitThree;
    private TextMeshProUGUI m_DigitFour;
    private bool DoOnce;

    private AudioClip m_ButtonClickSound;
    private AudioSource m_AudioSource;
    private bool m_WasTaskAdded = false;
    private GameObject m_Player;
    protected override void Awake()
    {
        base.Awake();
        m_IsDisplayingEnding = false;
        m_ItemPopUpWindow = transform.Find("ItemCanvas").Find("ItemPopUpWindow").gameObject;
        m_LockPrompt = transform.Find("ItemCanvas").Find("LockPrompt").gameObject;
        m_Text = m_ItemPopUpWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        m_ItemPopUpWindow.SetActive(false);
    }
    void Start()
    {
        DoOnce = true;
        m_ButtonClickSound = Resources.Load<AudioClip>("Sound/HoverSound");
        m_AudioSource = transform.GetComponent<AudioSource>();
        m_DigitOne = m_LockPrompt.transform.Find("DigitOne").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>();
        m_DigitTwo = m_LockPrompt.transform.Find("DigitTwo").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>();
        m_DigitThree = m_LockPrompt.transform.Find("DigitThree").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>();
        m_DigitFour = m_LockPrompt.transform.Find("DigitFour").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>();

        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_ItemName = "Main Door";
        m_IsInTextScreen = false;
        m_IsPlayerCandleLit = false;
        EventManager.Get().OnPlayerPressedCandleToggleButton += OnPlayerTogglingCandle;
    }
    private void Update()
    {
        if(Vector3.Distance(m_Player.transform.position, transform.position) >= 3.0f)
        {
            if(DoOnce)
            {
                DoOnce = false;
                m_LockPrompt.SetActive(false);
                EventManager.Get().EnableJournalButton();
            }

        }
        if (GameState.IsCaught)
        {
            m_LockPrompt.SetActive(false);
        }
        string code = "";
        code += m_DigitOne.text[0];
        code += m_DigitTwo.text[0];
        code += m_DigitThree.text[0];
        code += m_DigitFour.text[0];
        if (string.CompareOrdinal(code, "6263") == 0 && !m_IsDisplayingEnding)
        {
            DisplayEnding();
        }
        if (m_IsInTextScreen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Find("FadeIn/Out").transform.Find("Canvas").Find("BlackScreen").GetComponent<Animator>().SetTrigger("FadeEndOfScene");
                EventManager.Get().FadeOutMusic(2.5f);
                Destroy(transform.Find("ItemCanvas").Find("PressToContinue").gameObject);
            }
        }
    }
    void DisplayEnding()
    {
        m_IsDisplayingEnding = true;
        EventManager.Get().DisableSanityBar();
        EventManager.Get().MainDoorEnding();
        ScreenTransitions.FadeOutToCutscene(m_ItemPopUpWindow);
        m_LockPrompt.SetActive(false);
        EventManager.Get().PauseCandleCounter();
        StartCoroutine(TypeSentence(@"So this is it... I did what my wife thought would be the right choice.

I did not surrender to my past and chose to it leave it behind me.

While escaping the prison and the grasp of the deputy director, I decided to burn it all...

But at what cost? 

I set fire to this asylum and buried my past, and any chance of my wife's survival with it too.

My wife wanted me to stop these experiments when I was trying to find a cure for her. I did everything for her... 

Yet she didn't want the cure, she thought what I was doing was evil. By experimenting on hundreds of innocent people I was going to save a single person.

Maybe she was right...", 0.03f));

    }
    public void OnPlayerTogglingCandle(bool candleStatus)
    {
        m_IsPlayerCandleLit = candleStatus;
    }
    protected IEnumerator TypeSentence(string sentence, float typeSpeed = 0) //Animating the letters with this.
    {
        m_Text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            m_Text.text += letter;
            if (typeSpeed == 0)
                yield return null;
            else
                yield return new WaitForSeconds(typeSpeed);
        }
        m_IsInTextScreen = true;
        EventManager.Get().EnableMouseInputs();
        transform.Find("ItemCanvas").Find("PressToContinue").gameObject.SetActive(true);
    }
    public override void OnPlayerLeavesInteractionRange()
    {
        //m_LockPrompt.SetActive(false);
    }
    public override void OnCompletingEnlargmentAnimation()
    {
        throw new System.NotImplementedException();
    }
    public void OnBackButtonPressed()
    {
        m_AudioSource.PlayOneShot(m_ButtonClickSound);
        m_LockPrompt.SetActive(false);
        EventManager.Get().EnableJournalButton();
    }
    public void OnUpButtonPressed(GameObject textField)
    {
        m_AudioSource.PlayOneShot(m_ButtonClickSound);
        var inputField = textField.transform.GetComponent<TMP_InputField>();
        Int32.TryParse(inputField.text, out int otp);
        otp++;
        if (otp > 9)
            otp = 0;
        inputField.text = (otp).ToString();
    }
    public void OnDownButtonPressed(GameObject textField)
    {
        m_AudioSource.PlayOneShot(m_ButtonClickSound);
        var inputField = textField.transform.GetComponent<TMP_InputField>();
        Int32.TryParse(inputField.text, out int otp);
        otp--;
        if (otp < 0)
            otp = 9;
        inputField.text = (otp).ToString();
    }
    public override void OnClick()
    {
        if (GameManager.Get().AreMouseInputsEnabled)
        {
            if (!m_ItemPopUpWindow.activeInHierarchy && Vector3.Distance(m_Player.transform.position, transform.position) <= 2.0f && m_IsPlayerCandleLit)
            {
                if(!m_WasTaskAdded)
                {
                    m_WasTaskAdded = true;
                    EventManager.Get().UpdateTask("- Find the code for the exit (Ending B).");
                    EventManager.Get().UpdateThought("I found the main exit but the door is locked. Where is the code I wonder...");
                }
                m_LockPrompt.SetActive(true);
                DoOnce = true;
                EventManager.Get().DisableJournalButton();
            }
            else if (Vector3.Distance(m_Player.transform.position, transform.position) <= 2.0f && !m_IsPlayerCandleLit)
                EventManager.Get().TypeSentenceToHeadBubble("It's too dark here... I need light to see.");

        }
    }
}
