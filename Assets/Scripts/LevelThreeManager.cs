using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreeManager : MonoBehaviour
{
    public GameObject SecondFloor;
    public GameObject SecondFloorWalls;
    void Start()
    {
        EventManager.Get().OnPlayerEntersFirstFloor += OnEnteringFirstFloor;
        EventManager.Get().OnPlayerEntersSecondFloor += OnEnteringSecondFloor;
    }
    void OnEnteringFirstFloor()
    {
        if (GameState.IsSecondFloorEnabled)
        {
            if (SecondFloor.activeInHierarchy)
            {
                SecondFloor.SetActive(false);
            }
            if (SecondFloorWalls.activeInHierarchy)
            {
                SecondFloorWalls.SetActive(false);
            }
            GameState.IsSecondFloorEnabled = false;
            EventManager.Get().StopAI();
        }
    }
    void OnEnteringSecondFloor()
    {
        if(!GameState.IsSecondFloorEnabled)
        {
            if (!SecondFloor.activeInHierarchy)
            {
                SecondFloor.SetActive(true);
            }
            if (!SecondFloorWalls.activeInHierarchy)
            {
                SecondFloorWalls.SetActive(true);
            }
            GameState.IsSecondFloorEnabled = true;
            EventManager.Get().ContinueAI();
        }
    }
}
