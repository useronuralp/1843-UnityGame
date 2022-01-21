using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Bucket : Clickable
{
    private bool m_IsPlayerCandleLit;
    private TextMeshProUGUI m_Text;
    private GameObject m_ItemPopUpWindow;
    private bool m_IsInTextScreen;
    private bool m_WasClicked;
    protected override void Awake()
    {
        base.Awake();
        m_WasClicked = false;
        m_ItemPopUpWindow = transform.Find("ItemCanvas").Find("ItemPopUpWindow").gameObject;
        m_Text = m_ItemPopUpWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        m_ItemPopUpWindow.SetActive(false);
    }
    void Start()
    {
        m_ItemName = "Bucket";
        m_IsInTextScreen = false;
        m_IsPlayerCandleLit = false;
        EventManager.Get().OnPlayerPressedCandleToggleButton += OnPlayerTogglingCandle;
    }
    private void Update()
    {
        if(m_IsInTextScreen)
        {
            if(Input.GetMouseButtonDown(0))
            {
                m_IsInTextScreen = false;
                bool disable = false;
                ScreenTransitions.FadeOutToCutscene(m_ItemPopUpWindow, disable);
                EventManager.Get().InteractedWithTheBucket();
                EventManager.Get().ContinueCandleCounter();
                Destroy(transform.Find("ItemCanvas").Find("PressToContinue").gameObject);
            }
        }
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
            if(typeSpeed == 0)
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
    }
    public override void OnCompletingEnlargmentAnimation()
    {
        throw new System.NotImplementedException();
    }
    public override void OnClick()
    {
        if (GameManager.Get().AreMouseInputsEnabled)
        {
            if (!m_ItemPopUpWindow.activeInHierarchy && m_IsPlayerInInteractionRange && m_IsPlayerCandleLit && !m_WasClicked)
            {
                m_WasClicked = true;
                ScreenTransitions.FadeOutToCutscene(m_ItemPopUpWindow);
                EventManager.Get().PauseCandleCounter();
                StartCoroutine(TypeSentence(@"Is this really me?

I look so old. I almost couldn't recognize myself.

Looks like I am being held prisoner here in this godforsaken place.

How long has it been? 10 years, 20? My face is full of wrinkles.

The damn pain in my head doesn't help either.

I need find a way out of this cell.", 0.03f));
            }
            else if (m_IsPlayerInInteractionRange && !m_IsPlayerCandleLit && !m_WasClicked)
                EventManager.Get().TypeSentenceToHeadBubble("It's too dark here... I need light to see.");
            else if (m_IsPlayerInInteractionRange && m_WasClicked && m_IsPlayerCandleLit && m_WasClicked)
            {
                EventManager.Get().TypeSentenceToHeadBubble("I don't want to see that horrifying face again...");
            }
        }
    }
}
