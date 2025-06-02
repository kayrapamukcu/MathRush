using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartGame()
    {
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(1);
    }
}
