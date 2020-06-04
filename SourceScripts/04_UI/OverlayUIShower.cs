using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayUIShower : MonoBehaviour
{
    public Transform EnemyUI;
    public Transform target;    // UI가 표시될 오브젝트 위치  
    
    private TMP_Text Name;       // 이름을 표시할 텍스트 객체
    private Slider slider;       // 체력을 표시할 슬라이더

    public Camera mainCamera;   // 메인카메라
    private Canvas canvas;       // UI를 표시할 캔버스 객체

    private BaseCharacter baseCharacter;
    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;
        canvas = GameObject.FindGameObjectWithTag("EnemyUICanvas").GetComponent<Canvas>();
        baseCharacter = this.GetComponent<BaseCharacter>();

        Name = EnemyUI.GetChild(0).GetComponent<TMP_Text>();
        slider = EnemyUI.GetChild(1).GetComponent<Slider>();

        Name.text = baseCharacter.NAME;

        EnemyUI.SetParent(canvas.transform);
        EnemyUI.localPosition = Vector3.zero;
        EnemyUI.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        slider.transform.position = screenPos;
        Name.transform.position = screenPos + new Vector3(0, 15.0f, 0);

        slider.value = (float)baseCharacter.HEALTH / (float)baseCharacter.MAXHEALTH;
    }
}
