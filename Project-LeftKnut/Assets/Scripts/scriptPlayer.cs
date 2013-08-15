using UnityEngine;

public class scriptPlayer : MonoBehaviour {
	public Transform Target;
	public Transform Selected;
	public float RayDistance = 100.0f;
    public GameObject Enemy;
    public float FirstEnemySpawnTimer = 15.0f;
    public float EnemyWaveSpawnTimer = 10.0f;

    private float _timeSinceLevelStart = 0.0f;
    private float _timeSinceLastWave = 0.0f;
    private int _currentEnemyWave = 0;
    private int _score = 0;

    void Update()
    {
        var silo = GameObject.FindGameObjectWithTag("silo");

        if (silo)
        {
            HandleUserInput();

            HandleEnemyWaves();            
        }
        else
        {
            // Game over
            print("Game Over!!");
        }
    }
    void OnGUI()
    {
        var silo = GameObject.FindGameObjectWithTag("silo");
        var siloScript = silo.GetComponent<scriptSilo>();

        GUI.Label(new Rect(0, 0, 50, 20), "Score: " + _score);
        GUI.Label(new Rect(0, 30, 50, 20), "Wave: " + _currentEnemyWave);

        if (siloScript)
        {
            GUI.Label(new Rect(0, 60, 100, 20), "Resources: " + siloScript.GetResourceCount());            
        }
    }

    public void IncreaseScore()
    {
        _score++;
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
            _timeSinceLastWave += Time.deltaTime;

            if (_timeSinceLastWave > EnemyWaveSpawnTimer)
            {
                SpawnEnemyWave();
                _currentEnemyWave++;
                _timeSinceLastWave = 0f;
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
        for (int i = 0; i < _currentEnemyWave + 1; i++)
        {
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        Vector3 spawnNodePosition;
        var topSpawnPosition = new Vector3(Random.Range(-100f, 100f), 0f, 175f);
        var rightSpawnPosition = new Vector3(150f, 0, Random.Range(0f, 150f));
        var bottomSpawnPosition = new Vector3(Random.Range(-100f, 100f), 0f, -33f);
        var leftSpawnPosition = new Vector3(-140f, 0f, Random.Range(0f, 150f));
        int enemySpawnNode = Random.Range(1, 4);

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
