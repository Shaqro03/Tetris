using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Board previewPiece { get; private set; }

    int arandom;
    TetrominoData adata;
    int prandom;
    TetrominoData pdata;
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    Vector3Int previewspawnPosition = new Vector3Int(10, 6, 0);

    public int scoreOneLine = 40;
    public int scoreTwoLine = 100;
    public int scoreThreeLine = 300;
    public int scoreFourLine = 1200;

    public Canvas classicHud;
    public Text hudScore;
    public static int numberOfRowsThisTurn = 0;
    public static int currentScore = 0;


    public Canvas pauseCanves;
    public static bool isPaused = false;
    public bool gamestart = false;

    public Canvas arcadeHud;
    public Text hudLevel;
    public static int level;
    public TileBase levTile;
    public static int rowleft;



    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }
    private void Start()
    {
        SpawnPiece();
        if (level != 0)
        {
            arcadeHud.enabled = true;
            FillLine(level);
        }
        else
            classicHud.enabled = true;
    }
    void Update()
    {
        CheckPause();

        UpdateScore();

        UpdateUI();
    }

    public void CheckPause() 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 1) 
            {
                Time.timeScale = 0;
                isPaused = true;
                pauseCanves.enabled = true;
            }
            else 
            {
                Time.timeScale = 1;
                isPaused = false;
                pauseCanves.enabled = false;
            }
        }
    }
    public void UpdateUI() 
    {
        hudLevel.text = level.ToString();
        hudScore.text = currentScore.ToString();
    }
    public void UpdateScore() 
    {
        switch (numberOfRowsThisTurn) 
        {
            case 1:
                currentScore += scoreOneLine;
                numberOfRowsThisTurn = 0;
                break;
            case 2:
                currentScore += scoreTwoLine;
                numberOfRowsThisTurn = 0;
                break;
            case 3:
                currentScore += scoreThreeLine;
                numberOfRowsThisTurn = 0;
                break;
            case 4:
                currentScore += scoreFourLine;
                numberOfRowsThisTurn = 0;
                break;
            default:
                break;
        }
    }

    public void SpawnPiece()
    {
        
        if (gamestart == false)
        {
            this.arandom = Random.Range(0, tetrominoes.Length);
            this.adata = tetrominoes[this.arandom];
            this.activePiece.Initialize(this, spawnPosition, this.adata);

            this.prandom = Random.Range(0, tetrominoes.Length);
            this.pdata = tetrominoes[this.prandom];
            OutPreviw(pdata, 1);


            gamestart = true;
        }
        else
        {
            OutPreviw(pdata, 0);
            this.activePiece.Initialize(this, spawnPosition, this.pdata);
            this.prandom = Random.Range(0, tetrominoes.Length);
            this.pdata = tetrominoes[this.prandom];
            OutPreviw(pdata, 1);
        }

        if (IsValidPosition(this.activePiece, spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }

    public void OutPreviw(TetrominoData data, int state) 
    {
        Vector3Int[] matrix = new Vector3Int[4];

        for (int i = 0; i < 4; i++)
        {
            matrix[i] = (Vector3Int)data.cells[i];
        }

        for (int i = 0; i < 4; i++)
        {
            Vector3Int tilePosition = matrix[i] + this.previewspawnPosition;
            if(state == 1)
                this.tilemap.SetTile(tilePosition, data.tile);
            else
                this.tilemap.SetTile(tilePosition, null);
        }

    }

    public void GameOver()
    {
        SceneManager.LoadScene("Gameover");
    }
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }
    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                if (row <= rowleft - 11) 
                {
                    rowleft--;
                    if (rowleft == 0) 
                    {
                        level++;
                        rowleft = level;
                        tilemap.ClearAllTiles();
                        FillLine(level);
                        return;
                    }
                }
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }
    public bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        numberOfRowsThisTurn++;
        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    public void FillLine(int level) 
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int blocks;
        do
        {
            blocks = 0;
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                if (Random.Range(0, 2) == 1 && blocks != 9)
                {
                    Vector3Int position = new Vector3Int(col, row, 0);
                    this.tilemap.SetTile(position, levTile);
                    blocks++;
                }
            }
            if(blocks == 0) 
            {
                Vector3Int position = new Vector3Int(1, row, 0);
                this.tilemap.SetTile(position, levTile);
            }
            row++;
            level--;
        }
        while (level > 0);
    }
}
