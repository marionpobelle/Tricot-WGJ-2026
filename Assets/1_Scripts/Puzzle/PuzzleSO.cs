using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleSO", menuName = "Puzzle/PuzzleSO", order = 0)]
public class PuzzleSO : ScriptableObject
{
    public List<PointStatus> PuzzleLine1 = new List<PointStatus>();
    public List<PointStatus> PuzzleLine2 = new List<PointStatus>();
    public List<PointStatus> PuzzleLine3 = new List<PointStatus>();
    public List<PointStatus> PuzzleLine4 = new List<PointStatus>();
}

