using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class RemoveObstaclesInBetween : MonoBehaviour
{
    private GameObject                   m_LookAtTarget;
    public GameObject                    Guard;
    private Vector3                      m_RayCastDirectionPlayer;
    private Vector3                      m_RayCastDirectionGuard;
    private Dictionary<string, Material[]> m_WallCache;
    private LayerMask                    m_WallMask;
    private CinemachineBrain             m_CineBrain;
    private bool m_EnableRayCast;
    void Start()
    {
        m_WallMask         = LayerMask.GetMask("Transparent");
        m_LookAtTarget     = GameObject.FindGameObjectWithTag("Player");
        m_EnableRayCast    = true;
        m_WallCache        = new Dictionary<string, Material[]>();
        if(Guard)
            m_RayCastDirectionGuard = Guard.transform.position - transform.position;
        m_RayCastDirectionPlayer = m_LookAtTarget.transform.position - transform.position;
        m_CineBrain        = GetComponent<CinemachineBrain>();
    }
    void Update()
    {
        if(!m_CineBrain)
        {
            if (m_CineBrain.ActiveVirtualCamera.Name == "PlayerFollowCamera")
                m_EnableRayCast = true;
            else
                m_EnableRayCast = false;
        }
        if(m_CineBrain.IsBlending)
        {
            m_EnableRayCast = false;
        }
        if (m_EnableRayCast)
        {


            RaycastHit[] hitObjectsPlayer = Physics.RaycastAll(transform.position, m_RayCastDirectionPlayer, m_RayCastDirectionPlayer.magnitude, m_WallMask);
            //Guard should also be able to change the transparency of the walls.

            if (hitObjectsPlayer.Length > 0)
            {
                for(int i = 0; i < hitObjectsPlayer.Length; i++ )
                {
                    if(!m_WallCache.ContainsKey(hitObjectsPlayer[i].transform.name))
                    {
                        Material[] materials = hitObjectsPlayer[i].transform.GetComponent<Renderer>().materials;
                        m_WallCache.Add(hitObjectsPlayer[i].transform.name, materials);
                        foreach(Material mat in materials)
                        {
                            StartCoroutine(FadeAlpha(mat));
                        }
                    }
                }
            }
            else
            {
                if(m_WallCache.Count > 0)
                {
                    StopAllCoroutines();
                    foreach(var pair in m_WallCache)
                    {
                        foreach(Material mat in pair.Value)
                        {
                            mat.color = new Vector4(mat.color.r, mat.color.g, mat.color.b, 1.0f);
                        }
                    }
                    m_WallCache.Clear();
                }
            }
        }
    }
    IEnumerator FadeAlpha(Material material)
    {
        while(material.color.a > 0)
        {
            material.color = new Vector4(material.color.r, material.color.g, material.color.b, material.color.a - 4.0f * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
