using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour
{

    NavMeshAgent                 m_Agent;
    private LayerMask            m_TerrainLayerMask;
    private LayerMask            m_ClickableLayerMask;
                                 
    private TextMeshPro          m_TextHead;
    private Animator             m_Animator;
    private ThirdPersonCharacter m_Character;

    public bool                  m_UnlockedCandle;
    private Light                m_Candle;

    public bool                  m_IsCandleUsable;
    private float                m_CandleStartIntensity;
    private float                m_CandleStartRange;
    private float                m_FlickerTimer;

    private void Start()
    {
        m_FlickerTimer = 0.5f;
        //m_IsCandleUsable = false;
        m_Character = GetComponent<ThirdPersonCharacter>();
        m_Animator = GetComponent<Animator>();
        m_TerrainLayerMask = LayerMask.GetMask("Walkable");
        m_ClickableLayerMask = LayerMask.GetMask("Clickable");
        m_Agent = GetComponent<NavMeshAgent>();
        m_TextHead = transform.Find("TextBubbleHead").GetComponent<TextMeshPro>();
        m_TextHead.GetComponent<LookAtCamSprite>().enabled = true;
        m_Candle = transform.Find("CandleLight").GetComponent<Light>();
        EventManager.Get().OnTypeSentenceToHeadBubble += HeadBubbleSpeech;
        EventManager.Get().OnPickingUpCandle += OnPickingUpCandle;
        EventManager.Get().OnForceTurnOffCandle += OnForceTurnOffCandle;
        EventManager.Get().OnDisableCandleUse += OnDisableCandleUse;
        EventManager.Get().OnEnableCandleUse += OnEnableCandleUse;
        //m_UnlockedCandle = false;
        m_Agent.updateRotation = false;
        m_CandleStartIntensity = m_Candle.intensity;
        m_CandleStartRange = m_Candle.range;

    }
    void Update()
    {
        //Debug.Log(m_UnlockedCandle);
        if(GameState.IsCaught)
        {
            m_Agent.isStopped = true;
        }
        if(m_Candle.enabled)
        {
            m_FlickerTimer -= Time.deltaTime;
            if(m_FlickerTimer <= 0.0f)
            {
                m_FlickerTimer = 0.05f;
                m_Candle.range = m_CandleStartRange;
                m_Candle.range += Random.Range(-0.1f, 0.1f);
                m_Candle.intensity = m_CandleStartIntensity;
                m_Candle.intensity += Random.Range(-1.0f, 1.0f);
            }
        }
        
        //Debug.Log("keyboard: " + GameManager.Get().AreKeyboardInputsEnabled);
        //Debug.Log("mouse: " + GameManager.Get().AreMouseInputsEnabled);
        if (m_Agent.remainingDistance < 1.0f)
            m_Agent.speed = 0.6f;
        else
            m_Agent.speed = 1.0f;

        if(GameManager.Get().AreKeyboardInputsEnabled)
        {
            //Candle toggle.
            if(m_IsCandleUsable)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    if (m_UnlockedCandle)
                    {
                        m_Candle.enabled = !m_Candle.enabled;
                        EventManager.Get().PlayerPressedCandleToggleButton(m_Candle.enabled);
                    }
                }
            }
        }
        if (GameManager.Get().AreMouseInputsEnabled)
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.Get().IsMouseOverUI())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, m_TerrainLayerMask | m_ClickableLayerMask))
                {
                    var vectorToTarget = transform.position - hit.point;
                    vectorToTarget.y = 0;
                    var distanceToTarget = vectorToTarget.magnitude;
                    if(distanceToTarget <= 0.5f)
                    {
                        m_Agent.ResetPath();
                    }
                    else
                        m_Agent.destination = hit.point;
                }
            }
        }
        if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            m_Character.Move(m_Agent.desiredVelocity, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
    //World Events--------------------------------------
    void OnPickingUpCandle()
    {
        m_UnlockedCandle = true;
        m_Candle.enabled = true;
        EventManager.Get().PlayerPressedCandleToggleButton(m_Candle.enabled);
    }
    void HeadBubbleSpeech(string sentence)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentenceHeadText(sentence));
    }
    //Input Events--------------------------------------
    IEnumerator TypeSentenceHeadText(string sentence)
    {
        m_TextHead.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            m_TextHead.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.0f);
        m_TextHead.text = "";
    }
    void OnForceTurnOffCandle()
    {
        m_Candle.enabled = false;
        EventManager.Get().PlayerPressedCandleToggleButton(m_Candle.enabled);
    }
    void OnDisableCandleUse()
    {
        m_IsCandleUsable = false;
    }
    void OnEnableCandleUse()
    {
        m_IsCandleUsable = true;
    }
}