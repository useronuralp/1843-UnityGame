using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [TextArea (3, 10)]
    public string[] Sentences;
    public GameObject Camera; //The camera that the GameManager will switch to when this dialogue is being shown.
}
