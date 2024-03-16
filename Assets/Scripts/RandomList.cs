using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomList : MonoBehaviour
{
    List<string> m_HarcCodedStrings = new List<string>();
    public GameObject[] output;

    void Start()
    {
        m_HarcCodedStrings.Add("Quelle est la somme de 1+1");
        m_HarcCodedStrings.Add("Quel est le poids total de 8 rhinocéros ?");
        m_HarcCodedStrings.Add("Quand est-ce qu'on mange ?");
        m_HarcCodedStrings.Add("Qui est le meilleur professeur ?");
        m_HarcCodedStrings.Add("Qu'est-ce qui est jaune et qui attend ?");
        m_HarcCodedStrings.Add("Quelle est la vitesse maximale d'un tigre blanc ?");
        m_HarcCodedStrings.Add("Puis-je adopter un tigre blanc ?");
        m_HarcCodedStrings.Add("Quels sont les prédateurs du tigre blanc ?");
        m_HarcCodedStrings.Add("Où pouvons-nous trouver des tigres blancs ?");
        m_HarcCodedStrings.Add("Qui est le plus fort entre un tigre et un lion ?");

        FillList();
    }

    void FillList()
    {
        ShuffleList(m_HarcCodedStrings);

        for (int i = 0; i < Mathf.Min(m_HarcCodedStrings.Count, output.Length); i++)
        {
            var text = output[i].GetComponent<TMP_Text>();
            text.text = m_HarcCodedStrings[i];
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
