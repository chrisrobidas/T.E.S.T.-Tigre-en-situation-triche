using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RandomList : MonoBehaviour
{

    public string[] m_HardCodedStrings;

    public GameObject[] output;

    void Start()
    {
        m_HardCodedStrings = new string[]
            {
                "Quelle est la somme de 1+1 ? ",
                "Quel est le poids total de 8 rhinocéros ?  ",
                "Quand est-ce qu'on mange ?",
                "Qui est le meilleur professeur ?",
                "Qu'est-ce qui est jaune et qui attend ?",
                "Quelle est la vitesse maximale d'un tigre blanc ",
                "Combien de rayures a un tigre blanc?",
                "Puis-je adopter un tigre blanc ? ",
                "Quels sont les prédateurs du tigre blanc ? ",
                "Où pouvons-nous trouver des tigres blancs ?",
            };

        FillList();


    }


    public string GetRandomHardCodedString()
    {
        return m_HardCodedStrings[Random.Range(0, m_HardCodedStrings.Length)];
    }

    void FillList()
    {

        for (int i = 0; i < m_HardCodedStrings.Length; i++)
        {
            var text = output[i].GetComponent<TextMeshPro>();
            text.text = GetRandomHardCodedString();

        }

    }


}