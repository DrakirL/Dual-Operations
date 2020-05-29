// Manager for the minigame in Dual Operations
// Every piece needs a "Piece" tag
// Only works when lower left piece is set on position (0,0)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    
    [System.Serializable]
    public class Puzzle
    {
        public int width;
        public int height;
        public Vector2Int startCoords, endCoords;
        public Vector2[] firewallCoords;
        public Piece[,] pieces;
    }
    public Puzzle puzzle;
    public GameObject[] minigames;
    HackerButton hb;

    GameObject canvas;
    [Tooltip("Shuffle pieces when game start/restart")]
    [SerializeField] bool shuffle;
    //[Tooltip("Activate the electrical current starting from the start connection")]
    //[SerializeField] bool activePulse;
    [Tooltip("Time it takes to reset/shuffle after game is won")]
    [SerializeField] float resetTime;
    [Tooltip("Value which is added to the alert meter when out of time")]
    [SerializeField] float alertPenaltyValue;
    [Space(10)]

    [Header("References")]
    public Button button;
    public Timer timer;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    bool[,] visited;
    bool firewall;   
    bool input = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //InitializePuzzle();      

        //FindFirewalls();

        //SetSpecialPieceCoords();

        //if (shuffle)
        //   ShuffleBoard();

        //if (activePulse)
        //CheckNeighbours(puzzle.startCoords.x, puzzle.startCoords.y);
    }

    private void Update()
    {
        if (input)
        {
           /*if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                UpdateBoard();              
            }*/

            Lose();
        }       
    }

    void UpdateBoard()
    {
        // Sound pls
        DualOperationsAudioPlayer.Instance.Hack(2);

        // Update on mouseclick

        RefreshPuzzle();

        // if (activePulse)
         CheckNeighbours(puzzle.startCoords.x, puzzle.startCoords.y);

         CheckFirewalls();       
    }

    public void GetPuzzle(Piece[,] p)
    {
        InitializePuzzle(p);

        FindFirewalls();

        SetSpecialPieceCoords();

        if (shuffle)
            ShuffleBoard();
    }

    // Set puzzle dimensions and give each piece a coordinate
    void InitializePuzzle(Piece[,] p)
    {
        Vector2 dimensions = GetDimensions();
        puzzle.width = (int)dimensions.x;
        puzzle.height = (int)dimensions.y;
        puzzle.pieces = new Piece[puzzle.width, puzzle.height];
        visited = new bool[puzzle.width, puzzle.height];

        puzzle.pieces = p;

        List<Piece> lp = new List<Piece>();
        bool isRepeated = false;

        for (int i = 0; i < puzzle.width; i++)
        {
            for (int j = 0; j < puzzle.height; j++)
            {
                if (lp.Contains(puzzle.pieces[i, j]))
                    isRepeated = true;
                lp.Add(puzzle.pieces[i, j]);               
            }           
        }
        Debug.Log(isRepeated);

        /*foreach (var piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            puzzle.pieces[(int)piece.transform.localPosition.x, (int)piece.transform.localPosition.y] = piece.GetComponent<Piece>();
        }       */
    }

    // Get the board dimensions
    Vector2 GetDimensions()
    {
        Vector2 temp = Vector2.zero;
        foreach (var piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            if (piece.transform.localPosition.x > temp.x)
                temp.x = piece.transform.localPosition.x;
            if (piece.transform.localPosition.y > temp.y)
                temp.y = piece.transform.localPosition.y;
        }
        temp.x++;
        temp.y++;

        return temp;
    }

    // Initialize firewall array
    void FindFirewalls()
    {
        int counter = 0;
        foreach (var piece in puzzle.pieces)
        {
            if (piece.firewall)
                counter++;
        }
        puzzle.firewallCoords = new Vector2[counter];

    }

    // Set coords on every piece on the board
    void SetSpecialPieceCoords()
    {
        int firewallIndex = 0;

        for (int i = 0; i < puzzle.width; i++)
        {
            for (int j = 0; j < puzzle.height; j++)
            {
                if (puzzle.pieces[i, j].startConnector)
                {
                    puzzle.pieces[i, j].active = true;
                    puzzle.startCoords.x = (int)puzzle.pieces[i, j].transform.localPosition.x;
                    puzzle.startCoords.y = (int)puzzle.pieces[i, j].transform.localPosition.y;
                }
                if (puzzle.pieces[i, j].endConnector)
                {
                    puzzle.endCoords.x = (int)puzzle.pieces[i, j].transform.localPosition.x;
                    puzzle.endCoords.y = (int)puzzle.pieces[i, j].transform.localPosition.y;
                }
                if (puzzle.pieces[i, j].firewall)
                {
                    puzzle.firewallCoords[firewallIndex].x = (int)puzzle.pieces[i, j].transform.localPosition.x;
                    puzzle.firewallCoords[firewallIndex].y = (int)puzzle.pieces[i, j].transform.localPosition.y;
                    firewallIndex++;
                }
            }
        }
    }


    // Rotate all pieces 0-360 degrees
    void ShuffleBoard()
    {
        for (int i = 0; i < puzzle.width; i++)
        {
            for (int j = 0; j < puzzle.height; j++)
            {
                int r = Random.Range(0, 4);

                for (int k = 0; k < r; k++)
                {
                    puzzle.pieces[i, j].RotateRight();
                }
            }
        }
    }

    void CheckForFirewallNeighbours(int w, int h)
    {
        if (puzzle.pieces[w, h].endConnector)
            return;
        if (puzzle.pieces[w, h].firewall)
        {
            //firewall = true;
            //if(activePulse)
            AlertMeter._instance.AddAlert(alertPenaltyValue);
            anim.Play("LoseAnimation");

            // Sound pls
            DualOperationsAudioPlayer.Instance.Hack(0);
            return;
        }
        else
        {
            CheckNeighbours(w, h);
        }
            
    }

    // Checks for neighbour pieces in every direction (not diagonally) recursively
    void CheckNeighbours(int w, int h)
    {
        // Set current piece to visited
        visited[w, h] = true;

        // Set startpiece to active since it deactivates on mouse-click
        puzzle.pieces[puzzle.startCoords.x, puzzle.startCoords.y].active = true;

        // Check if current piece is active
        if (puzzle.pieces[w, h].active)
        {
            // Check Up
            if (h < puzzle.height - 1 && !visited[w, h + 1])
            {
                if (puzzle.pieces[w, h].values[0] == 1 && puzzle.pieces[w, h + 1].values[2] == 1)
                {
                    puzzle.pieces[w, h + 1].active = true;                   
                    CheckForFirewallNeighbours(w, h + 1);
                }
            }

            // Check Right
            if (w < puzzle.width - 1 && !visited[w + 1, h])
            {
                if (puzzle.pieces[w, h].values[1] == 1 && puzzle.pieces[w + 1, h].values[3] == 1)
                {
                    puzzle.pieces[w + 1, h].active = true;
                    CheckForFirewallNeighbours(w + 1, h);
                }
            }

            // Check Down
            if (h > 0 && !visited[w, h - 1])
            {
                if (puzzle.pieces[w, h].values[2] == 1 && puzzle.pieces[w, h - 1].values[0] == 1)
                {
                    puzzle.pieces[w, h - 1].active = true;
                    CheckForFirewallNeighbours(w, h - 1);
                }
            }

            // Check Left
            if (w > 0 && !visited[w - 1, h])
            {
                if (puzzle.pieces[w, h].values[3] == 1 && puzzle.pieces[w - 1, h].values[1] == 1)
                {
                    puzzle.pieces[w - 1, h].active = true;
                    CheckForFirewallNeighbours(w - 1, h);
                }
            }
        }
        else return;
    }

    void ResetPositions()
    {
        foreach(Piece p in puzzle.pieces)
        {
            p.GetStartPos();
        }
    }

    void RefreshPuzzle() {
        firewall = false;
        // Initialize all visited/firewalls to false
        for (int i = 0; i < puzzle.width; i++)
        {
            for (int j = 0; j < puzzle.height; j++)
            {
                visited[i, j] = false;
                puzzle.pieces[i, j].active = false;
            }
        }   
    }

    // Check if firewalls are touched and set alert meter state accordingly
    void CheckFirewalls()
    {
        if (firewall)
            AlertMeter._instance.SetDetected(true);
        else
            AlertMeter._instance.SetDetected(false);
    }

    bool WinCondition() => !firewall && puzzle.pieces[puzzle.endCoords.x, puzzle.endCoords.y].active ? true : false;

    public void Win()
    {
        // Animation holder
        anim.Play("WinAnimation");

        // Sound pls
        DualOperationsAudioPlayer.Instance.Hack(3);

        Deactivate();     
    }

    void Lose()
    {
        if(timer != null)
        {
            if (timer.IsDepleted())
            {
                AlertMeter._instance.AddAlert(alertPenaltyValue);
                // Animation holder
                anim.Play("LoseAnimation");

                // Sound pls
                DualOperationsAudioPlayer.Instance.Hack(1);

                // Remove if no reset after win
                StartCoroutine(Reset(resetTime));
                Debug.Log("Batsoup dinner :()");
            }
        }      
    }

    // Toggle the pulse and update the board
    public void Pulse()
    {
        if (input) // input manager?
        {
            //activePulse = true;
            UpdateBoard();
            if (WinCondition())
            {
                Win();
            }
            //StartCoroutine(Reset(resetTime));
        }
    }

    // Temporary input management
    void EnableInput(bool boolToSet)
    {
        foreach(var piece in puzzle.pieces)
        {
            piece.input = boolToSet;
            input = boolToSet;
        }
    }
    
    // May be used when there are animations
    IEnumerator Test(int w, int h)
    {
        // -- Change to animation time when there are animations
        yield return new WaitForSeconds(.2f);
    }

    IEnumerator Reset(float time)
    {
        EnableInput(false);
        button.interactable = false;
        timer.TogglePause();
        //firewall = false;        
        yield return new WaitForSeconds(time);
        //activePulse = false;
        RefreshPuzzle();
        CheckFirewalls();
        if(shuffle)
        ShuffleBoard();
        else
        ResetPositions();       
        timer.ResetTimer();
        timer.TogglePause();
        button.interactable = true;
        EnableInput(true);     
    }
    GameObject o;
    public void Activate(HackerButton hb, GameObject o)
    {
        this.o = o;
        this.hb = hb;
        o.SetActive(true);
        button.gameObject.SetActive(true);
        canvas.SetActive(false);
        GetPuzzle(o.GetComponent<Minigame>().pieces);       
    }
    public void Deactivate()
    {
        hb.hack();
        o.SetActive(false);
        button.gameObject.SetActive(false);
        canvas.SetActive(true);
        timer.ResetTimer();
    }
    public void GetHackerCanvas(GameObject o)
    {
        canvas = o;
    }
}
