using UnityEngine;

public class scriptPlayer : MonoBehaviour {
	public Transform Target;
	public Transform Selected;
	public float RayDistance = 100.0f;
    public GameObject Enemy;
    public float FirstEnemySpawnTimer = 15.0f;
    public float EnemyWaveSpawnTimer = 10.0f;
    public AudioClip WaveSpawnNotification;
    public bool GameOver;

    private float _timeSinceLevelStart;
    private float _timeSinceLastWave;
    private int _currentEnemyWave;
    private int _currentEnemyCount;
    private int _score;
    private int _spawnA;
    private int _spawnB = 1;

    void Update()
    {
        var silo = GameObject.FindGameObjectWithTag("silo");

        if (!GameOver)
        {
            if (silo)
            {
                HandleUserInput();
                UpdateEnemyCount();
                HandleEnemyWaves();
            }
            else
            {
                GameOver = true;
                Time.timeScale = 0;
                GameObject.FindGameObjectWithTag("MainCamera").audio.Stop();
            }
        }
    }
    void OnGUI()
    {
        DisplayResources();

        const float labelWidth = 150f;
        const float halfLabelWidth = labelWidth/2f;
        float halfScreenWidth = Screen.width/2f;
        float halfScreenHeight = Screen.height/2f;

        GUI.Label(new Rect(0, 0, 100, 20), "Score: " + _score);
        GUI.Label(new Rect(0, 30, 100, 20), "Wave: " + _currentEnemyWave);

        if (!GameOver)
        {
            DisplayWaveDetails(labelWidth, halfScreenWidth, halfLabelWidth);

        }
        else
        {
            float gameOverWindowWidth = 400f;
            float halfGameOverWindowWidth = gameOverWindowWidth/2;
            float gameOverWindowHeight = 200f;
            float halfGameoverWindowHeight = gameOverWindowHeight/2;

            GUI.BeginGroup(new Rect(halfScreenWidth-halfGameOverWindowWidth, halfScreenHeight-halfGameoverWindowHeight, gameOverWindowWidth, gameOverWindowHeight));
            GUI.Box(new Rect(0, 0, gameOverWindowWidth, gameOverWindowHeight), "Game Over");
            GUI.Label(new Rect(10, 40, 300f, 20f), "Number of waves completed: " + (_currentEnemyWave - 1));
            GUI.Label(new Rect(10, 60, 300f, 20f), "Number of enemies killed: " + _score);
            if (GUI.Button(new Rect(50, 150, 100, 30), "Play Again"))
            {
                Application.LoadLevel("MainMenu");
                Application.LoadLevel("Level");
            }
            if (GUI.Button(new Rect(gameOverWindowWidth-150, 150, 100, 30), "Main Menu"))
            {
                Application.LoadLevel("MainMenu");
            }
            GUI.EndGroup();
            if (!GameObject.FindGameObjectWithTag("Player").audio.isPlaying)
            {
                GameObject.FindGameObjectWithTag("Player").audio.Play();
            }
        }
    }
    public void IncreaseScore()
    {
        _score++;
    }

    private void DisplayWaveDetails(float labelWidth, float halfScreenWidth, float halfLabelWidth)
    {
        if (_currentEnemyCount > 0)
        {
            GUI.Label(new Rect(halfScreenWidth - halfLabelWidth, 0, labelWidth, 20),
                      "Enemies Remaining: " + _currentEnemyCount);
        }
        else
        {
            int nextWaveIn = _currentEnemyWave == 0
                                 ? (int) (FirstEnemySpawnTimer - _timeSinceLevelStart)
                                 : (int) (EnemyWaveSpawnTimer - _timeSinceLastWave);

            GUI.Label(new Rect(halfScreenWidth - halfLabelWidth, 0, labelWidth, 20),
                      "Next Wave In: " + nextWaveIn.ToString("D2"));
        }
    }
    private void UpdateEnemyCount()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("enemy");

         _currentEnemyCount = enemyList.GetLength(0);
    }
    private static void DisplayResources()
    {
        var silo = GameObject.FindGameObjectWithTag("silo");

        if (silo)
        {
            var siloScript = silo.GetComponent<scriptSilo>();
            if (siloScript)
            {
                GUI.Label(new Rect(0, 60, 100, 20), "Resources: " + siloScript.GetResourceCount());
            }
        }
    }
    private void HandleEnemyWaves()
    {
        if (_currentEnemyWave == 0)
        {
            _timeSinceLevelStart += Time.deltaTime;

            if (_timeSinceLevelStart > FirstEnemySpawnTimer)
            {
                SpawnEnemyWave();
                _currentEnemyWave++;
            }
        }
        else
        {
            if (_currentEnemyCount <= 0)
            {
                _timeSinceLastWave += Time.deltaTime;

                if (_timeSinceLastWave > EnemyWaveSpawnTimer)
                {
                    SpawnEnemyWave();
                    _currentEnemyWave++;
                    _timeSinceLastWave = 0f;
                }
            }
        }
    }
    private void HandleUserInput()
    {
        // Handle left click
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
        // Handle right click
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void HandleLeftClick()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, RayDistance))
        {
            // Get target
            Target = hit.transform;

            // Deselect current target if new target can select
            if (Selected)
            {
				Debug.Log(Selected);
                Selected.GetComponent<scriptCanSelect>().Deselect();
            }

            // Select new left click target
            SelectedLeftClickTarget();
        }
    }
    private void HandleRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, RayDistance))
        {
            if (Selected)
            {
                SelectHarvestNode(hit);
            }
        }
    }
    private void SelectHarvestNode(RaycastHit hit)
    {
        var harvester = Selected.GetComponent<scriptHarvester>();

        // If it is, assign the selected object as the harvester node target
        if (harvester)
        {
            // Make sure the selected object is a node befoe assigning it as the target node
            var node = hit.transform.GetComponent<scriptNode>();

            if (node)
            {
                harvester.TargetNode = hit.transform;
            }
        }
    }
    private void SelectedLeftClickTarget()
    {
        var selectScript = Target.GetComponent<scriptCanSelect>();

        if (selectScript)
        {
            selectScript.Select();
            Selected = Target;
        }
        else
        {
            //Selected = null;
        }
    }
    private void SpawnEnemyWave()
    {
        if (WaveSpawnNotification)
        {
            audio.PlayOneShot(WaveSpawnNotification);
        }

        int spawnCount = _spawnA + _spawnB;

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }

        _spawnA = _spawnB;
        _spawnB = spawnCount;
    }
    private void SpawnEnemy()
    {
        Vector3 spawnNodePosition;
        var topSpawnPosition = new Vector3(Random.Range(-100f, 100f), 0f, 175f);
        var rightSpawnPosition = new Vector3(150f, 0, Random.Range(0f, 150f));
        var bottomSpawnPosition = new Vector3(Random.Range(-100f, 100f), 0f, -33f);
        var leftSpawnPosition = new Vector3(-140f, 0f, Random.Range(0f, 150f));
        int enemySpawnNode = Random.Range(1, 5);

        switch (enemySpawnNode)
        {
            case 1:
                spawnNodePosition = topSpawnPosition;
                break;
            case 2:
                spawnNodePosition = rightSpawnPosition;
                break;
            case 3:
                spawnNodePosition = bottomSpawnPosition;
                break;
            case 4:
                spawnNodePosition = leftSpawnPosition;
                break;
            default:
                spawnNodePosition = bottomSpawnPosition;
                break;
        }

        Instantiate(Enemy, spawnNodePosition, new Quaternion());
    }
}
