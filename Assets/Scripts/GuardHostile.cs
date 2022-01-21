using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using TMPro;
using UnityEngine.AI;

public class GuardHostile : MonoBehaviour
{
    private ThirdPersonCharacter m_ThirdPersonCharacter;
    public GameObject m_MovePoint_Bedroom;
    public GameObject m_MovePoint_Bathroom;
    public GameObject m_MovePoint_Surgery;
    public GameObject m_MovePoint_Surgery2;
    public GameObject m_MovePoint_Office;
    public GameObject m_MovePoint_Classroom;
    private int m_CurrentWayPointIndex;
    private GameObject m_CurrentWayPoint;
    private int m_PreviousWayPointIndex;
    private NavMeshAgent m_Agent;
    private GameObject m_Player;
    private float m_WaitTimer;
    public bool m_Wander;
    private float m_WaitDuration;
    public bool m_Hostile;
    public float m_SeeRange;
    private bool doOnce = true;
    private bool doOnce2 = true;
    private bool doOnce3 = true;
    private Dictionary<GameObject, int> m_RandomTable;
    private bool m_PlayerCandleStatus;
    private bool m_InSearchMode;

    private float m_SpeakTimer;
    private float m_SpeakCooldown = 5.0f;
    private TextMeshPro m_TextHead;
    public float m_BackAwarenessDistance = 2.0f;
    private string[] m_Lines;

    public GameObject ResetPoint;
    private void Awake()
    {
        m_TextHead = transform.Find("TextBubbleHead").GetComponent<TextMeshPro>();
        m_TextHead.GetComponent<LookAtCamSprite>().enabled = true;
    }
    private void OnEnable()
    {
        m_TextHead.text = "";
    }

