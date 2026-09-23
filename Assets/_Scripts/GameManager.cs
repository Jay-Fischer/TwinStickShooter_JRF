using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn System")]
    public GameObject _enemy;
    public Transform[] _spawnPoints;
    public int _enemiesInPlay;
    public int maxEnemyCount;
    public int enemyKills = 0;
    public float spawnSpeed;

    [Header("Panels")]
    public GameObject upgradePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
    }

    private void Start()
    {
        InvokeRepeating("spawnEnemy", 3f, spawnSpeed);
    }

    public void spawnEnemy()
    {
        if(_enemiesInPlay < maxEnemyCount)
        {
            _enemiesInPlay++;
            int _spawnChoice = Random.Range(0, _spawnPoints.Length);

            Instantiate(_enemy, _spawnPoints[_spawnChoice].position,
                _spawnPoints[_spawnChoice].rotation);
        }
    }
    public void EnemyKillCount()
    {
        _enemiesInPlay--;
        enemyKills++;
        if(enemyKills >= 10)
        {
            enemyKills = 0;
            OpenUpgradePanel();

        }
            
    }

    void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void pauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        upgradePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void RestartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pauseGame();
        }
    }
}
