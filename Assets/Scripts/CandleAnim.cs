using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CandleAnim : MonoBehaviour
{
    private Sprite[] m_CandleAnimSprites;
    private Sprite[] m_CandleGoOutAnim;
    private Image Sprite;
    private int m_Counter;
    private float m_AnimationRate;
    public bool m_Animate = false;
    private bool m_IsTutorial;
    private int m_AnimationCount;
    private int m_GoOutAnimCount;
    private Image m_FillBar;
    private Image m_CandleBar;
    private bool DoOnce;
    private bool m_PlayGoOutAnim;
    private void Start()
    {
        DoOnce = true;
        m_CandleGoOutAnim = new Sprite[5];
        m_CandleGoOutAnim[0] = Resources.Load<Sprite>("Candle_Off/Candle_Off_00001");
        for (int i = 1; i < 5; i++)
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
            m_CandleGoOutAnim[i] = Resources.Load<Sprite>("Candle_Off/Candle_Off_00" + trail + (i + 1).ToString());
        }


        m_CandleAnimSprites = new Sprite[4];
        m_CandleAnimSprites[0] = Resources.Load<Sprite>("Candle_Icon/Candle_Icon_00001");
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
            m_CandleAnimSprites[i] = Resources.Load<Sprite>("Candle_Icon/Candle_Icon_00" + trail + (i + 1).ToString());
        }



        m_CandleBar = transform.parent.Find("CandleBar").GetComponent<Image>();
        m_FillBar = transform.parent.Find("CandleBar").Find("FillBar").GetComponent<Image>();

        m_AnimationCount = 4;
        m_GoOutAnimCount = 5;
        m_AnimationRate = 0.125f;
        Sprite = GetComponent<Image>();
        m_Counter = 0;

        EventManager.Get().OnAnimateCandle += OnEnableAnim;
        EventManager.Get().OnDoNotAnimateCandle += OnDisableAnim;
        EventManager.Get().OnResettingCandleAnim += OnResettingCandleAnim;
    }
    void Update()
    {
        if(m_PlayGoOutAnim)
        {
            m_AnimationRate -= Time.deltaTime;
            if (m_Counter >= m_GoOutAnimCount)
            {
                m_PlayGoOutAnim = false;
                m_Counter = 0;
            }
            else if (m_AnimationRate <= 0)
            {
                Sprite.sprite = m_CandleGoOutAnim[m_Counter];
                m_Counter++;
                m_AnimationRate = 0.125f;
            }
        }
        if(m_Animate)
        {
            if (m_Counter >= m_AnimationCount)
                m_Counter = 0;
            m_AnimationRate -= Time.deltaTime;
            if(m_AnimationRate <= 0)
            {
                Sprite.sprite = m_CandleAnimSprites[m_Counter];
                m_Counter++;
                m_AnimationRate = 0.125f;
            }
        }
    }
    public void OnDisableAnim()
    {

        m_Animate = false;
        m_PlayGoOutAnim = true;
        m_AnimationRate = 0.125f;
        m_Counter = 0;
        //Sprite.color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
        m_FillBar.color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
        m_CandleBar.color = new Color(0.3207547f, 0.3207547f, 0.3207547f);
    }
    public void OnEnableAnim()
    {
        m_Animate = true;
        m_PlayGoOutAnim = false;
        //Sprite.color = new Color(1.0f, 1.0f, 1.0f);
        m_FillBar.color = new Color(1.0f, 1.0f, 1.0f);
        m_CandleBar.color = new Color(1.0f, 1.0f, 1.0f);
    }
    public void OnResettingCandleAnim()
    {

        Sprite.sprite = m_CandleAnimSprites[0];
        m_Counter = 0;
        m_AnimationRate = 0.125f;
    }
}
