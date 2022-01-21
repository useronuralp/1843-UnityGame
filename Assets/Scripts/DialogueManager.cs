using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    private static DialogueManager               s_Instance;

    private GameObject                           m_CurrentDialogueBubble;
    private Queue<string>                        m_SentenceQueue;
    private bool                                 m_IsTypingSentence;
    private string                               m_CurrentTypedSentence;
    private Queue<GameObject>                    m_CurrentDialogueQueue;
    public bool                                  m_IsPlayerInDialogue;
    private bool                                 m_IsEndOfLevel;
    //Camera switches---------------------
    private Cinemachine.CinemachineVirtualCamera m_CameraFrom;
    private Cinemachine.CinemachineVirtualCamera m_CameraTo;
    private void Awake()
    {
        s_Instance = this;
    }
    private void Start()
    {
        m_IsEndOfLevel                      = false;

        m_IsPlayerInDialogue                = false;
        m_CurrentTypedSentence              = "None";
        m_IsTypingSentence                  = false;
        EventManager.Get().OnStartDialogue += OnStartDialogue;
        m_SentenceQueue                     = new Queue<string>();
        m_CameraFrom                        = GameManager.Get().GetCamera("Player");
    }
    private void Update()
    {
        if (m_IsPlayerInDialogue)
        {
            if (Input.GetMouseButtonUp(0))
            {
                DisplayNextSentence();
            }
        }
    }
    void OnStartDialogue(Queue<GameObject> dialogueQueue, bool isEndOfLevel)
    {
        
        m_IsEndOfLevel = isEndOfLevel;
        EventManager.Get().DisableMouseInputs();
        EventManager.Get().DisableKeyboardInputs();
        EventManager.Get().StartedDialogue();
        EventManager.Get().PauseCandleCounter();
        EventManager.Get().DisableSanityBar();
        m_CurrentDialogueQueue = dialogueQueue; //Entire structure.
        m_CurrentDialogueBubble = m_CurrentDialogueQueue.Dequeue();
        m_CurrentDialogueBubble.SetActive(true);
        m_IsPlayerInDialogue = true;
        m_SentenceQueue.Clear();
        m_CameraTo = m_CurrentDialogueBubble.GetComponent<Dialogue>().Camera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        GameManager.Get().SwitchCameras(m_CameraFrom, m_CameraTo);
        m_CameraFrom = m_CameraTo;
        foreach (string sentence in m_CurrentDialogueBubble.GetComponent<Dialogue>().Sentences)
        {
            m_SentenceQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    void MoveToNextBubble()
    {
        m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
        m_CurrentDialogueBubble.SetActive(false);
        m_CurrentDialogueBubble = m_CurrentDialogueQueue.Dequeue();
        m_CurrentDialogueBubble.SetActive(true);
        m_SentenceQueue.Clear();
        m_CameraTo = m_CurrentDialogueBubble.GetComponent<Dialogue>().Camera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        GameManager.Get().SwitchCameras(m_CameraFrom, m_CameraTo);
        m_CameraFrom = m_CameraTo;
        foreach (string sentence in m_CurrentDialogueBubble.GetComponent<Dialogue>().Sentences)
        {
            m_SentenceQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (m_SentenceQueue.Count == 0 && !m_IsTypingSentence)
        {
            if(m_CurrentDialogueQueue.Count > 0)
            {
                MoveToNextBubble();
                return;
            }
            else
            {
                if(GameState.IsCaught)
                {
                    switch(Random.Range(0,4))
                    {
                        case 0: EventManager.Get().GameOver("You have been caught", "Tip: When you break line of sight, guards will investigate your last seen location."); break;
                        case 1: EventManager.Get().GameOver("You have been caught", "Tip: You can bait the guards out of a room if you happen to have business there."); break;
                        case 2: EventManager.Get().GameOver("You have been caught", "Tip: Candle light will compromise your location to the guards, use it wisely."); break;
                        case 3: EventManager.Get().GameOver("You have been caught", "Tip: You can hide behind wide objects like doors, curtains or walls."); break;
                    }

                    m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
                    m_CurrentDialogueBubble.SetActive(false);
                    m_IsPlayerInDialogue = false;
                    GameState.IsCaught = false;
                    return;
                }
                if(m_IsEndOfLevel)
                {
                    //The case where the game loads another scene right after the dialogue ends.
                    EventManager.Get().FadeOutMusic(2.0f);
                    GameObject.Find("FadeIn/Out").transform.Find("Canvas").Find("BlackScreen").GetComponent<Animator>().SetTrigger("FadeEndOfScene");
                    m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
                    m_CurrentDialogueBubble.SetActive(false);
                    m_IsPlayerInDialogue = false;
                }
                else
                {
                    if(m_CurrentDialogueBubble.name == "Player_Self1")
                    {
                        EventManager.Get().DisplayLMBTutorial();
                    }
                    else if(m_CurrentDialogueBubble.name == "Player_Self_Explorable")
                    {
                        //EventManager.Get().UpdateTask("- Find a way out of the prison cell.");
                        //EventManager.Get().UpdateTask("- Search for clues.");
                        //EventManager.Get().UpdateThought("That was hell of a ride. There is something seriously wrong with this place. Better look around...");
                        EventManager.Get().DisplaySanityTutorial();
                    }
                    EventManager.Get().EnableMouseInputs();
                    EventManager.Get().EnableKeyboardInputs();
                    EventManager.Get().EndedDialogue();
                    EventManager.Get().ContinueCandleCounter();
                    if(!GameState.HasSeenLevelThreeBefore)
                    {
                        //EventManager.Get().EnableSanityBar();
                    }
                    if (SceneManager.GetActiveScene().buildIndex == 4)
                    {
                        EventManager.Get().StartWanderingGuard();
                        EventManager.Get().SetupLevelThree(); //Setups the journal journal item shirt and enables the UI upon loading the explorable area.
                    }
                    m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
                    m_CurrentDialogueBubble.SetActive(false);
                    m_IsPlayerInDialogue = false;
                    GameManager.Get().SwitchToPlayerCamera();
                    m_CameraFrom = GameManager.Get().GetCamera("Player"); //Reset the "from" camera to player camera. After every dialogue. The camera focuses back to the player.?
                }
                return;
            }
        }
        if (m_IsTypingSentence)
        {
            StopAllCoroutines();
            m_IsTypingSentence = false;
            m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>().text = m_CurrentTypedSentence;
            m_CurrentTypedSentence = "None";
            m_CurrentDialogueBubble.transform.Find("SkipImage").gameObject.SetActive(true);
        }
        else
        {
            string sentence = m_SentenceQueue.Dequeue();
            StartCoroutine(TypeSentenceDialogueBox(sentence));
        }
    }
    IEnumerator TypeSentenceDialogueBox(string sentence)
    {
        m_CurrentTypedSentence = sentence;
        m_CurrentDialogueBubble.transform.Find("SkipImage").gameObject.SetActive(false);
        TextMeshProUGUI textField = m_CurrentDialogueBubble.transform.Find("Box").Find("Text").GetComponent<TextMeshProUGUI>();
        textField.text = "";
        m_IsTypingSentence = true;
        foreach (char letter in sentence.ToCharArray())
        {
            textField.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        m_IsTypingSentence = false;
        m_CurrentDialogueBubble.transform.Find("SkipImage").gameObject.SetActive(true);
        m_CurrentTypedSentence = "None";
    }
    public static DialogueManager Get()
    {
        return s_Instance;
    }
}
