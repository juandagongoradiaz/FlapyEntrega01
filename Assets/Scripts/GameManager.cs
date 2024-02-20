using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

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

    [SerializeField] private List<GameObject> prefabList; 


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
            maxScoreReached = score;
            PlayerPrefs.SetInt("MaxScore", maxScoreReached);
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

   
        if (prefabList.Count > 0)
        {
            int randomIndex = Random.Range(0, prefabList.Count);
            Instantiate(prefabList[randomIndex], Vector3.zero, Quaternion.identity);
        }
    }
}