﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestInfoSlot : MonoBehaviour
{
    public int QuestID;

    public TMP_Text nameText;
    public TMP_Text TypeText;
    public TMP_Text NeedsText;
    public TMP_Text AmountText;

    public GameObject completeImage;

    public bool is_Using = false;

    public void AddInfo(Quest quest)
    {
        this.gameObject.SetActive(true);
        completeImage.SetActive(false);

        QuestID = quest.ID;
        nameText.text = quest.title;
        TypeText.text = quest.goal.goalType.ToString();

        switch (quest.goal.goalType)
        {
            case QuestGoalType.NONE:
                break;
            case QuestGoalType.KILL:
                if (quest.goal.enemy != null)
                    NeedsText.text = quest.goal.enemy.NAME;
                else
                    NeedsText.text = "None";

                break;
            case QuestGoalType.GATHERING:
                if (quest.goal.needItem != null)
                    NeedsText.text = quest.goal.needItem.itemName;
                else
                    NeedsText.text = "None";

                break;
            default:
                break;
        }

        AmountText.text = quest.goal.currentAmount.ToString() + " / " + quest.goal.requireAmount.ToString();

        is_Using = true;
    }

    public void UpdateInfo(int curAmount, int requireAmount)
    {
        if (is_Using)
        {
            AmountText.text = curAmount.ToString() + " / " + requireAmount.ToString();
        }
    }
    public void ClearInfo()
    {
        QuestID = 0;

        nameText.text = "";
        TypeText.text = "";
        NeedsText.text = "";
        AmountText.text = "";

        this.gameObject.SetActive(false);
        is_Using = false;
    }

    public void Complete()
    {
        completeImage.SetActive(true);
    }

}
