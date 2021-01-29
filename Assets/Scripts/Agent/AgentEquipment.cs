using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AnimationStance
{
    TwoHanded, OneHandedShield
}

public class AgentEquipment : MonoBehaviour
{
    public EquipmentSlot primarySlot;
    public EquipmentSlot secondarySlot;
    public List<Equipment> carriedPrimaryEquipment;
    public List<Equipment> carriedSecondaryEquipment;

    public AnimationStance CurrentStance { get; private set; }

    private Queue<Equipment> primaryEquipQueue = new Queue<Equipment>();
    private Queue<Equipment> secondaryEquipQueue = new Queue<Equipment>();

    private Animator anim;
    private Dictionary<AnimationStance, int> equipmentStanceLayers = new Dictionary<AnimationStance, int>();

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        equipmentStanceLayers.Add(AnimationStance.TwoHanded, anim.GetLayerIndex("Two Handed"));
        equipmentStanceLayers.Add(AnimationStance.OneHandedShield, anim.GetLayerIndex("One Handed Shield"));
        foreach (var primaryEquip in carriedPrimaryEquipment)
        {
            primaryEquipQueue.Enqueue(primaryEquip);
        }
        foreach (var secondaryEquip in carriedSecondaryEquipment)
        {
            secondaryEquipQueue.Enqueue(secondaryEquip);
        }
    }

    public void GoToNextPrimaryEquipment()
    {
        if (primarySlot.CurrentlyEquipped != null)
        {
            primaryEquipQueue.Enqueue(primarySlot.UnEquip());
        }
        if (primaryEquipQueue.Count > 0)
        {
            primarySlot.Equip(primaryEquipQueue.Dequeue());
            UpdateCurrentEquipmentStance();
        }
        // if primary uses both hands, unequip secondary
        if (primarySlot.CurrentlyEquipped?.usage == Equipment.Usage.Both)
        {
            secondaryEquipQueue.Enqueue(secondarySlot.UnEquip());
        }
    }

    public void GoToNextSecondaryEquipment()
    {
        if (primarySlot.CurrentlyEquipped?.usage != Equipment.Usage.Both)
        {
            if (secondarySlot.CurrentlyEquipped != null)
            {
                secondaryEquipQueue.Enqueue(secondarySlot.UnEquip());
            }
            if (secondaryEquipQueue.Count > 0)
            {
                secondarySlot.Equip(secondaryEquipQueue.Dequeue());
                UpdateCurrentEquipmentStance();
            }
        }
    }

    public void UpdateCurrentEquipmentStance()
    {
        if (primarySlot.CurrentlyEquipped?.usage == Equipment.Usage.Both)
        {
            CurrentStance = AnimationStance.TwoHanded;
        }
        else
        {
            CurrentStance = AnimationStance.OneHandedShield;
        }
        // set all animation stance layers to 0 weight
        foreach (var stance in equipmentStanceLayers)
        {
            anim.SetLayerWeight(stance.Value, 0);
        }
        anim.SetLayerWeight(equipmentStanceLayers[CurrentStance], 1);
    }
}
