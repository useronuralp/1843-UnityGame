using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using TMPro;
using UnityEngine.AI;
public class Guard : MonoBehaviour
{
    private ThirdPersonCharacter m_Character;
    public GameObject m_MovePoint;
    private NavMeshAgent m_Agent;
    private GameObject m_Player;
    private bool DoOnce = true;
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player");
        m_Character = GetComponent<ThirdPersonCharacter>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        EventManager.Get().OnPlayerCompletesTutorialRoomTasks += MoveToTalkPoint;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, m_MovePoint.transform.position) <= m_Agent.stoppingDistance)
        {
            if(DoOnce)
            {
                DoOnce = false;
                EventManager.Get().OpenPlayerCellGate();
                EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV0_Guard1, DialogueDatabase.Get().LV0_Player1, DialogueDatabase.Get().LV0_Guard2, DialogueDatabase.Get().LV0_Player2}), true);
            }
            FaceTarget(m_Player.transform.position);
        }
        if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            //Below 0.5f is walking animaiton.
            m_Character.Move(new Vector3(m_Agent.desiredVelocity.x, m_Agent.desiredVelocity.y, 0.6f), false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
    void MoveToTalkPoint()
    {
        m_Agent.destination = m_MovePoint.transform.position;
    }
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f);
    }
}
