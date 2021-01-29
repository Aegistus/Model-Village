using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    public Equipment CurrentlyEquipped { get; private set; }

    public void Equip(Equipment equipment)
    {
        if (equipment != null)
        {
            CurrentlyEquipped = equipment;
            CurrentlyEquipped.transform.position = transform.position;
            CurrentlyEquipped.transform.rotation = transform.rotation;
            CurrentlyEquipped.transform.parent = transform;
            CurrentlyEquipped.gameObject.SetActive(true);
        }
    }

    public Equipment UnEquip()
    {
        Equipment toReturn = CurrentlyEquipped;
        CurrentlyEquipped = null;
        toReturn?.gameObject.SetActive(false);
        return toReturn;
    }
}
