using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Safe : Clickable
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

    private AudioClip m_ButtonClickSound;
    private AudioSource m_AudioSource;
    private bool DoOnce;
    private GameObject m_Player;
    private bool m_WasTaskAdded = false;
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

        m_ItemName = "Safe";
        m_IsInTextScreen = false;
        m_IsPlayerCandleLit = false;
        EventManager.Get().OnPlayerPressedCandleToggleButton += OnPlayerTogglingCandle;
    }
    private void Update()
    {
        if (Vector3.Distance(m_Player.transform.position, transform.position) >= 2.0f)
        {
            if (DoOnce)
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
        if (string.CompareOrdinal(code, "5343") == 0 && !m_IsDisplayingEnding)
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
        EventManager.Get().DisableSanityBar();
        EventManager.Get().SafeEnding();
        m_IsDisplayingEnding = true;
        ScreenTransitions.FadeOutToCutscene(m_ItemPopUpWindow);
        m_LockPrompt.SetActive(false);
        EventManager.Get().PauseCandleCounter();
        StartCoroutine(TypeSentence(@"I never thought I would miss this feeling of being in control of this asylum.

Here I am, sitting in my chair after overthrowing the deputy director. As I remembered my past more and more, I embraced my past, my true nature again.

The documents that I found in his safe proved to be quite useful. I blackmailed him to leave his place and the rest was as easy as just claiming the empty throne...

I will continue my experiments in this asylum...

I will find a cure for my wife...

Some call what I do here is inhumane, I call it love. I will kill for my wife, I can't let her perish...

I will do whatever it takes to save her...  Even if it costs hundreds of people's life in the process.", 0.03f));
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
        //EventManager.Get().EnableJournalButton();
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
            if (!m_ItemPopUpWindow.activeInHierarchy && Vector3.Distance(m_Player.transform.position, transform.position) <= 1.3f && m_IsPlayerCandleLit)
            {
                if(!m_WasTaskAdded)
                {
                    EventManager.Get().UpdateTask("- Crack the code for the safe (Ending A).");
                    EventManager.Get().UpdateThought("There is a huge safe in the living room. There must be something worthwhile inside.");
                    m_WasTaskAdded = true;
                }
                m_LockPrompt.SetActive(true);
                DoOnce = true;
                EventManager.Get().DisableJournalButton();
            }
            else if (Vector3.Distance(m_Player.transform.position, transform.position) <= 1.3f && !m_IsPlayerCandleLit)
                EventManager.Get().TypeSentenceToHeadBubble("It's too dark here... I need light to see.");
        }
    }
}
