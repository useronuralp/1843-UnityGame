using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : Clickable
{
    private static bool m_StartListening;
    protected override void Awake()
    {
        base.Awake();
        m_ItemName = "Candle";
    }
    public override void OnPlayerLeavesInteractionRange()
    {

    }
    public override void OnCompletingEnlargmentAnimation() //TODO : This part is a little problematic. This function is being called from tha main prefab not from the instance in the scene. So whatever you set here needs to be STATIC.
    {
        throw new System.NotImplementedException();
    }
    public override void OnClick()
    {
        if (m_IsPlayerInInteractionRange)
        {
            EventManager.Get().AddCandle();
            Destroy(transform.parent.gameObject);
        }
    }
}
