using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeToReloadScene;
    [SerializeField] private int maxScore;

    [Space, SerializeField] private UnityEvent onStartGame;
    [SerializeField] private UnityEvent onGameOver, onIncreaseScore;

    public int score { get; private set; }
    public int maxScoreReached { get; private set; }
    public bool isGameOver { get; private set; }


    [SerializeField] private TextMeshProUGUI TopScore;

    // Singleton!
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this.gameObject);
    }

    public void StartGame()
    {
        Debug.Log("GameManager :: StartGame()");
        onStartGame?.Invoke();
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        Debug.Log("GameManager :: GameOver()");
        isGameOver = true;

        if (score > maxScoreReached)
        {
            maxScoreReached = score; // Actualiza el puntaje máximo alcanzado si el puntaje actual es mayor
            PlayerPrefs.SetInt("MaxScore", maxScoreReached); // Guarda el puntaje máximo en PlayerPrefs
        }

        onGameOver?.Invoke();
        StartCoroutine(ReloadScene());
    }

    public void IncreaseScore()
    {
        Debug.Log("GameManager :: IncreaseScore()");
        score++;
        onIncreaseScore?.Invoke();
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(timeToReloadScene);
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        maxScoreReached = PlayerPrefs.GetInt("MaxScore", 0); 
        TopScore.text = ("Top Score: " + maxScoreReached.ToString()); 
    }
}
