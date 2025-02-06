using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    enum Move { Left = 0, Down = 1, Right = 2 }
    readonly Vector3[] directions = { Vector3.left, Vector3.down, Vector3.right };

    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 11;

    [SerializeField] private Invader invaderPrefab = null;

    // Initial bounds in which invaders are spawning.
    [SerializeField] private Vector2 bounds;

    // Difficulty progress depending on enemy left ratio
    [SerializeField] private AnimationCurve difficultyProgress = AnimationCurve.Linear(0, 0, 1, 1);

    // Speed min and max depending on difficulty progress
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;

    // Random shoot rate min and max depending on difficulty progress
    [SerializeField] private Vector2 shootRandomMin = new(3f,5f);
    [SerializeField] private Vector2 shootRandomMax = new(1f, 3f);

    // A cozy time with no alien harm at start of the game. I guess Player shoot first.
    [SerializeField] private float timeBeforeFirstShoot = 5f;

    // Distance moved when moving downward
    [SerializeField] private float downStep = 1f;
    [SerializeField] private float TimeToCross = 5f;
    [SerializeField] private float EndPositionRight = 7.5f;
    [SerializeField] private float EndPositionLeft = -7.5f;
    private bool changeDirection = false;
    private Vector2 BasePosition;
    private float elapsed = 0f;
    private Vector3 direction = Vector3.right;
    float distBtwInvaders = 0;
    float lenghtInvaders = 0;
    float lenghtInvader = 0;
    private bool isFirstSequence = true;
    private float invaderSpacing;
    private int InvaderDeathCount = 0;



    private Bounds Bounds => new Bounds(transform.position, new Vector3(bounds.x, bounds.y, 1000f));

    Move move = Move.Right;
    int moveCount = 0;

    float distance = 0f;

    float shootCooldown;

    struct Column { public int id; public List<Invader> invaders; }
    struct Row { public int id; public List<Invader> invaders; }

    List<Invader> invaders = new();
    List<Column> invaderPerColumn = new(); // Keeps track of invaders per column. A column will be removed if empty.
    List<Row> invaderPerRow = new(); // Keeps track of invaders per row. A row will be removed if empty.

    void Awake()
    {
        DOTween.Init();
        shootCooldown = timeBeforeFirstShoot;

        for (int i = 0; i < columns; i++)
        {
            invaderPerColumn.Add(new() { invaders= new() });
        }
        for (int i = 0; i < rows; i++)
        {
            invaderPerRow.Add(new() { invaders = new() });
        }

        // Spaw the invader grid
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Invader invader = GameObject.Instantiate<Invader>(invaderPrefab, GetPosition(i, j), Quaternion.identity, transform);
                invader.Initialize(new Vector2Int(i, j));
                invader.onDestroy += RemoveInvader;
                invader.BasePosition = invader.transform.position;
                invaders.Add(invader);
                invaderPerColumn[i].invaders.Add(invader);
                invaderPerRow[j].invaders.Add(invader);
            }
        }
        lenghtInvaders = invaders[0].transform.position.x - invaders[^1].transform.position.x;
        distBtwInvaders = invaders[0].transform.position.x - invaders[1].transform.position.x;
        lenghtInvader = invaders[0].GetComponent<Renderer>().bounds.size.x;
        if (columns > 1)
            invaderSpacing = Mathf.Abs(invaders[1].transform.position.x - invaders[0].transform.position.x);
    }

   

    void Update()
    {
        UpdateMovement();
        UpdateShoot();
        
    }

    private void UpdateShoot()
    {
        shootCooldown -= Time.deltaTime;
        if (shootCooldown > 0) { return; }

        // Shoot rate depends on remaining invaders ratio
        float t = 1f - (invaders.Count - 1) / (float)((rows * columns) - 1);
        Vector2 shootRandom = Vector2.Lerp(shootRandomMin, shootRandomMax, difficultyProgress.Evaluate(t));

        // One column is selected to shoot a bullet. Only the invader at the bottom of that column can shoot.
        int columnIndex = Random.Range(0, invaderPerColumn.Count);
        invaderPerColumn[columnIndex].invaders[0].Shoot();

        shootCooldown += Random.Range(shootRandom.x, shootRandom.y);
    }


    void UpdateMovement()
    {
        if (invaders.Count <= 0)
        {
            return;
        }

        float time = 1f - (invaders.Count - 1) / (float)((rows * columns) - 1);
        float speed = Mathf.Lerp(speedMin, speedMax, difficultyProgress.Evaluate(time));

        Vector3 downDirection = Vector3.down;
        if (changeDirection)
        {
            if (BasePosition == Vector2.zero)
            {
                BasePosition = invaders[0].transform.position + downDirection;
            }

            foreach (var invader in invaders)
            {
                if(invader.transform.position.y <= -3.9f)
                {
                    EventsManager.Instance.OnGameOver.Invoke();

                }
                if (invaders[0].transform.position.y < BasePosition.y)
                {
                    changeDirection = false;
                    move = move == Move.Right ? Move.Left : Move.Right;
                    BasePosition = Vector2.zero;
                    float bottom = GetRowPosition(invaderPerRow[0].id);
                    invader.BasePosition = invader.transform.position;
                    if (GameManager.Instance.IsBelowGameOver(bottom))
                    {
                        GameManager.Instance.PlayGameOver();
                    }

                    isFirstSequence = true;

                    break;

                }

                invader.transform.position += downDirection * speed * Time.deltaTime;

            }

            return;
        }

        // Speed depends on remaining invaders ratio
        if (!isFirstSequence)
            return;
        isFirstSequence = false;
        if (invaders.Count <= 0) return;
        //distBtwInvaders = 0.2f;

        if (direction == Vector3.left)
        {
            Sequence globalSequence = DOTween.Sequence();
            int iLeft = 0;
            for (int col = 0; col < columns; col++)
            {
                MovementLateral(iLeft, col, globalSequence);
                iLeft++;
            }
        }
        else
        {
            Sequence globalSequence = DOTween.Sequence();
            int iRight = 0;
            for (int col = columns - 1; col >= 0; col--)
            {
                MovementLateral(iRight, col, globalSequence);
                iRight++;
            }
        }
    }



    void MovementLateral(int i, int col, Sequence globalSequence)
    {
        List<Invader> column = invaderPerColumn[i].invaders;
        float columnDelay = col * 0.1f; 
        foreach (var invader in column)
        {
                
            float finalXPosition;
                
            if(direction == Vector3.right)
                finalXPosition = EndPositionRight - (col);
            else
            {
                finalXPosition = EndPositionLeft +  (col);
            }
            // Déplacer chaque invader en respectant l'espacement initial
            globalSequence.Insert(columnDelay, 
                invader.transform.DOMoveX(finalXPosition, TimeToCross)
                    .SetEase(Ease.OutCirc)
            );
        }

    globalSequence.OnComplete(() =>
    {
        Debug.Log(direction);
        if(direction == Vector3.right)
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.right;
        }
        changeDirection = true;
            
    });
    }
    

    /// <summary>
    /// Removing an invader from the wave will remove it from "invaders", "invaderPerColumn" and "invaderPerRow". If a column or a row is empty, it will be removed.
    /// </summary>
    void RemoveInvader(Invader invader)
    {
        EventsManager.Instance.OnEnnemyKilled.Invoke();
        invaders.Remove(invader);

        int indexColumn = invaderPerColumn.FindIndex(x => x.id == invader.GridIndex.x);
        if(indexColumn != -1)
        {
            Column column = invaderPerColumn[indexColumn];
            column.invaders.Remove(invader);
            if (column.invaders.Count <= 0)
            {
                invaderPerColumn.RemoveAt(indexColumn);
            }
            else
            {
                invaderPerColumn[indexColumn] = column;
            }
        }

        int indexRow = invaderPerRow.FindIndex(x => x.id == invader.GridIndex.y);
        if (indexRow != -1)
        {
            Row row = invaderPerRow[indexRow];
            row.invaders.Remove(invader);
            if (row.invaders.Count <= 0)
            {
                invaderPerRow.RemoveAt(indexRow);
            }
            else
            {
                invaderPerRow[indexRow] = row;
            }
        }
    }

    // Get position of an invader in the bounding box according to it's index
    Vector3 GetPosition(int i, int j)
    {
        return new Vector3( GetColumnPosition(i), GetRowPosition(j), 0f );
    }

    // Get position of an invader in the bounding box according to it's column index
    float GetColumnPosition(int column)
    {
        return Mathf.Lerp(Bounds.min.x, Bounds.max.x, column / (float)(columns - 1));
    }

    // Get position of an invader in the bounding box according to it's row index
    float GetRowPosition(int row)
    {
        return Mathf.Lerp(Bounds.min.y, Bounds.max.y, row / (float)(rows - 1));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(bounds.x, bounds.y, 0f));
    }

    public void addInvaderDeath()
    {
        InvaderDeathCount++;
        if(InvaderDeathCount == rows * columns)
        {
            EventsManager.Instance.OnGameWin.Invoke();
            
        }
    }
}
