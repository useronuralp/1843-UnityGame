using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class JournalManager : MonoBehaviour
{

    public Sprite TaskBarNormal;
    public Sprite TaskBarHover;
    public Sprite CluesBarNormal;
    public Sprite CluesBarHover;
    public Sprite SettingsBarNormal;
    public Sprite SettingsBarHover;

    [SerializeField]
    private GameObject[] ClueImages;

    public AudioMixer AudioMixer;
    [SerializeField]
    private Sprite[] ClueImagesBig;

    private AudioClip m_JournalPage1;
    private AudioClip m_JournalPage2;
    private AudioClip m_ButtonClick;
    private AudioSource m_AudioSource;

    private GameObject[] m_Tabs;
    private GameObject m_JournalCanvas;
    private GameObject m_ItemPreviewCanvas;
    private GameObject m_JournalToggleButtonCanvas;
    private GameObject m_CandleCanvas;

    private GameObject m_ExclamationMark;

    private bool m_IsFirstUse;
    private bool m_IsJournalOpen;
    private bool m_IsInDialogue;
    private bool m_ClickedCluesButton;
    //Clue item exclamation marks
    private GameObject m_ShirtMark;
    private GameObject m_FileMark;
    private GameObject m_MedicineMark;
    private GameObject m_OldDocuments_Mark;
    private GameObject m_NewDocuments_Mark;
    private GameObject m_PhotographMark;
    private GameObject m_WeddingPictureMark;
    private GameObject m_SyringeMark;
    private GameObject m_PocketwatchMark;
    private GameObject m_NewspaperMark;
    private GameObject m_LetterMark;

    private GameObject m_SanityBarCanvas;

    private bool m_EnableJournalButton;
    private bool m_IsInTutorial;

    private TextMeshProUGUI m_ThoughtText;
    private int m_CurrentTaskNumber;

    private TextMeshProUGUI[] m_Tasks;

    private Slider m_BackgroundSoundSlider;
    private string m_ActiveTab;

    private GameObject m_TaskButton;
    private GameObject m_CluesButton;
    private GameObject m_SettingsButton;
    enum JournalTabs
    {
        Tasks,
        Items,
        Clues,
        Settings
    }
    enum Clues
    {
        Shirt,
        File,
        Medicine,
        StaffList_1,
        StaffList_2,
        Photograph,
        Photograph_2,
        Syringe,
        Pocketwatch,
        Newspaper,
        Letter,
    }
    private void Start()
    {
        m_Tasks = new TextMeshProUGUI[5];

        m_Tasks[0] = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("ActiveTask").GetComponent<TextMeshProUGUI>();
        m_Tasks[1] = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("Task2").GetComponent<TextMeshProUGUI>();
        m_Tasks[2] = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("Task3").GetComponent<TextMeshProUGUI>();
        m_Tasks[3] = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("Task4").GetComponent<TextMeshProUGUI>();
        m_Tasks[4] = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("Task5").GetComponent<TextMeshProUGUI>();

        m_AudioSource = GetComponent<AudioSource>();
        m_JournalPage1 = Resources.Load<AudioClip>("Sound/JournalPage");
        m_JournalPage2 = Resources.Load<AudioClip>("Sound/JournalPage2");
        m_ButtonClick = Resources.Load<AudioClip>("Sound/ButtonClick");
        m_ActiveTab = "Tasks";


        m_ThoughtText = transform.Find("JournalCanvas").Find("Background").Find("TasksTab").Find("Thought").Find("Text").GetComponent<TextMeshProUGUI>();
        m_CurrentTaskNumber = 1;



        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            m_IsInTutorial = true;
        }
        else
            m_IsInTutorial = false;

        m_EnableJournalButton = true;
        m_ClickedCluesButton = false;
        m_IsFirstUse = true;
        m_IsJournalOpen = false;
        m_IsInDialogue = true;
        m_Tabs = new GameObject[4];
        m_JournalCanvas = transform.Find("JournalCanvas").gameObject;
        m_SanityBarCanvas = transform.Find("UI").Find("SanityBarCanvas").gameObject;
        m_ItemPreviewCanvas = transform.Find("ItemPreviewCanvas").gameObject;
        m_JournalToggleButtonCanvas = transform.Find("UI").Find("JournalToggleButtonCanvas").gameObject;
        m_CandleCanvas = transform.Find("UI").Find("CandleCanvas").gameObject;
        m_Tabs[(int)JournalTabs.Tasks] = m_JournalCanvas.transform.Find("Background").Find("TasksTab").gameObject;
        m_Tabs[(int)JournalTabs.Items] = m_JournalCanvas.transform.Find("Background").Find("ItemDescriptionTab").gameObject;
        m_Tabs[(int)JournalTabs.Clues] = m_JournalCanvas.transform.Find("Background").Find("CluesTab").gameObject;
        m_Tabs[(int)JournalTabs.Settings] = m_JournalCanvas.transform.Find("Background").Find("SettingsTab").gameObject;

        m_TaskButton = m_JournalCanvas.transform.Find("Background").Find("Buttons").Find("Tasks").gameObject;
        m_CluesButton = m_JournalCanvas.transform.Find("Background").Find("Buttons").Find("Clues").gameObject;
        m_SettingsButton = m_JournalCanvas.transform.Find("Background").Find("Buttons").Find("Settings").gameObject;
        m_TaskButton.GetComponent<Image>().sprite = TaskBarHover;
        m_CluesButton.GetComponent<Image>().sprite = CluesBarNormal;
        m_SettingsButton.GetComponent<Image>().sprite = SettingsBarNormal;

        m_BackgroundSoundSlider = m_JournalCanvas.transform.Find("Background").Find("SettingsTab").Find("SoundSlider").GetComponent<Slider>();
        m_BackgroundSoundSlider.value = GameState.Volume;
        //Clue pick up events will go into here. When picking up a clue, It's image will be shown in the journal and it's description will be added to the "items" tab.
        EventManager.Get().OnClickingShirt += OnClickingShirt;
        EventManager.Get().OnClickingFile += OnClickingFile;
        EventManager.Get().OnClickingMedicine += OnClickingMedicine; 
        EventManager.Get().OnClickingStaffList_1 += OnClickingStaffList_1;
        EventManager.Get().OnClickingStaffList_2 += OnClickingStaffList_2;
        EventManager.Get().OnClickingPhotograph += OnClickingPhotograph;
        EventManager.Get().OnClickingPhotograph_2 += OnClickingPhotograph_2;
        EventManager.Get().OnClickingSyringe += OnClickingSyringe;
        EventManager.Get().OnClickingPocketwatch += OnClickingPocketwatch;
        EventManager.Get().OnClickingNewspaper += OnClickingNewspaper;
        EventManager.Get().OnClickingLetter += OnClickingLetter;

        //These events are here for when an NPC talks to the player while they are navigating the journal. We want to close the journal then.
        EventManager.Get().OnStartingDialogue += OnStartingDialogue;
        EventManager.Get().OnEndingDialogue += OnEndingDialogue;
        EventManager.Get().OnPlayerPicksUpAClue += OnPickingUpClue;

        //Clues----------
        EventManager.Get().OnPickingUpShirt += OnPickingUpShirt;
        EventManager.Get().OnPickingUpFile += OnPickingUpFile;
        EventManager.Get().OnPickingUpMedicine += OnPickingUpMedicine;
        EventManager.Get().OnPickingUpStaffList_1 += OnPickingUpStaffList_1;
        EventManager.Get().OnPickingUpStaffList_2 += OnPickingUpStaffList_2;
        EventManager.Get().OnPickingUpPhotograph += OnPickingUpPhotograph;
        EventManager.Get().OnPickingUpPhotograph_2 += OnPickingUpPhotograph_2;
        EventManager.Get().OnPickingUpSyringe += OnPickingUpSyringe;
        EventManager.Get().OnPickingUpPocketwatch += OnPickingUpPocketwatch;
        EventManager.Get().OnPickingUpNewspaper += OnPickingUpNewspaper;
        EventManager.Get().OnPickingUpLetter += OnPickingUpLetter;

        EventManager.Get().OnEnablingJournalButton += OnEnablingJournalButton;
        EventManager.Get().OnDisablingJournalButton += OnDisablingJournalButton;

        EventManager.Get().OnLoadingLevel_Three += OnLoadingLevelThree;

        EventManager.Get().OnUpdateTask += OnUpdateTask;
        EventManager.Get().OnUpdateThought += OnUpdateThought;

        m_ExclamationMark = transform.Find("UI").Find("JournalToggleButtonCanvas").Find("JournalToggleButton").Find("ExclamationMark").gameObject;

        //Clues marks
        m_ShirtMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Shirt").Find("ExclamationMark").gameObject;
        m_FileMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("File").Find("ExclamationMark").gameObject;
        m_MedicineMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Medicine").Find("ExclamationMark").gameObject;
        m_OldDocuments_Mark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Old Documents").Find("ExclamationMark").gameObject;
        m_NewDocuments_Mark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("New Documents").Find("ExclamationMark").gameObject;
        m_WeddingPictureMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Wedding Picture").Find("ExclamationMark").gameObject;
        m_PhotographMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Photograph").Find("ExclamationMark").gameObject;
        m_SyringeMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Syringe").Find("ExclamationMark").gameObject;
        m_PocketwatchMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Pocket Watch").Find("ExclamationMark").gameObject;
        m_NewspaperMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Newspaper").Find("ExclamationMark").gameObject;
        m_LetterMark = m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Letter").Find("ExclamationMark").gameObject;


        m_Tabs[(int)JournalTabs.Tasks].SetActive(true);

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnUpdateTask("test");
        }
        if (!GameManager.Get().IsMouseOverUI() && m_IsJournalOpen && Input.GetMouseButtonUp(0))
        {
            if (GameManager.Get().AreMouseInputsEnabled && !m_IsInDialogue)
            {
                if (m_IsJournalOpen)
                {
                    if (m_IsFirstUse)
                    {
                        m_IsFirstUse = false;
                        EventManager.Get().ClosedJournal();
                    }
                    if(m_ClickedCluesButton)
                    {
                        DisableAllExclamationMarks();
                        m_ClickedCluesButton = false;
                    }
                    m_JournalCanvas.SetActive(false);
                    EventManager.Get().EnableCandleUse();
                    EventManager.Get().EnableUI();
                    EventManager.Get().ContinueCandleCounter();
                    EventManager.Get().EnableSanityBar();
                    m_CandleCanvas.SetActive(true);
                    if(!m_IsInTutorial)
                        m_SanityBarCanvas.SetActive(true);
                }
                m_IsJournalOpen = !m_IsJournalOpen;
            }
        }
    }
    void OnLoadingLevelThree()
    {
        //Shirt is alreayd looted in the previous level.
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Shirt").GetComponent<Button>().interactable = true;
        if(!GameState.HasSeenLevelThreeBefore)
        {
            //EventManager.Get().EnableSanityBar();
        }
        m_SanityBarCanvas.SetActive(true);
        m_JournalToggleButtonCanvas.SetActive(true);
        m_CandleCanvas.SetActive(true);
    }
    public void OnCluesButtonPressed()
    {
        DisableAllTabs();
        if (m_ActiveTab != "Clues")
        {
            switch(Random.Range(0,2))
            {
                case 0: m_AudioSource.PlayOneShot(m_JournalPage1); break;
                case 1: m_AudioSource.PlayOneShot(m_JournalPage2); break;
            }
        }
            
        if (m_ExclamationMark.activeInHierarchy)
        {
            m_ExclamationMark.SetActive(false);
            m_ClickedCluesButton = true;
        }
        m_ActiveTab = "Clues";
        m_Tabs[(int)JournalTabs.Clues].SetActive(true);
        m_CluesButton.GetComponent<Image>().sprite = CluesBarHover;
        m_SettingsButton.GetComponent<Image>().sprite = SettingsBarNormal;
        m_TaskButton.GetComponent<Image>().sprite = TaskBarNormal;
    }
    public void OnSettingsButtonPressed()
    {
        DisableAllTabs();
        if (m_ActiveTab != "Settings")
        {
            switch (Random.Range(0, 2))
            {
                case 0: m_AudioSource.PlayOneShot(m_JournalPage1); break;
                case 1: m_AudioSource.PlayOneShot(m_JournalPage2); break;
            }
        }
        m_ActiveTab = "Settings";
        m_Tabs[(int)JournalTabs.Settings].SetActive(true);
        m_SettingsButton.GetComponent<Image>().sprite = SettingsBarHover;
        m_CluesButton.GetComponent<Image>().sprite = CluesBarNormal;
        m_TaskButton.GetComponent<Image>().sprite = TaskBarNormal;
    }
    public void OnTasksButtonPressed()
    {
        DisableAllTabs();
        if (m_ActiveTab != "Tasks")
        {
            switch (Random.Range(0, 2))
            {
                case 0: m_AudioSource.PlayOneShot(m_JournalPage1); break;
                case 1: m_AudioSource.PlayOneShot(m_JournalPage2); break;
            }
        }
        m_Tabs[(int)JournalTabs.Tasks].SetActive(true);
        m_ActiveTab = "Tasks";
        m_TaskButton.GetComponent<Image>().sprite = TaskBarHover;
        m_CluesButton.GetComponent<Image>().sprite = CluesBarNormal;
        m_SettingsButton.GetComponent<Image>().sprite = SettingsBarNormal;
    }
    void DisableAllTabs()
    {
        foreach(var gameObject in m_Tabs)
            if(gameObject) gameObject.SetActive(false);
    }
    public void OnJournalToggleButtonPressed()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick);
        if(m_EnableJournalButton)
        {
            if (GameManager.Get().AreMouseInputsEnabled && !m_IsInDialogue)
            {
                if (m_IsJournalOpen)
                {
                    if(m_IsFirstUse)
                    {
                        m_IsFirstUse = false;
                        EventManager.Get().ClosedJournal();
                    }
                    if (m_ClickedCluesButton)
                    {
                        DisableAllExclamationMarks();
                        m_ClickedCluesButton = false;
                    }
                    m_JournalCanvas.SetActive(false);
                    EventManager.Get().EnableCandleUse();
                    EventManager.Get().EnableUI();
                    EventManager.Get().ContinueCandleCounter();
                    EventManager.Get().EnableSanityBar();
                    m_CandleCanvas.SetActive(true);
                    if (!m_IsInTutorial)
                        m_SanityBarCanvas.SetActive(true);
                }
                else
                {
                    if (m_ActiveTab == "Clues" && m_ExclamationMark.activeInHierarchy)
                    {
                        m_ExclamationMark.SetActive(false);
                        m_ClickedCluesButton = true;
                    }
                    m_JournalCanvas.SetActive(true);
                    EventManager.Get().DisableCandleUse();
                    EventManager.Get().PauseCandleCounter();
                    EventManager.Get().DisableSanityBar();
                    m_CandleCanvas.SetActive(false);
                    if(!m_IsInTutorial)
                        m_SanityBarCanvas.SetActive(false);
                }
                m_IsJournalOpen = !m_IsJournalOpen;
            }
        }
    }
    void DisableAllExclamationMarks()
    {
        //Add new clues here.
        m_ShirtMark.SetActive(false);
        m_FileMark.SetActive(false);
        m_MedicineMark.SetActive(false);
        m_OldDocuments_Mark.SetActive(false);
        m_NewDocuments_Mark.SetActive(false);
        m_PhotographMark.SetActive(false);
        m_WeddingPictureMark.SetActive(false);
        m_SyringeMark.SetActive(false);
        m_PocketwatchMark.SetActive(false);
        m_LetterMark.SetActive(false);
        m_NewspaperMark.SetActive(false);
    }
    void OnPickingUpLetter()
    {
        m_LetterMark.SetActive(true);
    }
    void OnPickingUpNewspaper()
    {
        m_NewspaperMark.SetActive(true);
    }
    void OnPickingUpPocketwatch()
    {
        m_PocketwatchMark.SetActive(true);
    }
    void OnPickingUpSyringe()
    {
        m_SyringeMark.SetActive(true);
    }
    void OnPickingUpPhotograph()
    {
        m_PhotographMark.SetActive(true);
    }
    void OnPickingUpPhotograph_2()
    {
        m_WeddingPictureMark.SetActive(true);
    }
    void OnPickingUpStaffList_1()
    {
        m_OldDocuments_Mark.SetActive(true);
    }
    void OnPickingUpStaffList_2()
    {
        m_NewDocuments_Mark.SetActive(true);
    }
    void OnPickingUpMedicine()
    {
        m_MedicineMark.SetActive(true);
    }
    void OnPickingUpFile()
    {
        m_FileMark.SetActive(true);
    }
    void OnPickingUpShirt()
    {
        m_ShirtMark.SetActive(true);
    }
    void OnPickingUpClue()
    {
        m_ExclamationMark.SetActive(true);
    }
    void OnClickingNewspaper()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Newspaper").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var newspaperPreview = Instantiate(ClueImages[(int)Clues.Newspaper], new Vector3(0, 0, 0), Quaternion.identity);
        newspaperPreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);

    }
    void OnClickingLetter()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Letter").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var letterPreview = Instantiate(ClueImages[(int)Clues.Letter], new Vector3(0, 0, 0), Quaternion.identity);
        letterPreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingPocketwatch()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Pocket Watch").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var pocketwatchPreview = Instantiate(ClueImages[(int)Clues.Pocketwatch], new Vector3(0, 0, 0), Quaternion.identity);
        pocketwatchPreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingSyringe()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Syringe").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var syringePreview = Instantiate(ClueImages[(int)Clues.Syringe], new Vector3(0, 0, 0), Quaternion.identity);
        syringePreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingShirt()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Shirt").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var shirtPreview = Instantiate(ClueImages[(int)Clues.Shirt], new Vector3(0, 0, 0), Quaternion.identity);
        shirtPreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);

    }
    void OnClickingPhotograph_2()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Wedding Picture").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var photograph_2_Preview = Instantiate(ClueImages[(int)Clues.Photograph_2], new Vector3(0, 0, 0), Quaternion.identity);
        photograph_2_Preview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingStaffList_1()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Old Documents").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var staffList_1_Preview = Instantiate(ClueImages[(int)Clues.StaffList_1], new Vector3(0, 0, 0), Quaternion.identity);
        staffList_1_Preview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingStaffList_2()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("New Documents").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var staffList_2_Preview = Instantiate(ClueImages[(int)Clues.StaffList_2], new Vector3(0, 0, 0), Quaternion.identity);
        staffList_2_Preview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingFile()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("File").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var filePreview = Instantiate(ClueImages[(int)Clues.File], new Vector3(0, 0, 0), Quaternion.identity);
        filePreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);

    }
    void OnClickingPhotograph()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Photograph").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var photographPreview = Instantiate(ClueImages[(int)Clues.Photograph], new Vector3(0, 0, 0), Quaternion.identity);
        photographPreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnClickingMedicine()
    {
        m_JournalCanvas.transform.Find("Background").Find("CluesTab").Find("Medicine").GetComponent<Button>().interactable = true;

        //Upon creation of this object, it's animator will take care of the events that need to be fired.
        var medicinePreview = Instantiate(ClueImages[(int)Clues.Medicine], new Vector3(0, 0, 0), Quaternion.identity);
        medicinePreview.transform.SetParent(m_ItemPreviewCanvas.transform, false);
    }
    void OnStartingDialogue()
    {
        m_IsInDialogue = true;
        if (m_IsJournalOpen)
        {
            m_JournalCanvas.SetActive(false);
            m_IsJournalOpen = !m_IsJournalOpen;
        }
    }
    void OnEndingDialogue()
    {
        m_IsInDialogue = false;
    }

    public void OnBackButtonClicked()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick);
        m_Tabs[(int)JournalTabs.Items].SetActive(false);
    }

    //Button event.
    public void OnClueClicked(string clueName)
    {
        string itemName = "None";
        string itemDescription = "None";
        Sprite itemImage = ClueImagesBig[(int)Clues.Shirt];
        m_AudioSource.PlayOneShot(m_ButtonClick);
        switch (clueName)
        {
            case "Shirt":
                itemName = "A Shirt";
                itemDescription = @"It looks like a very normal shirt, and it seems to have been here for a while.

When I opened my eyes, the only thing around me was this one shirt. They took everything that had to do with me. As I tried to put the shirt on, a note fell out sticked inside a small journal. It reads, 'You shouldn't have come back!' The words sent shivers down my spine. Because I had no memory of ever being here. And why was there someone here who knew me? What was the reason he didn't want me here?
";
                itemImage = ClueImagesBig[(int)Clues.Shirt];
                break;
            case "File":
                itemName = "A File";
                itemDescription = @"There is some basic information about myself on this file. It includes age, name, experience, etc. Compared to the files of the other prisoners next to it, this one looks very new.

It says I have pharmaceutical experience, but I don't remember anything. This is very different from the ordinary profile, especially for a criminal. The profile is surprisingly simple, not even any criminal record. I don't understand why pharmaceutical experience is such an important thing to look for in a prisoner. I peeked at the file next to me. I could almost conclude that my file had been purposely saved.
";
                itemImage = ClueImagesBig[(int)Clues.File];
                break;
            case "Medicine":
                itemName = "A Medicine Bottle";
                itemDescription = @"There are no instructions on the medicine bottles. The pharmaceutical process is so confidential that it is generally not possible to take the bottles out of the laboratory. The medicine bottle is blank, but there is a label attached to the bottom of the bottle stating that the date of manufacture was 1863 for the 2nd batch.

When producing medicines, each person only has access to part of the process, as the doctor said. In my opinion, this bottle of medicine will be the key to getting me to the truth about this prison. The only number that appears on this medicine bottle could be a hint of some sort. This should be a new medicine, it states that the D-2nd batch was produced this year
";
                itemImage = ClueImagesBig[(int)Clues.Medicine];
                break;
            case "StaffList_1":
                itemName = "Old Documents";
                itemDescription = @"The list of prison staff contains basic information about each employee, including their position and year of service. This list is compiled every twenty years. The one from twenty years ago is incomplete and some information is no longer recognizable.

Even though the handwriting is very faint, I can vaguely make out some valuable information. It looks like the current director of the prison was still the deputy director at the time. The name of the man who created the prison is visible, and it seems to be Karlheinz, the same person as the statue of imprisonment. The names of the doctor and the prison director are on the list of key personnel. Beyond that, they are no longer recognisable due to wear and tear. This list from twenty years ago has a footnote of S-5, which would be the fifth generation of staff. Does the 'S' stand for the safe?
";
                itemImage = ClueImagesBig[(int)Clues.StaffList_1];
                break;
            case "StaffList_2":
                itemName = "New Documents";
                itemDescription = @"This current list is clear. This list provides a detailed record of the current prison staff. This list is supposed to be classified.

Besides the names of the doctor and the prison director, Starrick's name on the list interested me more.
Why would a person who has never been in prison appear on this list? The list would only include people who are still associated with the hospital. Judging by the brand new list, this person must still contact the prison. This may be the breakthrough.
This list of staff has a footnote of D-6, which means that the current group of staff is the sixth generation. Maybe the 'D' stands for the door exit?
";
                itemImage = ClueImagesBig[(int)Clues.StaffList_2];
                break;
            case "Photograph":
                itemName = "Group Photograph";
                itemDescription = @"The group photo is placed in a very prominent position. There are four people in the photo, and the prison is in the background. The man second to right is very well dressed, and the stopwatch around his neck shows that he must have come from a prominent family. The man on the far left is wearing doctor's uniform. He is probably the head doctor of the prison. Next to him is a young man in formal wear. His hand is on the doctor's shoulder.

Although the photo appears to be quite old, the picture frame is spotless. It looks like someone has been cleaning it regularly. I guess the second person from right would be the director of this prison. Although the photo is from a long time ago, the prison director doesn't seem to have changed much. All other information that could be gathered was that it looked like the young man was close to the doctor.
";
                itemImage = ClueImagesBig[(int)Clues.Photograph];
                break;
            case "Photograph_2":
                itemName = "Wedding Picture";
                itemDescription = @"This is a wedding photo. The man has his arm around the woman, and they seem to be very much in love.

I feel very confused when I look closer at this photo. Isn't this man the same in the group photo? I remember four people in the group photo, and he seemed to be the young man who was very close to the doctor. When I looked at this photo over and over again, I couldn't help but feel a sad little inside. This strange feeling is hard to describe.Maybe I've seen this photo somewhere or seen the person in it.
";
                itemImage = ClueImagesBig[(int)Clues.Photograph_2];
                break;
            case "Syringe":
                itemName = "A Syringe";
                itemDescription = @"An extremely rare syringe that is neither used as a treatment nor in the production of medicines.

When I saw this, I couldn't help but question the doctor. Why is it in the doctor's room? But on second thought, I should be able to trust the doctor. If he had tried to kill me, I would have been dead several times by now.
";
                itemImage = ClueImagesBig[(int)Clues.Syringe];
                break;
            case "Pocketwatch":
                itemName = "Pocket Watch";
                itemDescription = @"Bespoke stopwatch, with the name of the Starrick family and the date of making in 1843 engraved on the back. Starrick Hospital was once the largest private hospital. There were rumours that they had had a significant research and development accident. People had taken to the streets because of it, and their manager at the time eventually apologized in public. 

After taking a closer look at the stopwatch, I noticed how gorgeous it looks, even after all these years. Its owner must be a member of some prominent family. I noticed a number S-3 on the side of the stopwatch, symbolizing that the watch belonged to the third generation of Starrick's heirs. Perhaps the 'S' stands for the safe?
";
                itemImage = ClueImagesBig[(int)Clues.Pocketwatch];
                break;
            case "Letter":
                itemName = "A Letter";
                itemDescription = @"The letter has been well kept. Wax Sealing is clearly visible on the letter. The seal belongs to this prison, which means that the letter has a solid connection to this prison. Even if well preserved, the letter is easy to see that it has received the baptism of age.

''Dear Karlheinz, How have you been?
Here's the thing, I really don't want you to be involved in any more experiments with the three of you. Why try to save us by hurting others?
I really couldn't stand it when you were doing this again. Just come home, please.''

As I read the letter, I was completely shocked by the words. Too many questions suddenly appeared in my head.Who is Karlheinz? What kind of experiments are they actually doing?
";
                itemImage = ClueImagesBig[(int)Clues.Letter];
                break;
            case "Newspaper":
                itemName = "A Newspaper";
                itemDescription = @"The newspaper's front-page title reads, ''Plutocrat's daughter suffers from a rare mental illness. How can we avoid getting this disease?'' The date of production of the newspaper is 1843.

This year is 1863, and twenty years have passed since the newspaper was published. Why would anyone care so much about an old newspaper that they would keep it for 20 years?
The years 1843-S and 1863-D seem destined to be crucial for this prison.
";
                itemImage = ClueImagesBig[(int)Clues.Newspaper];
                break;
        }
        m_Tabs[(int)JournalTabs.Items].SetActive(true);
        m_Tabs[(int)JournalTabs.Items].transform.Find("ItemImage").GetComponent<Image>().sprite = itemImage;
        m_Tabs[(int)JournalTabs.Items].transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
        m_Tabs[(int)JournalTabs.Items].transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = itemDescription;
    }
    void OnEnablingJournalButton()
    {
        m_EnableJournalButton = true;
    }
    void OnDisablingJournalButton()
    {
        m_EnableJournalButton = false;
    }

    void OnUpdateTask(string task)
    {

        if(m_CurrentTaskNumber != 5)
        {
            for(int i = m_CurrentTaskNumber - 1; i >= 0; i--)
            {
                m_Tasks[i + 1].text = m_Tasks[i].text;
            }
            m_Tasks[0].text = task;
            if (m_CurrentTaskNumber == 3)
            {
                m_Tasks[1].color = new Color(0, 0, 0, 1);
                m_Tasks[1].fontStyle = FontStyles.Bold | FontStyles.Italic;
            }
            else if(m_CurrentTaskNumber == 4)
            {
                m_Tasks[2].color = new Color(0, 0, 0, 1);
                m_Tasks[2].fontStyle = FontStyles.Bold | FontStyles.Italic;
            }
            m_CurrentTaskNumber++;
        }
    }
    void OnUpdateThought(string thought)
    {
        m_ThoughtText.text = thought;
    }
    public void BackgroundMusicLeftArrow()
    {
        float mixerLevelDB = 0;
        m_BackgroundSoundSlider.value--;
        GameState.Volume = (int)m_BackgroundSoundSlider.value;
        switch (m_BackgroundSoundSlider.value)
        {
            case 0: mixerLevelDB = -80.0f; break;
            case 1: mixerLevelDB = -20.0f; break;
            case 2: mixerLevelDB = -15.0f; break;
            case 3: mixerLevelDB = -12.0f; break;
            case 4: mixerLevelDB = -9.0f; break;
            case 5: mixerLevelDB = -6.0f; break;
            case 6: mixerLevelDB = -3.0f; break;
            case 7: mixerLevelDB = 0.0f; break;

        }
        AudioMixer.SetFloat("Master", mixerLevelDB);
        m_AudioSource.PlayOneShot(m_ButtonClick);
    }
    public void BackgroundMusicRightArrow()
    {
        float mixerLevelDB = 0;
        m_BackgroundSoundSlider.value++;
        GameState.Volume = (int)m_BackgroundSoundSlider.value;
        switch (m_BackgroundSoundSlider.value)
        {
            case 0: mixerLevelDB = -80.0f; break;
            case 1: mixerLevelDB = -20.0f; break;
            case 2: mixerLevelDB = -15.0f; break;
            case 3: mixerLevelDB = -12.0f; break;
            case 4: mixerLevelDB = -9.0f; break;
            case 5: mixerLevelDB = -6.0f; break;
            case 6: mixerLevelDB = -3.0f; break;
            case 7: mixerLevelDB = 0.0f; break;

        }
        AudioMixer.SetFloat("Master", mixerLevelDB);
        m_AudioSource.PlayOneShot(m_ButtonClick);
    }

    void ActiveButton()
    {

    }
}
