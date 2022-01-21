using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityAnim : MonoBehaviour
{
    private Sprite[] m_SanityAnimSprites;
    private Image Sprite;
    private int m_Counter;
    private float m_AnimationRate;
    public bool m_Animate = false;
    private bool m_IsTutorial;
    private int m_AnimationCount;
    private void Start()
    {
        //m_SanityAnimSprites = GameManager.GetSanityAnim();
        m_SanityAnimSprites = new Sprite[4];
        m_SanityAnimSprites[0] = Resources.Load<Sprite>("San_Icon/San_Icon_00001");
        for (int i = 1; i < 4; i++)
        {
            float digit = Mathf.Floor(Mathf.Log10(i) + 1);
            if (digit < 0)
                continue;
            string trail = "";
            switch ((int)(digit))
            {
                case 1: trail = "00"; break;
                case 2: trail = "0"; break;
                case 3: trail = ""; break;
            }
            m_SanityAnimSprites[i] = Resources.Load<Sprite>("San_Icon/San_Icon_00" + trail + (i + 1).ToString());
        }


        m_AnimationCount = 4;
        m_AnimationRate = 0.125f;
        Sprite = GetComponent<Image>();
        m_Counter = 0;

        //EventManager.Get().OnAnimateCandle += OnEnableAnim;
        //EventManager.Get().OnDoNotAnimateCandle += OnDisableAnim;
        //EventManager.Get().OnResettingCandleAnim += OnResettingCandleAnim;
    }
    void Update()
    {
        if (m_Counter >= m_AnimationCount)
            m_Counter = 0;
        m_AnimationRate -= Time.deltaTime;
        if (m_AnimationRate <= 0)
        {
            Sprite.sprite = m_SanityAnimSprites[m_Counter];
            m_Counter++;
            m_AnimationRate = 0.125f;
        }
    }
    //public void OnDisableAnim()
    //{
    //    m_Animate = false;
    //    Sprite.color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
    //}
    //public void OnEnableAnim()
    //{
    //    m_Animate = true;
    //    Sprite.color = new Color(1.0f, 1.0f, 1.0f);
    //}
    public void OnResettingSanityAnim()
    {
        Sprite.sprite = m_SanityAnimSprites[0];
        m_Counter = 0;
        m_AnimationRate = 0.125f;
    }
}
