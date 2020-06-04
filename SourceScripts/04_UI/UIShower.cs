using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//-----------------------------------------------------------
// Scripts\Character\UIShower.cs
//
// 머리위에 이름과 체력바를 보이게하는 스크립트(월드좌표 UI)
// 1. 메인카메라의 월드-뷰포트 변환으로 좌표를 저장합니다.
// 2. 저장한 좌표를 뷰포트-월드 변환으로 UI카메라에 표시합니다.
// 3. BaseCharacter를 가진 오브젝트만이 사용 가능합니다.
//-----------------------------------------------------------

public class UIShower : MonoBehaviour
{
    //-----------------------------------------------------------
    #region Variables

    public float ViewRange;
    
    public Transform EnemyUI;
    public Transform target;    // UI가 표시될 오브젝트 위치

    private Canvas canvas;       // UI를 표시할 캔버스 객체
    private TMP_Text Name;       // 이름을 표시할 텍스트 객체
    private Slider slider;       // 체력을 표시할 슬라이더

    private Camera mainCamera;   // 메인카메라
    private Camera UIcamera;     // UI카메라

    private BaseCharacter baseCharacter;

    private Transform player;

    [HideInInspector]
    public bool is_active;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        baseCharacter = this.GetComponent<BaseCharacter>();
        mainCamera = Camera.main;

        canvas = GameObject.FindGameObjectWithTag("EnemyUICanvas").GetComponent<Canvas>();
        UIcamera = GameObject.FindGameObjectWithTag("UICameraBack").GetComponent<Camera>();

        Name = EnemyUI.GetChild(0).GetComponent<TMP_Text>();
        slider = EnemyUI.GetChild(1).GetComponent<Slider>();

        Name.text = baseCharacter.NAME;
        canvas.worldCamera = UIcamera;

        EnemyUI.SetParent(canvas.transform);
        EnemyUI.localPosition = Vector3.zero;
        EnemyUI.localScale = Vector3.one;
        EnemyUI.localEulerAngles = Vector3.zero;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        is_active = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
            is_active = false;
        else
            is_active = true;

        float dist = 0f;

        if (player != null)
            dist = Vector3.Distance(transform.position, player.position);

        if (dist < ViewRange && is_active == true && baseCharacter.is_dead == false)
        {
            EnemyUI.gameObject.SetActive(true);

            Vector3 UIPosition = mainCamera.WorldToViewportPoint(target.position);
            Vector3 screenPos = UIcamera.ViewportToWorldPoint(UIPosition);

            slider.transform.position = screenPos;
            Name.transform.position = screenPos + new Vector3(0, 0.3f, 0);

            slider.value = (float)baseCharacter.HEALTH / (float)baseCharacter.MAXHEALTH;
        }
        else
        {
            EnemyUI.gameObject.SetActive(false);
        }

    }

    private void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch(Event_Type)
        {
            case EVENT_TYPE.DEAD:
                is_active = false;
                break;
        }
    }

}
