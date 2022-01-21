using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    private AudioClip MainMenuMusic;
    private AudioClip ChaseMusic;
    private AudioClip ExploreMusic;
    private AudioClip TutorialMusic;
    private AudioClip DirectorMusic;
    private AudioClip GoodEnding;
    private AudioClip BadEnding;
    private AudioSource m_AudioSource;

    private static MusicManager s_Instance;
    float m_ChaseMusicTime = 0.0f;
    float m_ExploreMusicTime = 0.0f;
    // Start is called before the first frame update
    private void Awake()
    {
        if(s_Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            s_Instance = this;
        }
    }
    void Start()
    {
        MainMenuMusic = Resources.Load<AudioClip>("Sound/MainMenu");
        DirectorMusic = Resources.Load<AudioClip>("Sound/Director");
        ChaseMusic = Resources.Load<AudioClip>("Sound/ChaseMusic");
        ExploreMusic = Resources.Load<AudioClip>("Sound/ExploreMusic");
        TutorialMusic = Resources.Load<AudioClip>("Sound/Tutorial");

        GoodEnding = Resources.Load<AudioClip>("Sound/GoodEnding");
        BadEnding = Resources.Load<AudioClip>("Sound/BadEnding");

        m_AudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnLoadingScene;

        EventManager.Get().OnFadeOutMusic += OnFadeOutMusic;
        EventManager.Get().OnStartChaseMusic += OnStartChaseMusic;
        EventManager.Get().OnContinueExploreMusic += OnContinueExploreMusic;

        EventManager.Get().OnSafeEnding += OnEndingSafe;
        EventManager.Get().OnMainDoorEnding += OnEndingMainDoor;
    }
    void OnFadeOutMusic(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StartFade(duration, 0.0f));
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
        yield break;
    }
    public IEnumerator ChaseStart(float duration) //This fnc is useful for slowly fading in / out a music track.
    {
        float currentTime = 0;
        float start = m_AudioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / duration);
            yield return null;
        }
        currentTime = 0;
        m_ExploreMusicTime = m_AudioSource.time;
        start = m_AudioSource.volume;
        m_AudioSource.clip = ChaseMusic;
        m_AudioSource.time = m_ChaseMusicTime;
        m_AudioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.3f, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public IEnumerator ContinueExploreMusic(float duration) //This fnc is useful for slowly fading in / out a music track.
    {
        float currentTime = 0;
        float start = m_AudioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / duration);
            yield return null;
        }
        currentTime = 0;
        m_ChaseMusicTime = m_AudioSource.time;
        start = m_AudioSource.volume;
        m_AudioSource.clip = ExploreMusic;
        m_AudioSource.time = m_ExploreMusicTime;
        m_AudioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.5f, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public IEnumerator TransitionToSafeEnding(float duration) //This fnc is useful for slowly fading in / out a music track.
    {
        float currentTime = 0;
        float start = m_AudioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / 1.0f);
            yield return null;
        }
        currentTime = 0;
        start = m_AudioSource.volume;
        m_AudioSource.clip = BadEnding;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.2f, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public IEnumerator TransitionToMainDoorEnding(float duration) //This fnc is useful for slowly fading in / out a music track.
    {
        float currentTime = 0;
        float start = m_AudioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / 1.0f);
            yield return null;
        }
        currentTime = 0;
        start = m_AudioSource.volume;
        m_AudioSource.loop = false;
        m_AudioSource.clip = GoodEnding;
        m_AudioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            m_AudioSource.volume = Mathf.Lerp(start, 0.2f, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    private void OnLoadingScene(Scene scene, LoadSceneMode mode)
    {
        EventManager.Get().OnFadeOutMusic += OnFadeOutMusic;
        EventManager.Get().OnStartChaseMusic += OnStartChaseMusic;
        EventManager.Get().OnContinueExploreMusic += OnContinueExploreMusic;
        EventManager.Get().OnSafeEnding += OnEndingSafe;
        EventManager.Get().OnMainDoorEnding += OnEndingMainDoor;

        if (scene.buildIndex == 0)
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = MainMenuMusic;
            m_AudioSource.volume = 0;
            StartCoroutine(StartFade(2.0f, 1.0f));
            m_AudioSource.Play();
        }
        if (scene.buildIndex == 2)
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = TutorialMusic;
            StartCoroutine(StartFade(2.0f, 0.2f));
            m_AudioSource.Play();
        }
        else if(scene.buildIndex == 3)
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = DirectorMusic;
            StartCoroutine(StartFade(2.0f, 0.5f));
            m_AudioSource.Play();
        }
        else if(scene.buildIndex == 4)
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = ExploreMusic;
            StartCoroutine(StartFade(2.0f, 0.5f));
            m_AudioSource.Play();
        }
    }
    void OnStartChaseMusic()
    {
        StopAllCoroutines();
        StartCoroutine(ChaseStart(1.0f));
    }
    void OnContinueExploreMusic()
    {
        StopAllCoroutines();
        StartCoroutine(ContinueExploreMusic(3.0f));
    }
    void OnEndingSafe()
    {
        StopAllCoroutines();
        StartCoroutine(TransitionToSafeEnding(2.0f));
    }
    void OnEndingMainDoor()
    {
        StopAllCoroutines();
        StartCoroutine(TransitionToMainDoorEnding(2.0f));
    }
}
