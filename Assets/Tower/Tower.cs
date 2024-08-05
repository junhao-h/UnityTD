using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] int goldCost = 75;


    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindAnyObjectByType<Bank>();
        if (bank == null)
        {
            return false;
        }

        if (bank.CurrentBalance >= goldCost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(goldCost);
            return true;
        }
        return false;
    }
}
