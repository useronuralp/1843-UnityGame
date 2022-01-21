using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class ScreenTransitions : MonoBehaviour
{
    private static Animator m_Animator;
    private static GameObject m_CurrentCutScene;
    private static bool m_ShouldEnable;
    private void Start()
    {
        m_ShouldEnable = true;
        m_CurrentCutScene = null;
    }
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    public static void FadeIntoCutScene()
    {
        m_Animator.SetTrigger("FadeInShort");
    }
    public static void FadeOutToCutscene(GameObject cutScene, bool shouldEnable = true)
    {
        m_ShouldEnable = shouldEnable;
        m_CurrentCutScene = cutScene;
        m_Animator.SetTrigger("FadeOutShort");
    }
    public static void FadeOutLong()
    {
        m_Animator.SetTrigger("FadeOutLong");
    }
    public void DisableMouseInputs()
    {
        EventManager.Get().DisableMouseInputs();
    }
    public void EnableMouseInputs()
    {
        EventManager.Get().EnableMouseInputs();
    }
    public void StartIntroDialogue()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            EventManager.Get().StartDialogue(new Queue<GameObject> (new GameObject[] {DialogueDatabase.Get().LV0_PlayerSelf})); //Create the queue.
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV1_Player1, DialogueDatabase.Get().LV1_Director1, DialogueDatabase.Get().LV1_Player2, DialogueDatabase.Get().LV1_Director2, DialogueDatabase.Get().LV1_Player3, DialogueDatabase.Get().LV1_Director3 , DialogueDatabase.Get().LV1_Player4, DialogueDatabase.Get().LV1_Director4 , DialogueDatabase.Get().LV1_Player5, DialogueDatabase.Get().LV1_Director5} ), true); //Create the queue.
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if(!GameState.HasSeenLevelThreeBefore) //This level is launched for the first time ever.
            {
                GameState.HasSeenLevelThreeBefore = true;
                EventManager.Get().StartDialogue(new Queue<GameObject>(new GameObject[] { DialogueDatabase.Get().LV3_PlayerSelf_Explorable })); //Create the queue.
            }
            else
            {
                EventManager.Get().EnableKeyboardInputs();
                EventManager.Get().EnableMouseInputs();
                EventManager.Get().SetupLevelThree();
                EventManager.Get().EnableJournalButton();
                EventManager.Get().EndedDialogue();
                EventManager.Get().StartWanderingGuard();
                EventManager.Get().EnableSanityBar();
            }
        }
    }
    public void ActivateOrDeactivateCutScene() //This is being used to activate / deactivate cutscenes during the transition animaitons.
    {
        if(m_ShouldEnable)
        {
            m_CurrentCutScene.SetActive(true);
            EventManager.Get().DisableKeyboardInputs();
        }
        else
        {
            m_CurrentCutScene.SetActive(false);
            m_CurrentCutScene = null;
            EventManager.Get().EnableKeyboardInputs();
        }
    }
    public void LoadNextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
    public void ReloadLevel()
    {
        EventManager.Get().CloseGameOver();
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
