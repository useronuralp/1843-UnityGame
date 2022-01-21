using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfScript : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(transform.root.gameObject);
    }
}