    void Start()
    {
        m_Lines = new string[5];
        m_Lines[0] = "Come back here!";
        m_Lines[1] = "I will break your legs!";
        m_Lines[2] = "Stop running!";
        m_Lines[3] = "I will HURT you if I catch you!";
        m_Lines[4] = "You can't run forever!";
        m_SpeakTimer = m_SpeakCooldown; 
        m_InSearchMode = false;
        m_Wander = false;
        m_RandomTable = new Dictionary<GameObject, int>() { { m_MovePoint_Bedroom, 0}, { m_MovePoint_Bathroom, 0 }, { m_MovePoint_Surgery, 0 }, { m_MovePoint_Surgery2, 0 }, { m_MovePoint_Office, 0 }, { m_MovePoint_Classroom, 0 } };
        m_SeeRange = 4.0f;
        m_WaitDuration = 2.0f;
        m_WaitTimer = m_WaitDuration;
        m_CurrentWayPointIndex = 0;
        m_PreviousWayPointIndex = 0;
        m_Player = GameObject.FindWithTag("Player");
        m_ThirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;



        EventManager.Get().OnGuardStartsWandering += OnStartWandering;
        EventManager.Get().OnPlayerPressedCandleToggleButton += OnCandleToggle;
    }
    private void Update()
    {
        if(m_PlayerCandleStatus)
        {
            m_SeeRange = 6;
            m_BackAwarenessDistance = 4;
        }
        else if(!m_InSearchMode && !m_Hostile)
        {
            m_SeeRange = 3;
            m_BackAwarenessDistance = 2;
        }

        Vector3 eyes = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
        Vector3 m_PlayerDirection = m_Player.transform.position - transform.position;
        if (m_PlayerDirection.magnitude <= 1.0f)
        {
            if (doOnce)
            {
                doOnce = false;
                EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV0_Guard1 }));
                m_Wander = false;
                m_Agent.isStopped = true;
                m_Agent.ResetPath();
                GameState.IsCaught = true;
                EventManager.Get().DisableSound();
            }
        }
        bool isHit = Physics.Linecast(eyes, new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 1.3f, m_Player.transform.position.z));
        if (((((m_PlayerDirection.magnitude < m_SeeRange && Vector3.Angle(m_PlayerDirection, transform.forward) <= 45)) || m_PlayerDirection.magnitude <= m_BackAwarenessDistance)
            && ((Mathf.Abs(m_Player.transform.position.y - transform.position.y) >= 0.0f) && (Mathf.Abs(m_Player.transform.position.y - transform.position.y) <= 0.3f))) && !isHit)
        {
            if (doOnce2 && !GameState.IsCaught)
            {
                doOnce2 = false;
                HeadBubbleSpeech(m_Lines[Random.Range(0, 4)]);
            }
            //Debug.DrawLine(eyes, new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 1.3f, m_Player.transform.position.z), Color.red);
            if(doOnce3)
            {
                doOnce3 = false;
                EventManager.Get().StartChaseMusic();
            }
            m_Hostile = true;
            m_SeeRange = 6.0f;
            m_Agent.destination = m_Player.transform.position;
        }
        else
        {
            if (m_Hostile)
            {
                m_Agent.destination = m_Player.transform.position;
                m_InSearchMode = true;
                m_Hostile = false;
            }
            //Debug.DrawLine(eyes, new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 1.3f, m_Player.transform.position.z), Color.green);
        }



        if (m_Hostile)
            m_Agent.speed = 1.0f;
        else
            m_Agent.speed = 0.5f;

        if (m_Hostile && !doOnce2)
        {
            m_SpeakTimer -= Time.deltaTime;
            if(m_SpeakTimer < 0.0f)
            {
                doOnce2 = true;
                m_SpeakTimer = m_SpeakCooldown;
            }
        }
        if(m_Wander)
        {
            if (!m_Agent.pathPending)
            {
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    if (!m_Agent.hasPath || m_Agent.velocity.sqrMagnitude == 0f)
                    {
                        m_WaitTimer -= Time.deltaTime;
                        if (m_WaitTimer <= 0.0f)
                        {
                            MoveToWalkPoint();
                            m_Hostile = false;
                            m_WaitTimer = m_WaitDuration;
                        }
                    }
                }
            }
        }
        if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            //Below 0.5f is walking animaiton.
            m_ThirdPersonCharacter.Move(m_Agent.desiredVelocity, false, false);
        }
        else
        {
            m_ThirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
    void MoveToWalkPoint()
    {
        doOnce2 = true;
        m_InSearchMode = false;
        m_SeeRange = 3.0f;
        if(!doOnce3)
        {
            EventManager.Get().ContinueExploreMusic();
            doOnce3 = true;
        }
        m_CurrentWayPointIndex = Random.Range(0, 6);
        if(m_CurrentWayPointIndex == m_PreviousWayPointIndex)
        {
            m_CurrentWayPointIndex = (++m_CurrentWayPointIndex) % 6;
        }
        m_PreviousWayPointIndex = m_CurrentWayPointIndex;
        switch (m_CurrentWayPointIndex)
        {
            case 0: m_CurrentWayPoint = m_MovePoint_Bedroom; m_RandomTable[m_MovePoint_Bedroom]++; break;
            case 1: m_CurrentWayPoint = m_MovePoint_Bathroom; m_RandomTable[m_MovePoint_Bathroom]++; break;
            case 2: m_CurrentWayPoint = m_MovePoint_Surgery; m_RandomTable[m_MovePoint_Surgery]++; break;
            case 3: m_CurrentWayPoint = m_MovePoint_Surgery2; m_RandomTable[m_MovePoint_Surgery2]++; break;
            case 4: m_CurrentWayPoint = m_MovePoint_Office; m_RandomTable[m_MovePoint_Office]++; break;
            case 5: m_CurrentWayPoint = m_MovePoint_Classroom; m_RandomTable[m_MovePoint_Classroom]++; break;
        }
        m_Agent.destination = m_CurrentWayPoint.transform.position;
    }
    void OnStartWandering()
    {
        m_Wander = true;
    }
    void OnCandleToggle(bool candleStatus)
    {
        m_PlayerCandleStatus = candleStatus;
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
    public void ResetLocation()
    {
        m_Agent.isStopped = false;
        m_Wander = true;
        m_Agent.ResetPath();
        m_Agent.Warp(ResetPoint.transform.position);
    }
}
