using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CursorManager : MonoBehaviour
{
    private Texture2D[] m_HandIcons;
    private Texture2D m_UnlockIcon;
    private Texture2D[] m_InteractableIcons;

    private TextMeshProUGUI m_ItemHoverName;
    private bool m_HoveredOverClue;
    private bool m_IsCursorDefualt;
    private float m_AnimationTimer;
    private float m_AnimationRate = 0.3f;
    private int m_CurrentFrame;
    private int m_FrameCount = 2;
    void Start()
    {
        m_CurrentFrame = 0;
        m_AnimationTimer = m_AnimationRate;
        m_IsCursorDefualt = true;
        m_ItemHoverName = transform.Find("MouseHoverTextCanvas").Find("MouseText").GetComponent<TextMeshProUGUI>();
        m_ItemHoverName.text = "";
        m_HandIcons = new Texture2D[2];
        m_InteractableIcons = new Texture2D[2];
        m_HandIcons[0] = Resources.Load<Texture2D>("Hand1");
        m_HandIcons[1] = Resources.Load<Texture2D>("Hand2");
        m_InteractableIcons[0] = Resources.Load<Texture2D>("See1");
        m_InteractableIcons[1] = Resources.Load<Texture2D>("See2");
        m_UnlockIcon = Resources.Load<Texture2D>("Unlock1");
    }
    void Update()
    {
        if (m_IsCursorDefualt)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            m_AnimationTimer = m_AnimationRate;
        }
        else
        {
            m_AnimationTimer -= Time.deltaTime;
            if(m_AnimationTimer <= 0.0f)
            {
                m_AnimationTimer = m_AnimationRate;
                m_CurrentFrame = (++m_CurrentFrame) % 2;
            }
        }
        m_ItemHoverName.text = "";
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitObjects = Physics.RaycastAll(ray);
        foreach(var obyekt in hitObjects)
        {
            if (obyekt.transform.gameObject.layer == 7) //Clickable layer.
            {
                var clickableScript = obyekt.transform.GetComponent<Clickable>();
                if (!GameManager.Get().IsMouseOverUI())
                {
                    if (obyekt.transform.CompareTag("Candle"))
                    {
                        Cursor.SetCursor(m_HandIcons[m_CurrentFrame], Vector2.zero, CursorMode.Auto);
                        m_IsCursorDefualt = false;
                    }
                    else if (obyekt.transform.CompareTag("Lock"))
                    {
                        Cursor.SetCursor(m_UnlockIcon, Vector2.zero, CursorMode.Auto);
                        m_IsCursorDefualt = false;
                    }
                    else
                    {
                        Cursor.SetCursor(m_InteractableIcons[m_CurrentFrame], Vector2.zero, CursorMode.Auto);
                        m_IsCursorDefualt = false;
                    }
                    m_ItemHoverName.text = obyekt.transform.parent.name;
                    if (Input.GetMouseButtonUp(0))
                    {
                        clickableScript.OnClick();
                    }
                }
                break;
            }
            else
                m_IsCursorDefualt = true;
        }
        m_ItemHoverName.transform.position = Input.mousePosition;
        m_ItemHoverName.transform.position = new Vector3(m_ItemHoverName.transform.position.x, m_ItemHoverName.transform.position.y + 16, m_ItemHoverName.transform.position.z);
    }
}
