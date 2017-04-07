using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Playing,
    End
}

public class GameController : MonoBehaviour
{
    public GameObject startUI;
    public GameObject gameoverUI;
    public Text txtHistory;
    public Text txtCurrent;
    public Text txtScore;

    public static int[] xOffsets = new int[3] { -14, 0, 14 };
    public static GameState gameState = GameState.Menu;

    public static GameController Instance;

    private int score;
    private float startZ;
    private Transform player;

    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        startZ = player.position.z;
    }

    void Update()
    {
        if (GameController.gameState == GameState.Menu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameState = GameState.Playing;
                startUI.SetActive(false);
            }
        }
        if (GameController.gameState == GameState.End)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameState = GameState.Menu;
                SceneManager.LoadScene(0);
            }
        }
        if (GameController.gameState == GameState.Playing)
        {
            score = (int)(player.position.z - startZ);
            txtScore.text = "Score : " + score;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Quit();
        }
    }

    public void GameOver()
    {
        gameState = GameState.End;

        var highest = PlayerPrefs.GetInt("HighestScore");
        txtHistory.text = "历史最高 " + highest;
        txtCurrent.text = "本次得分 " + score;
        gameoverUI.SetActive(true);

        if (score > highest)
        {
            PlayerPrefs.SetInt("HighestScore", score);
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
