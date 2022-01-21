using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class EventManager : MonoBehaviour
{
    private static EventManager s_Instance;
    private void Awake()
    {
        s_Instance = this;
    }
    //Events
    public event Action<string> OnTypeSentenceToHeadBubble;
    public event Action OnPlayerCellGateOpen;
    public event Action OnPlayerCellGateClose;
    public event Action OnInteractingWithBucket;
    public event Action OnPlayerCompletesTutorialRoomTasks;
    public event Action<bool> OnPlayerPressedCandleToggleButton;

    //Clue pickups go here. These are fired after the pick up animation is completed.
    public event Action OnPickingUpShirt;
    public event Action OnPickingUpCandle;
    public event Action OnPickingUpFile;
    public event Action OnPickingUpMedicine;
    public event Action OnPickingUpStaffList_1;
    public event Action OnPickingUpStaffList_2;
    public event Action OnPickingUpPhotograph;
    public event Action OnPickingUpPhotograph_2;
    public event Action OnPickingUpSyringe;
    public event Action OnPickingUpPocketwatch;
    public event Action OnPickingUpNewspaper;
    public event Action OnPickingUpLetter;

    //These animations are when the clues are clicked for the first time.
    public event Action OnClickingShirt;
    public event Action OnClickingCandle;
    public event Action OnClickingFile;
    public event Action OnClickingMedicine;
    public event Action OnClickingStaffList_1;
    public event Action OnClickingStaffList_2;
    public event Action OnClickingPhotograph;
    public event Action OnClickingPhotograph_2;
    public event Action OnClickingSyringe;
    public event Action OnClickingPocketwatch;
    public event Action OnClickingNewspaper;
    public event Action OnClickingLetter;

    //Dialogue Starter
    public event Action<Queue<GameObject>, bool> OnStartDialogue; //Dialogue manager is subscribed to this.

    //Inputs enable/disable
    public event Action OnMouseInputsEnabled;
    public event Action OnMouseInputsDisabled;
    public event Action OnKeyboardInputsEnabled;
    public event Action OnKeyboardInputsDisabled;

    ////Item higlight upon mouse hover over
    //public event Action<string> OnMouseEntersClickable;
    //public event Action OnMouseLeavesClickable;

    //Dialogue Events
    public event Action OnStartingDialogue;
    public event Action OnEndingDialogue;

    //Not in use right now
    public event Action OnPlayerPicksUpAClue;


    //Disable/Enable UI
    public event Action OnDisableUIExceptJournalIcon;
    public event Action OnDisableUI;
    public event Action OnEnableUI;

    public event Action OnForceTurnOffCandle;

    public event Action OnClosingJournal;

    //Candle Mechanics
    public event Action OnDisableCandleUse;
    public event Action OnEnableCandleUse;
    public event Action OnPauseCandleCounter;
    public event Action OnContinueCandleCounter;
    public event Action OnAddCandle;

    public event Action OnLoadingLevel_Three;

    public event Action OnPlayerEntersFirstFloor;
    public event Action OnPlayerEntersSecondFloor;

    public event Action OnEnableSanityBar;
    public event Action OnDisableSanityBar;

    public event Action OnAnimateCandle;
    public event Action OnDoNotAnimateCandle;
    public event Action OnResettingCandleAnim;

    public event Action OnEnablingJournalButton;
    public event Action OnDisablingJournalButton;

    public event Action<string, string> OnGameOver;
    public event Action OnCloseGameOverCanvas;

    public event Action OnGuardStartsWandering;

    public event Action OnStopAI;
    public event Action OnContinueAI;

    public event Action OnLMBTutorial;
    public event Action OnSanityTutorial;

    //Music
    public event Action<float> OnFadeOutMusic;
    public event Action OnDisableSound;
    public event Action OnEnableSound;
    public event Action OnStartChaseMusic;
    public event Action OnContinueExploreMusic;
    public event Action OnClickingAclue;

    public event Action<string> OnUpdateThought;
    public event Action<string> OnUpdateTask;

    public event Action OnSafeEnding;
    public event Action OnMainDoorEnding;

    public void PickedUpLetter()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpLetter?.Invoke();
    }
    public void ClickedLetter()
    {
        OnClickingAclue?.Invoke();
        OnClickingLetter?.Invoke();
    }
    public void PickedUpNewspaper()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpNewspaper?.Invoke();
    }
    public void ClickedNewspaper()
    {
        OnClickingAclue?.Invoke();
        OnClickingNewspaper?.Invoke();
    }
    public void PickedUpPocketwatch()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpPocketwatch?.Invoke();
    }
    public void ClickedPocketwatch()
    {
        OnClickingAclue?.Invoke();
        OnClickingPocketwatch?.Invoke();
    }
    public void PickedUpSyringe()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpSyringe?.Invoke();
    }
    public void ClickedSyringe()
    {
        OnClickingAclue?.Invoke();
        OnClickingSyringe?.Invoke();
    }
    public void PickedUpPhotograph_2()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpPhotograph_2?.Invoke();
    }
    public void ClickedPhotograph_2()
    {
        OnClickingAclue?.Invoke();
        OnClickingPhotograph_2?.Invoke();
    }
    public void PickedUpPhotograph()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpPhotograph?.Invoke();
    }
    public void ClickedPhotograph()
    {
        OnClickingAclue?.Invoke();
        OnClickingPhotograph?.Invoke();
    }
    public void PickedUpStaffList_1()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpStaffList_1?.Invoke();
    }
    public void ClickedStaffList_1()
    {
        OnClickingAclue?.Invoke();
        OnClickingStaffList_1?.Invoke();
    }
    public void PickedUpStaffList_2()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpStaffList_2?.Invoke();
    }
    public void ClickedStaffList_2()
    {
        OnClickingAclue?.Invoke();
        OnClickingStaffList_2?.Invoke();
    }
    public void PickedUpMedicine()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpMedicine?.Invoke();
    }
    public void ClickedMedicine()
    {
        OnClickingAclue?.Invoke();
        OnClickingMedicine?.Invoke();
    }
    public void PickedUpFile()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpFile?.Invoke();
    }
    public void ClickedFile()
    {
        OnClickingAclue?.Invoke();
        OnClickingFile?.Invoke();
    }
    public void TypeSentenceToHeadBubble(string sentence)
    {
        OnTypeSentenceToHeadBubble?.Invoke(sentence);
    }
    public void ClickedCandle()
    {
        OnClickingAclue?.Invoke();
        OnClickingCandle?.Invoke();
    }
    public void PickedUpCandle()
    {
        OnEnableCandleUse?.Invoke();
        OnPickingUpCandle?.Invoke();
    }
    public void EnableMouseInputs()
    {
        OnMouseInputsEnabled?.Invoke();
    }
    public void DisableMouseInputs()
    {
        OnMouseInputsDisabled?.Invoke();
    }
    public void OpenPlayerCellGate()
    {
        OnPlayerCellGateOpen?.Invoke();
    }
    public void ClosePlayerCellGate()
    {
        OnPlayerCellGateClose?.Invoke();
    }
    public void StartDialogue(Queue<GameObject> queue, bool isEndOfLevel = false)
    {
        OnStartDialogue?.Invoke(queue, isEndOfLevel);
    }
    public void PlayerCompletedTutorialRoomTasks()
    {
        OnPlayerCompletesTutorialRoomTasks?.Invoke();
    }
    public void PlayerPressedCandleToggleButton(bool candleStatus)
    {
        OnPlayerPressedCandleToggleButton?.Invoke(candleStatus);
    }
    public void EnableKeyboardInputs()
    {
        OnKeyboardInputsEnabled?.Invoke();
    }
    public void DisableKeyboardInputs()
    {
        OnKeyboardInputsDisabled?.Invoke();
    }
    //public void MouseHoveredOverClickable(string itemName)
    //{
    //    OnMouseEntersClickable?.Invoke(itemName);
    //}
    //public void MouseLeftClickable()
    //{
    //    OnMouseLeavesClickable?.Invoke();
    //}
    public void PickedUpShirt()
    {
        OnPlayerPicksUpAClue?.Invoke();
        OnPickingUpShirt?.Invoke();
    }
    public void InteractedWithTheBucket()
    {
        OnInteractingWithBucket?.Invoke();
    }
    public void StartedDialogue()
    {
        OnDisableUI?.Invoke();
        OnStartingDialogue?.Invoke();
    }
    public void EndedDialogue()
    {
        OnEndingDialogue?.Invoke();
        OnEnableUI?.Invoke();
    }
    public void PickedUpClue()
    {
        OnPlayerPicksUpAClue?.Invoke();
    }
    public void ClickedShirt()
    {
        OnClickingAclue?.Invoke();
        OnClickingShirt?.Invoke();
    }
    public void DisableUIExceptJournalIcon()
    {
        OnDisableUIExceptJournalIcon?.Invoke();
    }
    public void DisableUI()
    {
        OnDisableUI?.Invoke();
    }
    public void EnableUI()
    {
        OnEnableUI?.Invoke();
    }
    public void ForceTurnOffCandle()
    {
        OnForceTurnOffCandle?.Invoke();
    }
    public void ClosedJournal()
    {
        OnClosingJournal?.Invoke();
    }
    public void DisableCandleUse()
    {
        OnDisableCandleUse?.Invoke();
    }
    public void EnableCandleUse()
    {
        OnEnableCandleUse?.Invoke();
    }
    public void PauseCandleCounter()
    {
        OnPauseCandleCounter?.Invoke();
    }
    public void ContinueCandleCounter()
    {
        OnContinueCandleCounter?.Invoke();
    }
    public void SetupLevelThree()
    {
        OnLoadingLevel_Three?.Invoke();
    }
    public void DisableSecondFloor()
    {
        OnPlayerEntersFirstFloor?.Invoke();
    }
    public void EnableSecondFloor()
    {
        OnPlayerEntersSecondFloor?.Invoke();
    }
    public void EnableSanityBar()
    {
        OnEnableSanityBar?.Invoke();
    }
    public void DisableSanityBar()
    {
        OnDisableSanityBar?.Invoke();
    }
    public void AnimateCandle()
    {
        OnAnimateCandle?.Invoke();
    }
    public void DoNotAnimateCandle()
    {
        OnDoNotAnimateCandle?.Invoke();
    }
    public void ResetCandleAnim()
    {
        OnResettingCandleAnim?.Invoke();
    }
    public void EnableJournalButton()
    {
        OnEnablingJournalButton?.Invoke();
    }
    public void DisableJournalButton()
    {
        OnDisablingJournalButton?.Invoke();
    }
    public void AddCandle()
    {
        OnAddCandle?.Invoke();
    }
    public void GameOver(string cause, string tip)
    {
        OnGameOver?.Invoke(cause, tip);
    }
    public void CloseGameOver()
    {
        OnCloseGameOverCanvas?.Invoke();
    }
    public void StartWanderingGuard()
    {
        OnGuardStartsWandering?.Invoke();
    }
    public void StopAI()
    {
        OnStopAI?.Invoke();
    }
    public void ContinueAI()
    {
        OnContinueAI?.Invoke();
    }
    public void DisplayLMBTutorial()
    {
        OnLMBTutorial?.Invoke();
    }
    public void DisplaySanityTutorial()
    {
        OnSanityTutorial?.Invoke();
    }
    public void FadeOutMusic(float duration)
    {
        OnFadeOutMusic?.Invoke(duration);
    }
    public void DisableSound()
    {
        OnDisableSound?.Invoke();
    }
    public void EnableSound()
    {
        OnEnableSound?.Invoke();
    }
    public void StartChaseMusic()
    {
        OnStartChaseMusic?.Invoke();
    }
    public void ContinueExploreMusic()
    {
        OnContinueExploreMusic?.Invoke();
    }
    public void UpdateThought(string thought)
    {
        OnUpdateThought?.Invoke(thought);
    }
    public void UpdateTask(string task)
    {
        OnUpdateTask?.Invoke(task);
    }
    public void SafeEnding()
    {
        OnSafeEnding?.Invoke();
    }
    public void MainDoorEnding()
    {
        OnMainDoorEnding?.Invoke();
    }
    //Singleton getter
    public static EventManager Get()
    {
        return s_Instance;
    }
}
