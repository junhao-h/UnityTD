using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] int goldCost = 75;
    [SerializeField] float buildDelay = 1f;

    void Start()
    {
        StartCoroutine(Build());
    }
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
    IEnumerator Build()
    {
        // Disable all game object on creation
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}
