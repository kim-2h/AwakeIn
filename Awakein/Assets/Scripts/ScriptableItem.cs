using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]



public class Item : ScriptableObject
{
    [SerializeField] private string itemName; 
    public string ItemName { get { return itemName; } } 
    
    [SerializeField] private Material itemMaterial;
    public Material ItemMaterial { get { return itemMaterial; } }

    [SerializeField] private bool isClickable; 
    public bool IsClickable { get { return isClickable; } } 

    [SerializeField] private bool inInventory; 
    public bool InInventory { get { return inInventory; } set { inInventory = value; } }

    [SerializeField] private bool isUsed; 
    public bool IsUsed { get { return isUsed; } set { isUsed = value; } } 

    public void ItsClicked(string iName)
    {
        Debug.Log(iName);       
    }

}
