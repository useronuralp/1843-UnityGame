using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownstairsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventManager.Get().DisableSecondFloor();
        }
    }
}
