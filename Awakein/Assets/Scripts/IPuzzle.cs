using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzle 
{
    public bool IsSolved { get; set;} 

    void StartPuzzle();
    void ExitPuzzle();
}
