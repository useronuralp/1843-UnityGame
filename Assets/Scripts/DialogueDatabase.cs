using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    private static DialogueDatabase s_Instance;
    private void Awake()
    {
        s_Instance = this;
    }
    //You have to set these in the editor.
    public GameObject LV0_PlayerSelf;
    public GameObject LV0_PlayerSelf_ShirtPickup;
    public GameObject LV0_PlayerSelf_CandlePickup;
    public GameObject LV0_Guard1;
    public GameObject LV0_Player1;
    public GameObject LV0_Guard2;
    public GameObject LV0_Player2;

    public GameObject LV1_Player1;
    public GameObject LV1_Director1;
    public GameObject LV1_Player2;
    public GameObject LV1_Director2;
    public GameObject LV1_Player3;
    public GameObject LV1_Director3;
    public GameObject LV1_Player4;
    public GameObject LV1_Director4;
    public GameObject LV1_Player5;
    public GameObject LV1_Director5;
    public GameObject LV1_Player6;
    public GameObject LV1_Director6;

    public GameObject LV3_PlayerSelf_Explorable;

    public static DialogueDatabase Get()
    {
        return s_Instance;
    }
}
