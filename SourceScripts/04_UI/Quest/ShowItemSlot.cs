using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && ItemDescription.instance.is_running == false)
        {
            Item i = item.GetComponent<Item>();

            // 아이템 데이터를 집어넣음
            ItemDescription.instance.DataInput(i.itemName, i.description,
                i.Health, i.stamina, i.Damage, i.strength, i.defense, i.dexterity, i.intelligence);

            // 오브젝트를 액티브하고 실행중이라 알림
            ItemDescription.instance.gameObject.SetActive(true);
            ItemDescription.instance.is_running = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDescription.instance.Clear();
        ItemDescription.instance.is_running = false;
        ItemDescription.instance.gameObject.SetActive(false);
    }
}
