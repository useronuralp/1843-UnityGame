using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityBarTutorial : MonoBehaviour
{
    private bool m_IsEnlargmentAnimationDone = false;
    void Update()
    {
        if (m_IsEnlargmentAnimationDone)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_IsEnlargmentAnimationDone = false;
                EnableInputs();
                DestroySelf();
                EventManager.Get().EnableSanityBar();
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
        Destroy(transform.root.gameObject);
    }
}
