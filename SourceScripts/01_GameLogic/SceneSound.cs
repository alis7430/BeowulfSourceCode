using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSound : MonoBehaviour
{
    public string BGMname;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM(BGMname);

        if (UIManager.Instance != null && SceneManager.GetActiveScene().name != "05_Ending") 
            UIManager.Instance.ToVisible();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
