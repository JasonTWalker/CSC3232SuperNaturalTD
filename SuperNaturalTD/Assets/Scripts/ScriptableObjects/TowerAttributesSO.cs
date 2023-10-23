using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTowerAttributes", menuName = "Game/Tower Attributes")]
public class TowerAttributesSO : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public int towerDamage;
    public float towerRange;
    public float towerFireRate;
    public float towerProjectileSpeed;
    public float towerHealth;
    public float towerArmor;
}
