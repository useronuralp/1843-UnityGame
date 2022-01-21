using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpstairsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Get().EnableSecondFloor();
        }
    }
}
