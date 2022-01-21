using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRange : MonoBehaviour
{
    private Clickable m_ClickableScript;
    private GameObject m_Player;
    private GameObject m_Clickable;
    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_ClickableScript = transform.parent.Find("Object(Clickable)").GetComponent<Clickable>();
        m_Clickable = transform.parent.Find("Object(Clickable)").gameObject;
    }
    private void Update()
    {
        if(Vector3.Distance(new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 1.3f, m_Player.transform.position.z), m_Clickable.transform.position) <= 1.3f)
        {
            m_ClickableScript.m_IsPlayerInInteractionRange = true;
        }
        else
        {
            m_ClickableScript.m_IsPlayerInInteractionRange = false;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        m_ClickableScript.m_IsPlayerInInteractionRange = true;
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //  
    //    if(other.CompareTag("Player"))
    //    {
    //        m_ClickableScript.m_IsPlayerInInteractionRange = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Left");
    //        m_ClickableScript.m_IsPlayerInInteractionRange = false;
    //        m_ClickableScript.OnPlayerLeavesInteractionRange();
    //    }
    //}

}
