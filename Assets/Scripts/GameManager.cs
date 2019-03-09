using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int matchTime = 300;
    public int phaseOneTime = 120;
    public int phaseTime = 1;
    public Text scoreText;
    public GameObject endScreen; 
    public Text finalScoreText;
    private int score = 0;

    private float timer = 0f;
    private bool matchEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (matchEnd)
            return;

        timer += Time.deltaTime;

        if (timer >= matchTime)
        {
            StopGame();
        }
    }

    public void StopGame()
    {
        scoreText.enabled = false;
        endScreen.SetActive(true);
        finalScoreText.text = "Score: " + score;
        matchEnd = true;
    }

    public void AddScore(int scoreToAdd)
    {
        if (matchEnd)
            return;

        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
