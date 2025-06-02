using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;

    private void Start()
    {
        scoreText.fontSize = 32;
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            scoreText.text = $"Score: {ScoreManager.Instance.TotalScore}";
        } else
        {
            scoreText.text = $"Level {SceneManager.GetActiveScene().buildIndex}\nScore: {ScoreManager.Instance.TotalScore}";
        }
        
    }
}

