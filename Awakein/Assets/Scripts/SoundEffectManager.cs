using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    //조건이 열어지면 
    public LockManagerCharacter lockManager;
   public void DoorOpeningButtonClick(){
    if ( lockManager.BDManager.IsSolved||lockManager.TBManager.IsSolved)
    SoundManager.Instance.PlaySFX(1);
   }
   
}
