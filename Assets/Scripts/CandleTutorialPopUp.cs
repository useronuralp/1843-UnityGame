using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleTutorialPopUp : MonoBehaviour
{
    private bool m_IsEnlargmentAnimationDone = false;
    private GameObject m_RMBTutorial;
    private void Start()
    {
        m_RMBTutorial = transform.parent.Find("PressRMB").gameObject;
    }
    void Update()
    {
        if (m_IsEnlargmentAnimationDone)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_IsEnlargmentAnimationDone = false;
                EnableInputs();
                m_RMBTutorial.SetActive(true);
                transform.gameObject.SetActive(false);
            }
        }
    }
    public void DisableInputs()
    {
        EventManager.Get().DisableKeyboardInputs();
        EventManager.Get().DisableMouseInputs();
    }
    public void EnableInputs()
    {
        EventManager.Get().EnableKeyboardInputs();
        EventManager.Get().EnableMouseInputs();
    }
    public void AnimationDone()
    {
        m_IsEnlargmentAnimationDone = true;
    }
    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
