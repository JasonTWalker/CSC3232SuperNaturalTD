using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public string levelName;
    public int platformWidth;
    public int platformLength;
    public float cellSize;
    public Vector3 originPosition;
}

