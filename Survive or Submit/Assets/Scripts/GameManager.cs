using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] AudioSource _ambientEnvironment;
    [SerializeField] AudioSource _ambientRain;
    [SerializeField] GameObject _mapLoader;
    [SerializeField] CanvasController _canvasController;

    PerlinNoiseGenerator _perlinNoiseGenerator;
    ObstacleSpawner _obstacleSpawner;
    public int startingScrap;
    public int startingMeds;
    public int startingExperienceCap;

    bool _perlinNoiseFinished;
    bool _obstacleSpawnerFinished;

    // Start is called before the first frame update
    void Start()
    {
        Logger logger = new Logger(@"C:C:\Users\Daniel Kareken\Desktop\MyLog.log");
        logger.WriteLine("Hello World");

        //initialize RuntimeData vars
        _runtimeData.player_scrap = startingScrap;
        _runtimeData.player_meds = startingMeds;
        _runtimeData.playerInSpawn = true;
        _runtimeData.player_level = 0;
        _runtimeData.player_experience = 0;
        _runtimeData.player_exp_to_lvl_up = startingExperienceCap;
        _runtimeData.currZone = 1;
        _runtimeData.gameOver = false;
        _runtimeData.game_paused = true;

        _perlinNoiseGenerator = _mapLoader.GetComponent<PerlinNoiseGenerator>();
        _obstacleSpawner = _mapLoader.GetComponent<ObstacleSpawner>();   
    }

    public void StartGame()
    {
        //load map
        LoadMap();

        //play sounds
        if (_ambientEnvironment)
            _ambientEnvironment.Play();
        if (_ambientRain)
            _ambientRain.Play();
    }

    public void OnPerlinNoiseFinished()
    {
        _perlinNoiseFinished = true;
        if(_obstacleSpawnerFinished)
            _runtimeData.game_paused = false;
    }

    public void OnObstacleSpawnerFinished()
    {
        _obstacleSpawnerFinished = true;
        if(_perlinNoiseFinished)
            _runtimeData.game_paused = false;
    }

    void LoadMap()
    {
        _canvasController.DisplayLoadingUI();
        _perlinNoiseGenerator.StartPerlinNoise();
        _obstacleSpawner.StartSpawningObstacles();
    }
}
