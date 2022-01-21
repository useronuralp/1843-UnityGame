using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    private AudioClip m_ButtonSound;
    private AudioClip m_ButtonHoverSound;
    private AudioSource m_AudioSource;
    private void Awake()
    {
        if(PlayerPrefs.GetFloat("CurrentLevel") == 0)
        {
            transform.parent.Find("Continue").GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.parent.Find("Continue").GetComponent<Button>().interactable = true;
        }
    }
    private Animator m_Animator;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_ButtonSound = Resources.Load<AudioClip>("Sound/HoverSound");
        m_ButtonHoverSound = Resources.Load<AudioClip>("Sound/ButtonClick");
        m_Animator = GetComponent<Animator>();
    }
    public void LoadNewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void OnNewGameButtonPressed()
    {
        m_AudioSource.PlayOneShot(m_ButtonSound, 0.5f);
        PlayerPrefs.SetFloat("CurrentLevel", 0);
        m_Animator.SetTrigger("FadeOut");
        GameState.HasSeenLevelThreeBefore = false;
    }
    public void OnContinueButtonPressed()
    {
        m_AudioSource.PlayOneShot(m_ButtonSound, 0.5f);
        EventManager.Get().FadeOutMusic(4.0f);
        m_Animator.SetTrigger("FadeOutContinue");
    }
    public void OnQuitButtonPressed()
    {
        m_AudioSource.PlayOneShot(m_ButtonSound, 0.5f);
        Application.Quit();
    }
    public void Continue()
    {
        if(PlayerPrefs.GetFloat("CurrentLevel") != 0)
        {
            SceneManager.LoadScene((int)PlayerPrefs.GetFloat("CurrentLevel"), LoadSceneMode.Single);
        }
    }
    public void PlayHoverSound(GameObject button)
    {
        if(button.transform.GetComponent<Button>().interactable)
            m_AudioSource.PlayOneShot(m_ButtonHoverSound, 0.2f);
    }
}
