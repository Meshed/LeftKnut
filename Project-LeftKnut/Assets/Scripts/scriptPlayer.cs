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
    private bool _isObjectAttached;
    private GameObject _objectAttaced;

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

                if (_isObjectAttached && Input.GetMouseButtonDown(0))
                {
                    var turretScript = _objectAttaced.transform.GetComponent<TurretControl>();
                    turretScript.PlaceTurret();
                    _isObjectAttached = false;
                    _objectAttaced = null;
                }

                if (_objectAttaced)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        _objectAttaced.transform.position = hit.point;
                    }
                }
            }
            else
            {
                GameOver = true;
                Time.timeScale = 0;
            }
        }
    }
    void OnGUI()
    {
        DisplayResources();

        const float labelWidth = 150f;
        const float halfLabelWidth = labelWidth/2f;
        float halfScreenWidth = Screen.width/2f;

        GUI.Label(new Rect(0, 0, 100, 20), "Score: " + _score);
        GUI.Label(new Rect(0, 30, 100, 20), "Wave: " + _currentEnemyWave);

        DisplayWaveDetails(labelWidth, halfScreenWidth, halfLabelWidth);

        if (GUI.Button(new Rect(0, 100, 100, 50), "Add Turret"))
        {
            _isObjectAttached = true;
            _objectAttaced = (GameObject)Instantiate(Resources.Load("Turret")); ;

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
            if (Selected)
            {
				Debug.Log(Selected);
                Selected.GetComponent<scriptCanSelect>().Deselect();
            }

            Target = hit.transform;
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
            Selected = null;
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
