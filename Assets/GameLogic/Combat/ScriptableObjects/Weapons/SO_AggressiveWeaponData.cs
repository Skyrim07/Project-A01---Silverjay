using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newAggressiveWeaponData", menuName = "Data/Weapon Data/AggressiveWeapon")]
public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField] private AttackDetails[] attackDetails;

    public AttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }

    private void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed; 
        }
    }
}
