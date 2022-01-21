using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Intro : MonoBehaviour
{
    private Animator m_Animator;
    private TextMeshProUGUI m_Text;
    private GameObject m_Image;
    private GameObject m_Image2;
    private GameObject m_Image3;
    private string m_Passage;
    private int m_Counter;
    private bool m_ListenForInput;
    private GameObject m_MouseClickImage;
    void Start()
    {
        m_MouseClickImage = transform.parent.Find("MouseClick").gameObject;
        m_ListenForInput = false;
        m_Passage = @"  There is a prison in the countryside of the town. Most people have only heard of this prison. It has existed in the town for many years. Not only does it receive local prisoners, but it is also responsible for obtaining the most dangerous criminals from all over the country. These vicious criminals are rehabilitated here, and no one knows how the prison staff manages them. All that anyone knows is that the town's security has indirectly improved because of them. 

    Each term's director has been one of the town's most distinguished men. This one is even younger, having become the new director at the age of 25. He was not only well versed in psychology but also in medicine, and he knew how to keep these criminals in peace in this prison. It is rumoured that during his first year in charge of the prison, the drug lords began to be active in the town. In just one year, the drug lord, who had been involved in the drug business in other countries for ten years, disappeared from the town. He was not only a prison governor but a hero of the town. Under his administration, the prison ran in perfect order until this year.
";
        m_Counter = 0;
        m_Animator = GetComponent<Animator>();
        m_Text = transform.root.Find("Text").GetComponent<TextMeshProUGUI>();
        m_Image = transform.root.Find("Image").gameObject;
        m_Image2 = transform.root.Find("Image2").gameObject;
        m_Image3 = transform.root.Find("Image3").gameObject;
    }
    private void Update()
    {
        if(m_ListenForInput)
        {
            if(Input.GetMouseButtonUp(0))
            {
                m_MouseClickImage.GetComponent<Animator>().SetTrigger("FadeOut");
                m_ListenForInput = false;
                if (m_Counter == 3)
                {
                    EventManager.Get().FadeOutMusic(4.0f);
                    m_Animator.SetTrigger("FadeOutOutro");
                }
                else
                {
                    ClearText();
                    TypeSentence();
                }
            }
        }
    }
    public void TypeSentence()
    {
        StartCoroutine(TypeSentence(m_Passage, 0.005f));
    }
    IEnumerator TypeSentence(string sentence, float typeSpeed = 0) //Animating the letters with this.
    {
        m_Text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            m_Text.text += letter;
            if (typeSpeed == 0)
                yield return null;
            else
                yield return new WaitForSeconds(typeSpeed);
        }
        yield return new WaitForSeconds(1);
        m_Counter++;
        m_ListenForInput = true;
        m_MouseClickImage.SetActive(true);
    }
    public void ClearText()
    {
        m_Text.text  = "";
        switch(m_Counter)
        {
            case 1: m_Image.SetActive(false); m_Image2.SetActive(true); m_Passage = @"The year 1843 was destined to be a crazy year. A horrible mental illness spread among the people. No people knew where the illness had come from. Nor did people then understand why a disease that drove people mad was contagious. All they knew was that anyone who contracted the disease would experience hallucinations and then go mad until they died. No medical treatment of the time could cure the disease until a doctor came up with a treatment plan. But when the doctors said that human trials were needed, the local people were strongly opposed to human trials, and this treatment was forced to be abandoned. The people were all in a state of panic.
Moreover, the flood of drugs, the scandals of the plutocrats and the riots in prison all happened this year. And all these events seemed to be linked to a prison. At the same time, many people's life changed completely from that year. 

    It is now 1863. Twenty years have passed quickly. So many stories have been related in this prison in that time. Some have left, some have died, people's lives go on even though life here has changed radically. The first of the two most notable changes of the town is that medicines for the mental illness of the year have been developed, and the hospital claimed that the medicine could treat the mental illness. The second is the former prison director has gone missing. Some say he has been seen in other towns; others say he is dead."; break;
            case 2: m_Image2.SetActive(false); m_Image3.SetActive(true); m_Passage = @"The position of the current prison director has been taken over by the deputy director. The whole prison has also been renovated and appears to be no different from life before 1843. But there were still some strange things that happened after that. Some of those who had a mental health condition back then were cured, while others disappeared during their treatment, even their bodies, and the explanation given by the hospital was simply that they had deteriorated during the treatment and eventually died. 

The truth about this prison is waiting for you to uncover it."; break;
        }
    }
    public void SceneLoader(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
