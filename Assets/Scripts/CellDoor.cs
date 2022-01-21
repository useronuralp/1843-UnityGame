using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoor : MonoBehaviour
{
    private Animator m_Animator;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        EventManager.Get().OnPlayerCellGateOpen += OpenCellGate;
        EventManager.Get().OnPlayerCellGateClose += CloseCellGate;
    }
    void OpenCellGate()
    {
        m_Animator.SetTrigger("OpenCellGate");
    }
    void CloseCellGate()
    {
        m_Animator.SetTrigger("CloseCellGate");
    }
}
