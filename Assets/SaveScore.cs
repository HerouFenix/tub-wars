using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    public List<Tuple<string, int>> scores = new List<Tuple<string, int>>();

    private List<TextMeshProUGUI> scores_texts = new List<TextMeshProUGUI>();
    public GameObject scores_hud;


    private void Awake()
    {
        scores_texts.AddRange(scores_hud.GetComponentsInChildren<TextMeshProUGUI>());

        // Verify all scores and set them if not created yet
        for (int i = 0; i < 10; i++)
        {
            if(!PlayerPrefs.HasKey("highScore_" + (i))){
                PlayerPrefs.SetFloat("highScore_" + (i), 0.0f);
                PlayerPrefs.SetString("highScoreName_" + (i), "");
            }

        }
    }

    public void RecordScore(float newScore, string name)
    {

        for (int i = 0; i < 10; i++)
        {
            if (newScore >= PlayerPrefs.GetFloat("highScore_" + i))
            {
                float oldValue = PlayerPrefs.GetFloat("highScore_" + (i));
                string oldName = PlayerPrefs.GetString("highScoreName_" + (i));

                for (int x = i + 1; x < 10; x++)
                {                    

                    float curValue = PlayerPrefs.GetFloat("highScore_" + (x));
                    string curName = PlayerPrefs.GetString("highScoreName_" + (x));

                    PlayerPrefs.SetFloat("highScore_" + (x), oldValue);
                    PlayerPrefs.SetString("highScoreName_" + (x), oldName);

                    oldValue = curValue;
                    oldName = curName;

                    if (oldName == "" && oldValue <= 0.0f)
                    {
                        break;
                    }
                }


                PlayerPrefs.SetFloat("highScore_" + (i), newScore);
                PlayerPrefs.SetString("highScoreName_" + (i), name);

                break;
            }
        }


        /*
        PlayerManager pm = GetComponent<PlayerManager>();
        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(pm._tag + "," + pm.score.ToString());
        sw.Flush();
        sw.Close();

        StreamReader sr = new StreamReader(path);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            string[] splitArray = line.Split(char.Parse(","));
            scores.Add(new Tuple<string, int>(splitArray[0], Convert.ToInt32(splitArray[1])));
        }
        scores.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        for (int i = 0; i < scores.Count; i++)
        {
            scores_texts[i].text = scores[i].Item1 + "   /   " + (scores[i].Item2 + 1);
        }
        sr.Close();
        */
    }
}
