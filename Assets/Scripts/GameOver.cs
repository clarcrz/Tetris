using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text Score;
    public Text Level;
    public Text Lines;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 score_data = FindObjectOfType<DataHolder>().GetScoreData();
        Score.text = ((int)score_data[0]).ToString().PadLeft(6, '0');
        Level.text = ((int)score_data[1]).ToString().PadLeft(3, '0');
        Lines.text = ((int)score_data[2]).ToString().PadLeft(3, '0');
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            FindObjectOfType<DataHolder>().SetScoreData(0, 1, 0);
            SceneManager.LoadScene("Level");
        }
    }
}
