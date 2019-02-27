using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public int score = 0;
    public int full_lines = 0;
    public int consecutivectr = 0;
    public int total_lines = 0;
    public int level_up_ctr = 0;
    public int level = 1;

    private GameObject nextTetromino;
    private GameObject previewTetromino;

    private bool gameStarted = false;

    private Vector2 previewTetrominoPosition = new Vector2(14f, 8f);

    public Text ScoreValue;
    public Text LineValue;
    public Text LevelValue;
    string activeTetrominoName = "Prefabs/Tetromino_T";
    string nextTetrominoName = "Prefabs/Tetromino_T";

    public Text debugString;
    int debugvalue = 0;

    private void Awake()
    {
        Debug.Log("Reset Level @ awake");
        level = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Reset Level @ start");
        level = 1;
        SpawnNextTetromino();
    }

    public bool CheckIsAboveGrid (Tetromino tetromino)
    {
        for (int x = 0; x < gridWidth; ++x) {
            foreach (Transform mino in tetromino.transform) {
                Vector2 pos = Round(mino.position);
                if (pos.y > gridHeight) {
                    return true;
                }
            }
        }
        return false;
    }

    /*
     * Check if row is full
     */
    public bool IsFullRowAt (int y)
    {
        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x,y] == null) {
                return false;
            }
        }
        return true;
    }

    /*
     * Delete minos per row
     */
    public void DeleteMinoAt (int y)
    {
        for (int x = 0; x < gridWidth; ++x) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;

            // add line counter
            full_lines++;
            
        }
        consecutivectr++;
        total_lines++;

        
    }

    /*
     * Move minos down after deletion per row
     */
    public void MoveRowDown (int y)
    {
        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x,y] != null) {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    /*
     * Move rows down after deletion
     */
    public void MoveAllRowsDown (int y)
    {
        for (int i = y; i < gridHeight; ++i) {
            MoveRowDown (i);
        }
    }

    /*
     * Delete full rows
     */
     public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y) {
            if (IsFullRowAt(y)) {
                level_up_ctr++;
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
        CalculateScore();
        full_lines = 0;
        consecutivectr = 0;
    }

    /*
     * Update grid transform to indicate the spaces already 
     * occupied by the tetromino pieces
     * 
     * Called @ Tetromino.CheckUserInput() - while pieces are moving
     */
    public void UpdateGrid (Tetromino tetromino)
    {
        // update grid for moving tetromino
        for (int y = 0; y < gridHeight; ++y) {
            for (int x = 0; x < gridWidth; ++x) {
                if (grid[x, y] != null) {
                    if (grid[x, y].parent == tetromino.transform) {
                        grid[x, y] = null;
                    }
                }
            }
        }

        // set individual mino to grid
        foreach (Transform mino in tetromino.transform) {
            Vector2 pos = Round(mino.position);

            if (pos.y < gridHeight) {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    /*
     * Get coordinates(tranform properties) at given position
     * to indicate if grid position is occupied
     */
    public Transform GetTransformAtGridPosition (Vector2 pos)
    {
        if (pos.y > gridHeight -1) {
            return null;
        } else {

            return grid[(int)pos.x, (int)pos.y];
        }
    }

    /*
     * next to active tetromino
     */
     public void SpawnNextTetromino()
    {
        if (!gameStarted) {
            gameStarted = true;
            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled = false;
        } else {
            previewTetromino.transform.position = new Vector2(5.0f, 20.0f);
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<Tetromino>().enabled = true;

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoPosition, Quaternion.identity);
            previewTetromino.GetComponent<Tetromino>().enabled = false;
        }
        
    }

    /*
     * Check if given position is inside the grid
     */
    public bool CheckIsInsideGrid (Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    /*
     * Round off coordinates
     */
    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    /*
     * Generate random piece and get tetromino prefab
     */
    string GetRandomTetromino ()
    {
        int randomTetromino = Random.Range(1, 8);

        switch (randomTetromino) {
            case 1:
                activeTetrominoName = "Prefabs/Tetromino_T";
                break;
            case 2:
                activeTetrominoName = "Prefabs/Tetromino_Long";
                break;
            case 3:
                activeTetrominoName = "Prefabs/Tetromino_Square";
                break;
            case 4:
                activeTetrominoName = "Prefabs/Tetromino_J";
                break;
            case 5:
                activeTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 6:
                activeTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 7:
                activeTetrominoName = "Prefabs/Tetromino_Z";
                break;
        }

        return activeTetrominoName;
    }

    /*
     * Calculate score
     */
     int CalculateScore()
    {
        score = score + (full_lines * consecutivectr);
        ScoreValue.text = score.ToString().PadLeft(6,'0');
        LineValue.text = total_lines.ToString().PadLeft(3,'0');


        Debug.Log(level_up_ctr);
        // level up every 15 lines 
        if ((level_up_ctr >= 15) && (total_lines > 0)) {
            level_up_ctr = 0;
            LevelUp();
        }
        return score;
    }

    /*
     * Level Up and decrease falling speed (faster) by 50%
     */
    void LevelUp ()
    {
        level++;
        Debug.Log("Level: " + level);
        LevelValue.text = level.ToString().PadLeft(3, '0');
        float falling_speed_temp = FindObjectOfType<Tetromino>().GetFallSpeed();
        falling_speed_temp = falling_speed_temp - (falling_speed_temp * 0.5f);
        FindObjectOfType<Tetromino>().SetFallSpeed(falling_speed_temp);
    }

    public void GameOver ()
    {
        nextTetromino.GetComponent<Tetromino>().enabled = false;
        FindObjectOfType<DataHolder>().SetScoreData(score, level, total_lines);
        SceneManager.LoadScene("GameOver");
    }
}
