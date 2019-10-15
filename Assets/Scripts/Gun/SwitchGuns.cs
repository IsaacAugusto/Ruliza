using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGuns : MonoBehaviour
{
    [SerializeField] private GameObject _primaryGun;
    [SerializeField] private GameObject _secondaryGun;
    private GunClass _primary;
    private GunClass _secondary;

    void Start()
    {
        _primaryGun.SetActive(false);
        _secondaryGun.SetActive(true);
        _primary = _primaryGun.GetComponent<GunClass>();
        _secondary = _secondaryGun.GetComponent<GunClass>();
    }

    void Update()
    {
        SwitchWeapons();
    }

    private void SwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _primaryGun.SetActive(true);
            _secondaryGun.SetActive(false);
            _secondary._canShoot = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _primaryGun.SetActive(false);
            _secondaryGun.SetActive(true);
            _primary._canShoot = true;
        }
    }
}
