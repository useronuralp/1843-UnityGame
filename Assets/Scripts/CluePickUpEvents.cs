using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CluePickUpEvents : MonoBehaviour
{
    private bool m_IsEnlargmentAnimationDone;
    private TextMeshProUGUI m_ObtainText;
    [SerializeField]
    private GameObject Clue;

    void Start()
    {
        m_ObtainText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        m_IsEnlargmentAnimationDone = false;
    }
    void Update()
    {
        if(m_IsEnlargmentAnimationDone)
        {
            if(Input.GetMouseButtonUp(0))
            {
                m_IsEnlargmentAnimationDone = false;
                EnableInputs();
                Clue.transform.Find("Object(Clickable)").GetComponent<Clickable>().OnCompletingEnlargmentAnimation();
                Destroy(gameObject);
            }
        }
    }
    public void DisableInputs()
    {
        EventManager.Get().DisableKeyboardInputs();
        EventManager.Get().DisableMouseInputs();
        m_ObtainText.text = "You have obtained " + Clue.name + ".";
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
}
