using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shirt : Clickable
{
    private bool m_IsPlayerCandleLit;
    private bool m_WasAlreadyClicked;
    private static bool m_StartListening;
    private AudioClip m_JournalAcquireSound;
    protected override void Awake()
    {
        base.Awake();
        m_ItemName = "Shirt";
    }
    void Start()
    {
        m_IsPlayerCandleLit = false;
        m_StartListening = false;
        m_WasAlreadyClicked = false;
        EventManager.Get().OnPlayerPressedCandleToggleButton += OnPlayerTogglingCandle;
    }

    void Update()
    {
        if(m_StartListening)
        {
            if(!DialogueManager.Get().m_IsPlayerInDialogue)
            {
                m_StartListening = false;
                EventManager.Get().PickedUpShirt();
                Destroy(transform.parent.gameObject);
            }
        }
    }
    public override void OnPlayerLeavesInteractionRange()
    {
    }
    public override void OnCompletingEnlargmentAnimation() //TODO : This part is a little problematic. This function is being called from tha main prefab not from the instance in the scene. So whatever you set here needs to be STATIC.
    {
        m_StartListening = true;
        EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV0_PlayerSelf_ShirtPickup }));
    }
    public void OnPlayerTogglingCandle(bool candleStatus)
    {
        m_IsPlayerCandleLit = candleStatus;
    }
    public override void OnClick()
    {
        if (m_IsPlayerInInteractionRange && !m_WasAlreadyClicked && m_IsPlayerCandleLit)
        {
            m_WasAlreadyClicked = true;
            EventManager.Get().ClickedShirt();
        }
        else if (m_IsPlayerInInteractionRange && !m_IsPlayerCandleLit)
        {
            EventManager.Get().TypeSentenceToHeadBubble("I need light to see this...");
        }
    }
}
