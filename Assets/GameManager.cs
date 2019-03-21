using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    int goal;
    int counter;
    public Text victoryText;
    public Text controlText;
    public Text controlTip;
    public Text mode;
    public Text levelSelect;
    public Button lv1Button;
    public Button lv2Button;
    public Button lv3Button;
    public Button lv4Button;
    public Button lv5Button;
    public Button modeButton;
    bool cDisplay = true;
    bool paused = false;
    public string chosenScene;
    public bool line = true;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Time.fixedDeltaTime = 1 / 90f;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectsWithTag("Target").Length;
        Debug.Log("goal = " + goal);
        counter = 0;
        victoryText.enabled = false;
        controlTip.enabled = false;
        Invoke("displayControls", 8);
        lv1Button.onClick.AddListener(delegate { useButton(lv1Button.GetComponentInChildren<Text>().text); });
        lv2Button.onClick.AddListener(delegate { useButton(lv2Button.GetComponentInChildren<Text>().text); });
        lv3Button.onClick.AddListener(delegate { useButton(lv3Button.GetComponentInChildren<Text>().text); });
        lv4Button.onClick.AddListener(delegate { useButton(lv4Button.GetComponentInChildren<Text>().text); });
        lv5Button.onClick.AddListener(delegate { useButton(lv5Button.GetComponentInChildren<Text>().text); });
        modeButton.onClick.AddListener(toggleMode);
        toggleButtons(paused);
    }

    void useButton(string scene)
    {
        chosenScene = scene;
        chooseScene();
    }

    void toggleMode()
    {
        line = !line;
        if (line == true)
            modeButton.GetComponentInChildren<Text>().text = "Easy";
        if (line == false)
            modeButton.GetComponentInChildren<Text>().text = "Hard";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        goal = GameObject.FindGameObjectsWithTag("Target").Length;
        Debug.Log("goal = " + goal);
        counter = 0;
        victoryText.enabled = false;
        if (paused == true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void displayControls()
    {
        controlText.enabled = false;
        controlTip.enabled = true;
        cDisplay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused == false)
            {
                paused = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else if (paused == true)
            {
                paused = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            toggleButtons(paused);
        }
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        if (Input.GetKeyDown(KeyCode.C) && cDisplay == false && paused == false)
        {
            controlTip.enabled = false;
            controlText.enabled = true;
            cDisplay = true;
            Invoke("displayControls", 8);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && paused == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void targetHit()
    {
        counter++;
        if (counter >= goal)
            endLevel();
        Debug.Log("counter = " + counter);


    }
    public void endLevel()
    {
        victoryText.enabled = true;
        Debug.Log("Got Eem");
        Invoke("moveOn", 3.5f);
    }

    public void moveOn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void chooseScene()
    {
        SceneManager.LoadScene(chosenScene, LoadSceneMode.Single);
    }

    void toggleButtons(bool input)
    {
        lv1Button.gameObject.SetActive(input);
        lv2Button.gameObject.SetActive(input);
        lv3Button.gameObject.SetActive(input);
        lv4Button.gameObject.SetActive(input);
        lv5Button.gameObject.SetActive(input);
        modeButton.gameObject.SetActive(input);
        levelSelect.enabled = input;
        mode.enabled = input;
    }
}

