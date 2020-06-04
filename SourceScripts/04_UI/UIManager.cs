using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    #region C# properties
    //-----------------------------------------------------------
    // 인스턴스에 접근하기 위한 프로퍼티
    public static UIManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion

    private static UIManager _instance;

    private PlayerController pc;

    //-----------------------------------------------------------
    //PlayerBaseInfo
    //-----------------------------------------------------------

    public Image Healthbar;
    public Image Staminabar;
    public TMP_Text LevelText;
    //-----------------------------------------------------------
    //PlayerSkillInfo
    //-----------------------------------------------------------

    public Image EquipedWeapon;

    //-----------------------------------------------------------
    //Inventory
    //-----------------------------------------------------------
    public TMP_Text goldText;
    //-----------------------------------------------------------
    // UI Windows
    //-----------------------------------------------------------
    public GameObject inventory;
    public GameObject playerInfo;
    public GameObject questListWindow;
    public GameObject SlotHolder;
    public GameObject questListContent;
    public GameObject questWindow;
    public GameObject optionWindow;
    public GameObject DeadSceneCurtain;

    [HideInInspector]
    public bool inventoryEnabled;
    [HideInInspector]
    public bool infoEnabled;
    [HideInInspector]
    public bool questListEnabled;
    [HideInInspector]
    public bool questWindowEnabled;
    [HideInInspector]
    public bool optionWindowEnabled;

    public Sprite defaultImage;

    private bool activeUI = true;

    //-----------------------------------------------------------
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
    //-----------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>() as PlayerController;

        EventManager.Instance.AddListener(EVENT_TYPE.PLAYER_HIT, OnEvent);
        EventManager.Instance.AddListener(EVENT_TYPE.UPDATE_UI, OnEvent);
        EventManager.Instance.AddListener(EVENT_TYPE.PLAYER_BASEINFO_UPDATE, OnEvent);
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, OnEvent);

        //inventory = GameObject.FindGameObjectWithTag("Inventory");
        //playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo");
        //questListWindow = GameObject.FindGameObjectWithTag("QuestList");

        SlotHolder = GameObject.FindGameObjectWithTag("SlotHolder");
        questListContent = GameObject.FindGameObjectWithTag("QuestListContent");
        questWindow = GameObject.FindGameObjectWithTag("QuestWindow");
        optionWindow = GameObject.FindGameObjectWithTag("OptionWindow");
        DeadSceneCurtain = GameObject.FindGameObjectWithTag("DeadCurtain");

        inventory.GetComponent<RectTransform>().localPosition = Vector3.zero;
        playerInfo.GetComponent<RectTransform>().localPosition = Vector3.zero;
        questListWindow.GetComponent<RectTransform>().localPosition = Vector3.zero;
        optionWindow.GetComponent<RectTransform>().localPosition = Vector3.zero;

        questWindow.SetActive(false);
        optionWindow.SetActive(false);

        EquipedWeapon.sprite = defaultImage;

    }

    protected virtual void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.PLAYER_HIT:
                OnHealthBarChange();
                break;
            case EVENT_TYPE.UPDATE_UI:
                OnUpdateUI();
                break;
            case EVENT_TYPE.PLAYER_BASEINFO_UPDATE:
                OnHealthBarChange();
                OnStaminaBarChange();
                break;
            case EVENT_TYPE.DEAD:
                activeUI = false;

                inventory.SetActive(false);
                playerInfo.SetActive(false);
                questListWindow.SetActive(false);
                optionWindow.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                DeadSceneCurtain.GetComponent<DeadCurtain>().ShowDeadCurtain();

                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (activeUI)
        {
            //인벤토리는 I키로 열 수 있다.
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryEnabled = !inventoryEnabled;
            }
            if (inventoryEnabled)
                inventory.SetActive(true);
            else
                inventory.SetActive(false);

            //플레이어 정보는 P키로 열 수 있다.
            if (Input.GetKeyDown(KeyCode.P))
            {
                infoEnabled = !infoEnabled;
            }
            if (infoEnabled)
                playerInfo.SetActive(true);
            else
                playerInfo.SetActive(false);

            //퀘스트 창은 Q키로 열 수 있다.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                questListEnabled = !questListEnabled;
            }
            if (questListEnabled)
                questListWindow.SetActive(true);
            else
                questListWindow.SetActive(false);

            //옵션은 ESC키로 열수있다.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionWindowEnabled = !optionWindowEnabled;
            }
            if (optionWindowEnabled)
                optionWindow.SetActive(true);
            else
                optionWindow.SetActive(false);

            //켰을 때 커서
            if (infoEnabled || inventoryEnabled || questListEnabled || questWindowEnabled || optionWindowEnabled)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }


    private void OnHealthBarChange()
    {
        if (pc.MAXHEALTH == 0)
            return;

        Healthbar.fillAmount = (float)pc.HEALTH / (float)pc.MAXHEALTH;
    }
    private void OnStaminaBarChange()
    {
        if (pc.MAXSTAMINA == 0)
            return;
        Staminabar.fillAmount = (float)pc.STAMINA / (float)pc.MAXSTAMINA;
    }

    public void OnUpdateUI()
    {
        ExpAndLevelUpdate();
        goldText.text = Inventory.gold.ToString();

        if (playerInfo != null)
            playerInfo.GetComponent<PlayerInfo>().PlayerStatusUpdate();
        else
        {
            playerInfo = GameObject.FindGameObjectWithTag("PlayerInfo");
            playerInfo.GetComponent<PlayerInfo>().PlayerStatusUpdate();
        }
    }


    private void ExpAndLevelUpdate()
    {
        LevelText.text = LevelManager.instance.LEVEL.ToString();
    }

    public void WeaponImageUpdate(Sprite icon)
    {
        EquipedWeapon.sprite = icon;
        //PlayerEquipedWeapon.sprite = icon;
    }
    public void WeaponImageSetDefalut()
    {
        EquipedWeapon.sprite = defaultImage;
    }

    public void CloseInventory()
    {
        if (inventoryEnabled)
            inventoryEnabled = false;
    }

    public void ClosePlayerInfo()
    {
        if (infoEnabled)
            infoEnabled = false;
    }

    public void CloseQuestListWindow()
    {
        if (questListEnabled)
            questListEnabled = false;
    }

    public void OpenQuestWindow()
    {
        if (questWindowEnabled == false)
            questWindowEnabled = true;
        questWindow.GetComponent<RectTransform>().localPosition = Vector3.zero;
        questWindow.SetActive(true);
    }
    public void CloseQuestWindow()
    {
        if (questWindowEnabled == true)
            questWindowEnabled = false;

        questWindow.SetActive(false);
    }

    public void CloseOptionWindow()
    {
        if (optionWindowEnabled == true)
            optionWindowEnabled = false;

        optionWindow.SetActive(false);
    }
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
    public void ToInvisible()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);

    }
    public void ToVisible()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void OnLevelWasLoaded()
    {
        ToInvisible();
    }
}
