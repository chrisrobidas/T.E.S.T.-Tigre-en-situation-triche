using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomList : MonoBehaviour
{
    List<string> m_HarcCodedStrings = new List<string>();
    List<string> m_HarcCodedStrings_Reponse = new List<string>();

    public GameObject[] output;
    public GameObject[] outputReponse;

    void Start()
    {
        m_HarcCodedStrings.Add("Quelle est la somme de 1+1 ?");
        m_HarcCodedStrings.Add("Quel est le poids total de 8 rhinocéros ?");
        m_HarcCodedStrings.Add("Quand est-ce qu'on mange ?");
        m_HarcCodedStrings.Add("Qui est le meilleur professeur ?");
        m_HarcCodedStrings.Add("Qu'est-ce qui est jaune et qui attend ?");
        m_HarcCodedStrings.Add("Quelle est la vitesse maximale d'un tigre blanc ?");
        m_HarcCodedStrings.Add("Puis-je adopter un tigre blanc ?");
        m_HarcCodedStrings.Add("Quels sont les prédateurs du tigre blanc ?");
        m_HarcCodedStrings.Add("Où pouvons-nous trouver des tigres blancs ?");
        m_HarcCodedStrings.Add("Qui est le plus fort entre un tigre et un lion ?");
        m_HarcCodedStrings.Add("Est-ce que les ananas vont sur la pizza ?");
        m_HarcCodedStrings.Add("Quel est le bébé du tigre ?");
        m_HarcCodedStrings.Add("Combien pèse Madame Griffe ?");
        m_HarcCodedStrings.Add("iOS ou Android ? ");
        m_HarcCodedStrings.Add("Pourquoi les magasins ouverts 24h/24 ont-ils des serrures ?");
        m_HarcCodedStrings.Add("Qui était là en premier : l'oeuf ou la poule ?");
        m_HarcCodedStrings.Add("Pourquoi les biscuits durs deviennent-ils mous et inversement ?");
        m_HarcCodedStrings.Add("Quel est le synonyme de synonyme ?");
        m_HarcCodedStrings.Add("Comment s'appelait le Capitaine Crochet avant de perdre sa main ?");
        m_HarcCodedStrings.Add("Burger King ou Mcdonalds ?");
        m_HarcCodedStrings.Add("Quelles sont les 5 premières décimales de PI ?");
        m_HarcCodedStrings.Add("Quel est le mélange du tigre et du lion ?");


        m_HarcCodedStrings_Reponse.Add("2.");
        m_HarcCodedStrings_Reponse.Add("12 tonnes.");
        m_HarcCodedStrings_Reponse.Add("Ce soir.");
        m_HarcCodedStrings_Reponse.Add("Vous bien sûr.");
        m_HarcCodedStrings_Reponse.Add("Jonathan.");
        m_HarcCodedStrings_Reponse.Add("50Km/h.");
        m_HarcCodedStrings_Reponse.Add("Si tu le caches dans ta cave.");
        m_HarcCodedStrings_Reponse.Add("Les humains.");
        m_HarcCodedStrings_Reponse.Add("Asie du sud.");
        m_HarcCodedStrings_Reponse.Add("Le tigre.");
        m_HarcCodedStrings_Reponse.Add("Non, ce ne serait plus une pizza.");
        m_HarcCodedStrings_Reponse.Add(" Le tigreau.");
        m_HarcCodedStrings_Reponse.Add("Entre 65 et 170 kilos.");
        m_HarcCodedStrings_Reponse.Add("Android.");
        m_HarcCodedStrings_Reponse.Add("Pourquoi pas ?");
        m_HarcCodedStrings_Reponse.Add("L'oeuf.");
        m_HarcCodedStrings_Reponse.Add("Pour encourager le capitalisme.");
        m_HarcCodedStrings_Reponse.Add("Identique.");
        m_HarcCodedStrings_Reponse.Add("Jacques.");
        m_HarcCodedStrings_Reponse.Add("Mcdo pour les frites.");
        m_HarcCodedStrings_Reponse.Add("14159.");
        m_HarcCodedStrings_Reponse.Add("Le ligre.");









        ShuffleLists();

        FillList();
    }

    void FillList()
    {
        for (int i = 0; i < 10; i++)
        {
            var text = output[i].GetComponent<TMP_Text>();
            var text2 = outputReponse[i].GetComponent<TMP_Text>();
            text.text = m_HarcCodedStrings[i];
            text2.text = m_HarcCodedStrings_Reponse[i];
            outputReponse[i].GetComponent<TMP_Text>().alpha = 0.0f;
        }
    }

    void ShuffleLists()
    {
        // Utiliser le même ordre de mélange pour les deux listes
        for (int i = 0; i < 10; i++)
        {
            int randomIndex = Random.Range(i, m_HarcCodedStrings.Count);

            // Échanger les éléments dans la liste m_HarcCodedStrings
            string temp = m_HarcCodedStrings[i];
            m_HarcCodedStrings[i] = m_HarcCodedStrings[randomIndex];
            m_HarcCodedStrings[randomIndex] = temp;

            // Échanger les éléments dans la liste m_HarcCodedStrings_Reponse
            string temp2 = m_HarcCodedStrings_Reponse[i];
            m_HarcCodedStrings_Reponse[i] = m_HarcCodedStrings_Reponse[randomIndex];
            m_HarcCodedStrings_Reponse[randomIndex] = temp2;
        }
    }
}
