using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Cameras; //Camera 0 must always be the player follow camera.
    private Cinemachine.CinemachineVirtualCamera[] m_VCams;

    public bool AreMouseInputsEnabled;
    public bool AreKeyboardInputsEnabled;

    private int m_TutorialTaskCount; //When this is satisfied. The tutrial guard comes and starts talking to the player.

    private static GameManager s_Instance;

    public GameObject CandleImage;
    private GameObject m_GameOverScreen;
    private GameObject m_CandleCanvas;
    private GameObject m_SanityBarCanvas;

    private TextMeshProUGUI m_CandleTimerText;
    private TextMeshProUGUI m_CandleCountText;

    private Image m_SanityFillBar;
    private Image m_CandleFillBar;
    //Candle stuff
    public float m_CandleTimer;
    public bool m_IsCandleLit;
    public int m_CandleCount;
    public bool m_PauseCandleCounter;
    public bool m_IsOutOfCandles;
    private bool m_IsSanityBarEnabled;
    private bool m_IsInTutorial;

    private AudioClip m_JournalAcquireSound;
    private AudioClip m_ButtonClick;
    private AudioClip m_ButtonHoverSound;
    private AudioSource m_AudioSource;
    private AudioSource m_HeartBeatSound;

    private bool DoOnce = true;
    [SerializeField]
    private GameObject LMBTutorial;
    private AudioClip m_ItemPickUpSound;

    private void Awake()
    {


        s_Instance = this;
        //All the cameras go in here------------------------------
        m_VCams = new Cinemachine.CinemachineVirtualCamera[Cameras.Length];
        for(int i = 0; i < Cameras.Length; i++) //Set the VCAMs
        {
            m_VCams[i] = Cameras[i].GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_JournalAcquireSound = Resources.Load<AudioClip>("Sound/JournalPage");
        m_HeartBeatSound = transform.Find("HeartBeatSound").GetComponent<AudioSource>();
        m_ButtonClick = Resources.Load<AudioClip>("Sound/ButtonClick");
        m_ButtonHoverSound = Resources.Load<AudioClip>("Sound/HoverSound");
        m_ItemPickUpSound = Resources.Load<AudioClip>("Sound/ItemPickUp");
        //if (PlayerPrefs.GetFloat("CurrentLevel") == 4)
        //    GameState.HasSeenLevelThreeBefore = true;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            m_IsInTutorial = true;
            PlayerPrefs.SetFloat("CurrentLevel", 2);
            GameState.HasSeenLevelThreeBefore = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            PlayerPrefs.SetFloat("CurrentLevel", 3);
            m_IsInTutorial = false;
            GameState.HasSeenLevelThreeBefore = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            PlayerPrefs.SetFloat("CurrentLevel", 4);
            m_IsInTutorial = false;
            EventManager.Get().UpdateTask("- Find a way out of the prison cell.");
            EventManager.Get().UpdateTask("- Search for clues.");
            EventManager.Get().UpdateThought("That was hell of a ride. There is something seriously wrong with this place. Better look around...");
        }
        //if (SceneManager.GetActiveScene().buildIndex == 2)
        //{
        //    m_IsInTutorial = true;
        //}
        //else
        //    m_IsInTutorial = false;


        m_IsSanityBarEnabled = false;
        m_IsOutOfCandles = false;
        m_CandleCount = 1;
        m_CandleTimer = 60;
        m_IsCandleLit = false;
        AreMouseInputsEnabled = false;
        AreKeyboardInputsEnabled = false;
        m_PauseCandleCounter = false;

        m_TutorialTaskCount = 0;

        EventManager.Get().OnClickingCandle += OnClickingCandle;

        EventManager.Get().OnInteractingWithBucket += OnCompletingBucketEvent;

        EventManager.Get().OnClosingJournal += OnClosingJournal;

        EventManager.Get().OnPlayerPressedCandleToggleButton += OnCandleToggled;

        //Event count for the tutorial section -----------
        EventManager.Get().OnPickingUpShirt += OnPickingUpShirt;
        EventManager.Get().OnPickingUpCandle += OnPickingUpCandle;

        //Enable/Disable inputs ----------------
        EventManager.Get().OnMouseInputsDisabled += OnMouseInputsGetDisabled;
        EventManager.Get().OnMouseInputsEnabled += OnMouseInputsGetEnabled;
        EventManager.Get().OnKeyboardInputsEnabled += OnKeyboardEventsGetEnabled;
        EventManager.Get().OnKeyboardInputsDisabled += OnKeyboardEventsGetDisabled;

        EventManager.Get().OnDisableUI += OnDisableUI;
        EventManager.Get().OnEnableUI += OnEnableUI;

        EventManager.Get().OnPauseCandleCounter += PauseCandleCounter;
        EventManager.Get().OnContinueCandleCounter += ContinueCandleCounter;

        EventManager.Get().OnAddCandle += OnAddToCandleCounter;

        EventManager.Get().OnGameOver += OnGameOver;

        EventManager.Get().OnEnableSanityBar += OnEnableSanityBar;
        EventManager.Get().OnDisableSanityBar += OnDisableSanityBar;

        EventManager.Get().OnCloseGameOverCanvas += OnCloseGameOverCanvas;

        EventManager.Get().OnLMBTutorial += OnLMBTutorial;
        EventManager.Get().OnSanityTutorial += OnSanityTutorial;

        EventManager.Get().OnEnableSound += OnEnableSound;
        EventManager.Get().OnDisableSound += OnDisableSound;

        EventManager.Get().OnSafeEnding += OnEndingSafe;
        EventManager.Get().OnMainDoorEnding += OnEndingMainDoor;

        EventManager.Get().OnClickingAclue += PlayItemPickUpSound;
        m_GameOverScreen = transform.Find("GameOverScreen").gameObject;
        m_CandleTimerText = transform.Find("UI").Find("CandleCanvas").Find("CandleTimer").GetComponent<TextMeshProUGUI>();
        m_CandleCountText = transform.Find("UI").Find("CandleCanvas").Find("CandleCount").GetComponent<TextMeshProUGUI>();
        m_SanityFillBar = transform.Find("UI").Find("SanityBarCanvas").Find("SanityBar").Find("FillBar").GetComponent<Image>();
        m_CandleCanvas = transform.Find("UI").Find("CandleCanvas").gameObject;
        m_CandleFillBar = m_CandleCanvas.transform.Find("CandleBar").Find("FillBar").GetComponent<Image>();
        m_SanityBarCanvas = transform.Find("UI").Find("SanityBarCanvas").gameObject;
    }
    private void Update()
    {
        if(m_IsSanityBarEnabled)
        {
            if(m_SanityFillBar.fillAmount > 0.0f && m_SanityFillBar.fillAmount <= 0.2f)
            {
                StopCoroutine(nameof(StartFade));
                if(DoOnce)
                    m_HeartBeatSound.Play();
                DoOnce = false;
            }
            else
            {
                DoOnce = true;
                m_HeartBeatSound.Stop();
            }
            if(m_CandleCanvas.activeInHierarchy && m_SanityFillBar.gameObject.activeInHierarchy)
            {
                if(m_IsCandleLit)
                {
                    m_SanityFillBar.fillAmount += Time.deltaTime * 0.05f;
                }
                else
                {
                    m_SanityFillBar.fillAmount -= Time.deltaTime * 0.05f;
                }
            }
            if(m_SanityFillBar.fillAmount == 0.0f)
            {
                switch(UnityEngine.Random.Range(0,3))
                {
                    case 0: EventManager.Get().GameOver("You have lost your mind.", "Tip: Use your candle periodically to regenerate the sanity bar."); break;
                    case 1: EventManager.Get().GameOver("You have lost your mind.", "Tip: There are hidden candles around the area."); break;
                    case 2: EventManager.Get().GameOver("You have lost your mind.", "Tip: If your sanity bar fully depletes, you will go mad."); break;
                }
                m_IsSanityBarEnabled = false;
                OnDisableSound();
            }
        }
        if(m_IsInTutorial)
        {
            m_CandleCountText.text = "x " + "\u221E";
        }
        else
        {
            m_CandleCountText.text = "x" + m_CandleCount.ToString();
        }
        if(m_IsCandleLit)
        {
            m_CandleTimerText.text = TimeSpan.FromSeconds(m_CandleTimer).ToString(@"mm\:ss");
            if(!m_PauseCandleCounter)
            {
                EventManager.Get().AnimateCandle();
                m_CandleTimer -= Time.deltaTime;
            }
        }
        if(m_CandleTimer <= 0 && m_CandleCount == 0)
        {
            m_IsCandleLit = false;
            m_IsOutOfCandles = true;
            EventManager.Get().DisableCandleUse(); //Informs Player.cs  These two can be merged.
            EventManager.Get().ForceTurnOffCandle(); //Informs Player.cs
            m_CandleTimer = 60;
        }
        else if(m_CandleTimer <= 0 && m_CandleCount > 0)
        {
            if(!m_IsInTutorial)
                m_CandleCount--;
            m_CandleTimer = 60;
            EventManager.Get().ResetCandleAnim();
        }
        m_CandleFillBar.fillAmount = m_CandleTimer / 60;
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void SwitchCameras(Cinemachine.CinemachineVirtualCamera from, Cinemachine.CinemachineVirtualCamera to)
    {
        from.Priority = 0;
        to.Priority = 1;
    }
    public void SwitchToPlayerCamera()
    {
        m_VCams[0].Priority = 1; //player cam
        for(int i = 1; i < m_VCams.Length; i++) //all other cams
        {
            m_VCams[i].Priority = 0;
        }
    }
    public static GameManager Get()
    {
        return s_Instance;
    }
    void OnPickingUpCandle()
    {
        transform.Find("UI").Find("CandleCanvas").gameObject.SetActive(true);
        transform.Find("UI").Find("CandleCanvas").Find("CandleImage").GetComponent<Animator>().SetTrigger("AnimateCandleIntro");
    }

    void OnPickingUpShirt()
    {
        m_AudioSource.PlayOneShot(m_JournalAcquireSound);
        transform.Find("UI").Find("JournalToggleButtonCanvas").gameObject.SetActive(true);
        transform.Find("UI").Find("JournalToggleButtonCanvas").Find("JournalToggleButton").GetComponent<Animator>().SetTrigger("AnimateJournalButton");


        m_TutorialTaskCount++;
        if(m_TutorialTaskCount == 3)
        {
            EventManager.Get().PlayerCompletedTutorialRoomTasks();
        }
    }
    void OnCompletingBucketEvent()
    {
        m_TutorialTaskCount++;
        if(m_TutorialTaskCount == 3)
        {
            EventManager.Get().PlayerCompletedTutorialRoomTasks();
        }
    }
    void OnClosingJournal()
    {
        m_TutorialTaskCount++;
        if(m_TutorialTaskCount == 3)
        {
            EventManager.Get().PlayerCompletedTutorialRoomTasks();
        }
    }
    void OnClickingCandle()
    {
        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var candlePreview = Instantiate(CandleImage, new Vector3(0, 0, 0), Quaternion.identity);
        candlePreview.transform.SetParent(transform.Find("ItemPreviewCanvas").transform, false);
    }
    void OnCandleToggled(bool candleStatus)
    {
        if (candleStatus)
        {
            m_IsCandleLit = candleStatus; //true
            EventManager.Get().AnimateCandle();
        }
        else
        {
            m_IsCandleLit = candleStatus; //false
            EventManager.Get().DoNotAnimateCandle();
        }
    }
    public Cinemachine.CinemachineVirtualCamera GetCamera(string cameraName)
    {
        foreach(var cam in Cameras)
        {
            if(cam.name.Contains(cameraName))
            {
                return cam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            }
        }
        return null;
    }
    void OnMouseInputsGetDisabled()
    {
        AreMouseInputsEnabled = false;
    }
    void OnMouseInputsGetEnabled()
    {
        AreMouseInputsEnabled = true;
    }
    void OnKeyboardEventsGetEnabled()
    {
        AreKeyboardInputsEnabled = true;
    }
    void OnKeyboardEventsGetDisabled()
    {
        AreKeyboardInputsEnabled = false;
    }
    void OnEnableUI()
    {
        transform.Find("UI").gameObject.SetActive(true);
    }
    void OnDisableUI()
    {
        transform.Find("UI").gameObject.SetActive(false);
    }
    void PauseCandleCounter()
    {
        m_PauseCandleCounter = true;
        EventManager.Get().DoNotAnimateCandle();
    }
    void ContinueCandleCounter()
    {
        m_PauseCandleCounter = false;
        if(m_CandleCount > 0 && m_CandleTimer > 0 && m_IsCandleLit)
            EventManager.Get().AnimateCandle();
    }
    void OnAddToCandleCounter()
    {
        if (m_CandleCount == 0 && m_IsOutOfCandles)
        {
            m_IsOutOfCandles = false;
            m_CandleTimer = 60;
            EventManager.Get().EnableCandleUse();
            EventManager.Get().ResetCandleAnim();
        }
        else
            m_CandleCount++;
    }
    void OnGameOver(string cause, string tip)
    {
        m_GameOverScreen.SetActive(true);
        m_GameOverScreen.transform.Find("GameOverCanvas").Find("Cause").GetComponent<TextMeshProUGUI>().text = cause;
        m_GameOverScreen.transform.Find("GameOverCanvas").Find("Tip").GetComponent<TextMeshProUGUI>().text = tip;
    }
    public void OnRestartButtonPressed()
    {
        m_AudioSource.Stop();
        m_AudioSource.volume = 1.0f;
        m_AudioSource.PlayOneShot(m_ButtonHoverSound, 0.4f);
        OnEnableSound();
        GameObject.Find("FadeIn/Out").transform.Find("Canvas").Find("BlackScreen").GetComponent<Animator>().SetTrigger("FadeOutGameOver");
    }
    public void OnMainMenuButtonPressed()
    {
        m_AudioSource.Stop();
        m_AudioSource.volume = 1.0f;
        m_AudioSource.PlayOneShot(m_ButtonHoverSound, 0.4f);
        GameObject.Find("FadeIn/Out").transform.Find("Canvas").Find("BlackScreen").GetComponent<Animator>().SetTrigger("FadeOutMainMenu");
    }
    void OnCloseGameOverCanvas()
    {
        m_GameOverScreen.SetActive(false);
    }
    void OnEnableSanityBar()
    {
        m_IsSanityBarEnabled = true;
    }
    void OnDisableSanityBar()
    {
        m_IsSanityBarEnabled = false;
    }
    void OnLMBTutorial()
    {
        Instantiate(LMBTutorial);
    }
    void OnSanityTutorial()
    {
        //m_IsSanityBarEnabled = false;
        GameObject.Find("SanityBarTutorial").transform.Find("SanityBarCanvas").gameObject.SetActive(true);
    }
    public IEnumerator StartFade(float duration, float targetVolume) //This fnc is useful for slowly fading in / out a music track.
    {
        float currentTime = 0;
        float start = m_AudioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        m_AudioSource.Stop();
        yield break;
    }
    void OnEnableSound()
    {
        //m_HeartBeatSound.Play();
    }
    void OnDisableSound()
    {
        m_HeartBeatSound.Stop();
        EventManager.Get().FadeOutMusic(1.0f);
    }
    void OnEndingSafe()
    {
        m_HeartBeatSound.Stop();
        GameObject.Find("GuardManager").SetActive(false);
    }
    void OnEndingMainDoor()
    {
        GameObject.Find("GuardManager").SetActive(false);
        m_HeartBeatSound.Stop();
    }
    public void PlayHoverSound()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick, 0.2f);
    }
    public void PlayItemPickUpSound()
    {
        m_AudioSource.PlayOneShot(m_ItemPickUpSound, 0.3f);
    }
}
