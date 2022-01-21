using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCandle : Clickable
{
    private bool m_WasAlreadyGrabbedCnadle;
    private static bool m_StartListening;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        m_StartListening = false;
        m_WasAlreadyGrabbedCnadle = false;
        m_ItemName = "Candle";
    }
    private void Update()
    {
        if (m_StartListening)
        {
            if (!DialogueManager.Get().m_IsPlayerInDialogue)
            {
                m_StartListening = false;
                EventManager.Get().UpdateTask("- Find a way out of the prison cell.");
                EventManager.Get().UpdateThought("Oh thank god. With this candle, I should be able to see where I am going...");
                EventManager.Get().PickedUpCandle();
                enabled = false;
                transform.parent.Find("TogglePromptCanvas").gameObject.SetActive(true);
            }
        }
    }
    public override void OnPlayerLeavesInteractionRange()
    {
    }
    public override void OnCompletingEnlargmentAnimation() //The stuff you set in this function needs to be static.
    {
        m_StartListening = true;
        EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV0_PlayerSelf_CandlePickup }));
    }
    public override void OnClick()
    {
        if (m_IsPlayerInInteractionRange && !m_WasAlreadyGrabbedCnadle)
        {
            m_WasAlreadyGrabbedCnadle = true;
            EventManager.Get().ClickedCandle();
        }
    }
}
