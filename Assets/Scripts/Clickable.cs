using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This class is here to tint the color of clickable items when mouse hovers over them.
public abstract class Clickable : MonoBehaviour
{
    private Renderer m_Renderer;
    public bool m_IsPlayerInInteractionRange;
    protected string m_ItemName;
    protected virtual void Awake()
    {
        m_ItemName = "";
        m_Renderer = GetComponent<Renderer>();
    }
    
    private void Start()
    {
        m_IsPlayerInInteractionRange = false;
    }
    void Update()
    {

    }
    public abstract void OnClick();
    //private void OnMouseOver()
    //{
    //    if(!GameManager.Get().IsMouseOverUI())
    //    {
    //        EventManager.Get().MouseHoveredOverClickable(m_ItemName);
    //        if(m_Renderer)
    //        {
    //            m_Renderer.material.color = new Color(1, 0, 0, 1);
    //        }
    //        foreach(Transform child in transform)
    //        {
    //            var renderer = child.GetComponent<Renderer>();
    //            if (renderer)
    //            {
    //                renderer.material.color = new Color(1, 0, 0, 1);
    //            }
    //        }
    //    }
    //    //TODO : Cursor does not go back to it's default state when an item is picked up and the enlargment animaiton plays.
    //}
    private void OnMouseExit()
    {
        //EventManager.Get().MouseLeftClickable();
        //if (m_Renderer)
        //{
        //    m_Renderer.material.color = new Color(1, 1, 1, 1);
        //}
        //foreach (Transform child in transform)
        //{
        //    var renderer = child.GetComponent<Renderer>();
        //    if (renderer)
        //    {
        //        renderer.material.color = new Color(1, 1, 1, 1);
        //    }
        //}
    }
    public abstract void OnPlayerLeavesInteractionRange();
    public abstract void OnCompletingEnlargmentAnimation();
}
