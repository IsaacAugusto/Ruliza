using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrowGranade : MonoBehaviour
{
    [SerializeField] private GameObject _granadePrefab;
    private int _granadeCount = 3;
    private GunClass gun;
    private List<GameObject> _granadePool = new List<GameObject>();
    private ReloadBar _reloadInterface;
    private GameObject _poolHolder;
    private float _trowTimer = 2;

    void Start()
    {
        _reloadInterface = FindObjectOfType<ReloadBar>();
        _granadeCount = 3;
        _poolHolder = new GameObject("GranedePool");
        gun = GetComponentInChildren<GunClass>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _trowTimer <= 0 && _granadeCount > 0)
        {
            ShootGranade();
        }

        if (_trowTimer > 0)
        {
            _trowTimer -= Time.deltaTime;
        }

        if (!gun.gameObject.activeInHierarchy)
        {
            gun = GetComponentInChildren<GunClass>();
        }

        _reloadInterface.ShowGrenades(_granadeCount);
    }

    private void ShootGranade()
    {
        Granade g = GetGranade().GetComponent<Granade>();
        g.transform.position = gun._bulletSpawnPosition.position;
        g.transform.rotation = gun.transform.rotation;
        g.GetComponent<Rigidbody2D>().velocity = g.transform.right * 20;
        _trowTimer = 2;
        _granadeCount--;
    }

    private GameObject GetGranade()
    {
        foreach (GameObject g in _granadePool)
        {
            if (!g.activeInHierarchy)
            {
                g.SetActive(true);
                return g;
            }
        }
        GameObject gr = Instantiate(_granadePrefab, _poolHolder.transform);
        _granadePool.Add(gr);
        return gr;
    }

    
}
