using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//-----------------------------------------------------------
// Scripts\GameLogic\GameManager.cs
//
// 싱글톤으로 구현된 GameManager 클래스입니다.
// 게임의 전체적인 흐름을 제어하는 함수들을 가지고 있습니다.
// 게임매니저 인스턴스를 사용하여 함수를 호출하십시오.
//-----------------------------------------------------------
public class GameManager : MonoBehaviour
{
    #region C# properties
    //-----------------------------------------------------------
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion

    #region variables
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static GameManager _instance;
    #endregion

    //-----------------------------------------------------------
    #region methods
    private void Awake()
    {
        // 인스턴스가 존재하지 않는 경우, 이 객체를 인스턴스로 만든다.
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "02_Tutorial")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        EventManager.Instance.AddListener(EVENT_TYPE.GAME_END, OnEvent);
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, OnEvent);
    }
    //-------------------------------------------------------
    //Called when events happen
    protected virtual void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GAME_END:

                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                GameObject cp = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject;

                if (Player != null)
                    Destroy(Player);
                if (cp != null)
                    Destroy(cp);

                LoadingSceneManager.LoadScene("05_Ending");
                break;
            case EVENT_TYPE.DEAD:

                break;
            default:
                break;
        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
            LoadingSceneManager.LoadScene("03_Stage01");
        if (Input.GetKeyDown(KeyCode.F3))
            LoadingSceneManager.LoadScene("04_Stage02");
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (LevelManager.instance != null)
            {
                LevelManager.instance.LevelUp();
            }
        }
    }

    public void StartNewGame()
    {
        LoadingSceneManager.LoadScene("02_Tutorial");

        if (UIManager.Instance != null)
            UIManager.Instance.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        GameObject um = UIManager.Instance.gameObject;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        GameObject cp = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject;
        GameObject uc = GameObject.FindGameObjectWithTag("UICamera");

        if (um != null)
        {
            um.SetActive(false);
        }
        if (Player != null)
            Destroy(Player);
        if (cp != null)
            Destroy(cp);
        if (uc != null)
            Destroy(uc);

        LoadingSceneManager.LoadScene("01_MainMenu");
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    //-----------------------------------------------------------
    #endregion
}
