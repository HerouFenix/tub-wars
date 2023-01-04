using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _bathroom;
    [SerializeField] private float timer = 5 * 60;

    [SerializeField] GameObject _player;
    private PlayerManager _playerM;

    private GameObject _homeCamera;
    private GameObject _gameCamera;
    [SerializeField] GameObject _homeEffects;
    [SerializeField] GameObject _aboveWaterEffects;
    [SerializeField] GameObject _underWaterEffects;
    [SerializeField] GameObject _freeCamEffects;


    private GameObject _home;
    private GameObject _end;
    private TMP_InputField _tagInput;
    private GameObject _hud;
    private GameObject heart1;
    private GameObject heart2;
    private GameObject heart3;
    private GameObject water;
    private TextMeshProUGUI ballons;

    private TextMeshProUGUI score;
    public GameObject scoreHUD;
    private TextMeshProUGUI finalscore;
    public GameObject finalScoreHUD;

    private int lastHeart = 3;

    private float startTime = 0.0f;
    private bool start = false;
    private bool end = false;

    private float startY = 0.0f;
    private float endY = 51.0f;
    private float currY = 0.0f;


    private Vector3 initialHomeCameraPos = new Vector3(140.800003f, 38.7000008f, 1.16999996f);
    private Quaternion initialHomeCameraRot = new Quaternion(0.0844357237f, -0.505381286f, 0.141513705f, 0.847014964f);

    private int curSettings = 2;
    [SerializeField] Text settings_text_1;
    [SerializeField] Text settings_text_2;

    [SerializeField] ReflectionProbe probe1;
    [SerializeField] ReflectionProbe probe2;

    public AudioClip[] music;
    public AudioClip[] sfx;

    bool _freeCam;

    private void Awake()
    {
        _playerM = _player.GetComponent<PlayerManager>();

        _homeCamera = GameObject.Find("HomeCamera");
        _gameCamera = GameObject.Find("MainCamera");

        _home = GameObject.Find("Home");
        _end = GameObject.Find("End");
        _hud = GameObject.Find("Hud");
        _tagInput = _home.GetComponentInChildren<TMP_InputField>();
        heart1 = GameObject.Find("heart1");
        heart2 = GameObject.Find("heart2");
        heart3 = GameObject.Find("heart3");
        ballons = _hud.transform.Find("baloon").GetComponent<TextMeshProUGUI>();

        water = GameObject.Find("Water");
        score = scoreHUD.GetComponent<TextMeshProUGUI>();
        finalscore = finalScoreHUD.GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusicWithFade(music[0], 0.2f, 0.5f);
        _gameCamera.SetActive(false);

        _homeCamera.SetActive(true);
        _homeEffects.SetActive(true);
        _freeCamEffects.SetActive(false);
        _end.SetActive(false);
        _hud.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        //_bathroom = GameObject.Find("Bathroom");
        startY = _bathroom.transform.position.y;
        currY = startY;

        GetComponentInChildren<EnemySpawner>().spawnDisplayEnemies();

    }

    public void ChangeSettings()
    {
        switch (curSettings)
        {
            case 1:
                curSettings = 2;
                settings_text_1.text = "MEDIUM\nSETTINGS";
                settings_text_2.text = "MEDIUM\nSETTINGS";
                probe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.IndividualFaces;
                probe1.resolution = 1024;

                probe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.IndividualFaces;
                probe2.resolution = 1024;
                break;
            case 2:
                curSettings = 3;
                settings_text_1.text = "HIGH\nSETTINGS";
                settings_text_2.text = "HIGH\nSETTINGS";

                probe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
                probe1.resolution = 2048;

                probe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
                probe2.resolution = 2048;
                break;
            case 3:
                curSettings = 1;
                settings_text_1.text = "LOW\nSETTINGS";
                settings_text_2.text = "LOW\nSETTINGS";

                probe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                probe1.resolution = 256;

                probe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                probe2.resolution = 256;
                break;
        }
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void SwapToFreeCam()
    {
        // swap rotation, allow movement, disable hud
        AudioManager.Instance.PlayMusicWithCrossFade(music[2], 1f, 2.0f);
        _home.SetActive(false);
        _homeCamera.transform.rotation = new Quaternion(0f, -0.5f, 0f, 0.866025388f);
        _freeCam = true;
        _freeCamEffects.SetActive(true);
        _homeEffects.SetActive(false);
        _aboveWaterEffects.SetActive(false);
        _underWaterEffects.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        _homeCamera.GetComponent<MoveCamera>().ResetFocus();
        _homeCamera.GetComponent<MoveCamera>().camEnabled = true;
        _player.GetComponent<PlayerMovement>().canMove = false;

    }

    public void GoBackToHomeCamera()
    {
        // If in menu set camera to thingy, enable hud

        _home.SetActive(true);
        AudioManager.Instance.PlayMusicWithFade(music[0], 0.2f, 2.0f);
        _homeCamera.GetComponent<MoveCamera>().camEnabled = false;
        _homeCamera.GetComponent<MoveCamera>().yaw = 0;
        _homeCamera.GetComponent<MoveCamera>().pitch = 0;
        _homeCamera.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        _homeCamera.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);

        _homeCamera.transform.position = initialHomeCameraPos;
        _homeCamera.transform.rotation = initialHomeCameraRot;

        _freeCam = false;
        _freeCamEffects.SetActive(false);
        _homeEffects.SetActive(true);
        _aboveWaterEffects.SetActive(true);
        _underWaterEffects.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        */
        if (start && !end)
        {
            _playerM.increaseScore(Time.deltaTime);
            currY = Mathf.Lerp(startY, endY, ((Time.time - startTime) / timer));
            _bathroom.transform.position = new Vector3(_bathroom.transform.position.x, currY, _bathroom.transform.position.z);
            if (currY == endY)
            {
                if(water.activeInHierarchy)
                    water.SetActive(false);
                EndGame();
            }
        }
        else
        {
            if (_freeCam && Input.GetKeyDown(KeyCode.Escape))
            {
                GoBackToHomeCamera();
            }
        }
    }

    public void StartGame()
    {
        GetComponentInChildren<EnemySpawner>().deleteAllEnemies();

        _player.SetActive(true);
        AudioManager.Instance.PlayMusicWithCrossFade(music[1], 0.5f, 2.0f);

        _gameCamera.SetActive(true);
        _gameCamera.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        _gameCamera.transform.GetChild(0).GetComponent<ParticleSystem>().Clear();

        _homeCamera.SetActive(false);
        _homeEffects.SetActive(false);
        start = true;
        _playerM._tag = _tagInput.text;
        _playerM.dead = false;
        _player.GetComponent<PlayerMovement>().canMove = true;

        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
        lastHeart = 3;

        finalscore.text = _playerM.score.ToString("0000");
        _playerM.score = 0.0f;
        SetScore();

        _tagInput.text = "";
        _home.SetActive(false);
        _hud.SetActive(true);
        ballons.text = (_playerM.balloons).ToString();
        Cursor.visible = false;
        startTime = Time.time;
        GetComponentInChildren<EnemySpawner>().active = true;


        _gameCamera.gameObject.GetComponent<WaterParticles>().gameStarted = true;
    }

    public void EndGame()
    {
        _player.SetActive(false);
        GetComponentInChildren<EnemySpawner>().spawnDisplayEnemies();

        AudioManager.Instance.PlayMusicWithFade(music[0], 0.2f, 2.0f);
        end = true;
        _hud.SetActive(false);
        _homeEffects.SetActive(true);
        _end.SetActive(true);
        finalscore.text = _playerM.score.ToString("0000");
        _player.GetComponent<PlayerMovement>().canMove = false;
        _playerM.balloons = 0;
        SetBaloons();

        GameObject scores = _end.gameObject.transform.GetChild(3).gameObject;
        for (int i = 0; i < 10; i++)
        {
            if(PlayerPrefs.GetString("highScoreName_" + (i)) != "")
            {
                scores.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("highScoreName_" + (i)) + " : " + Mathf.RoundToInt(PlayerPrefs.GetFloat("highScore_" + (i)));
            }
        }

        Cursor.visible = true;
        GetComponentInChildren<EnemySpawner>().active = false;
        GetComponentInChildren<EnemySpawner>().initial = true;
        _bathroom.transform.position = new Vector3(_bathroom.transform.position.x, startY, _bathroom.transform.position.z);
        water.SetActive(true);

        _gameCamera.GetComponent<WaterParticles>().gameStarted = false;
        _gameCamera.SetActive(false);
        _homeCamera.SetActive(true);

    }

    public void Replay()
    {
        end = false;
        start = false;
        _home.SetActive(true);
        _end.SetActive(false);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(go);
        }
        _player.transform.position = _playerM.startPos;   
    }

    public void SubHeart()
    {
        if(lastHeart == 3)
        {
            heart3.SetActive(false);
        }
        else if (lastHeart == 2)
        {
            heart2.SetActive(false);
        }
        lastHeart--;
    }

    public void SetScore()
    {
        this.score.text = (_playerM.score).ToString("0000");
    }

    public void SetBaloons()
    {
        ballons.text = (_playerM.balloons).ToString();
    }

}
