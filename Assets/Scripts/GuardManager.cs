using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    void Start()
    {
        EventManager.Get().OnStopAI += OnStopAI;
        EventManager.Get().OnContinueAI += OnContinueAI;
    }
    public void OnStopAI()
    {
        transform.Find("Guard").GetComponent<GuardHostile>().StopAllCoroutines();
        transform.Find("Guard").gameObject.SetActive(false);
    }
    public void OnContinueAI()
    {
        transform.Find("Guard").gameObject.SetActive(true);
        transform.Find("Guard").GetComponent<GuardHostile>().ResetLocation();
    }
}
