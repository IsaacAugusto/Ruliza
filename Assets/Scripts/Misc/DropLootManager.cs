using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLootManager : MonoBehaviour
{
    public static DropLootManager Instance;

    [SerializeField] private GameObject _pistolBulletParticle;
    [SerializeField] private GameObject _shotgunBulletParticle;
    private GameObject _dHolder;
    private List<GameObject> _pList = new List<GameObject>();
    private List<GameObject> _sList = new List<GameObject>();

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        _dHolder = new GameObject("DropHolder");
    }

    void Update()
    {
        
    }

    private GameObject GetBulletDrop(int num)
    {
        List<GameObject> myList;
        GameObject particle;
        if (num == 2)
        {
            myList = _pList;
            particle = _pistolBulletParticle;
        } else
        {
            myList = _sList;
            particle = _shotgunBulletParticle;
        }

        foreach (GameObject p in myList)
        {
            if (!p.activeInHierarchy)
            {
                p.SetActive(true);
                return p;
            }
        }
        GameObject p2 = Instantiate(particle, _dHolder.transform);
        if (num == 1)
        {
            _sList.Add(p2);
        }
        else
        {
            _pList.Add(p2);
        }
        return p2;
    }

    public void EnemyDropBullets(Vector3 p)
    {
        int x = Random.Range(0, 101);
        int y = Random.Range(1, 3);

        if (x >= 60)
        {
            var d = GetBulletDrop(y);
            d.transform.position = p;
        }
    }
}
