using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestWindow : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text experienceText;
    public TMP_Text goldText;

    public Image rewardItemIcon;
    public ShowItemSlot rewardSlot;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetQuestWindow(string title,
        string description, string experience, string gold, Item itemReward)
    {
        titleText.text = title;
        descriptionText.text = description;
        experienceText.text = experience;
        goldText.text = gold;

        if (itemReward != null)
        {
            rewardItemIcon.sprite = itemReward.icon;
            rewardSlot.item = itemReward;
        }
        else
        {
            rewardItemIcon.sprite = UIManager.Instance.defaultImage;
            rewardSlot.item = null;
        }

        UIManager.Instance.questWindowEnabled = true;
    }
}