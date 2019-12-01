using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLegacyEnjinSDK;

public class BlockChainManager : MonoBehaviour
{
    public App App;
    public User Admin;

    private string _plataformURL = "https://kovan.cloud.enjin.io/";
    private string _adminEmail = "isaac.augusto@hotmail.com";
    private string _adminPas = "Is@@c1234567";
    private string _developAcessToken;
    private int _appId = 1747;
    private string _plataformID = "5";
    [SerializeField] private int _playerIdentityId;
    [SerializeField] private string _playerAddress;
    private User player;
    private Dictionary<int, List<CryptoItem>> _playerInvetory;
    public Dictionary<string, int> list;

    void Start()
    {
        // iniciar o admin e a plataforma
        Admin = Enjin.StartPlatform(_plataformURL, _adminEmail, _adminPas);
        _developAcessToken = Enjin.AccessToken;
        //-----------------------------------

        // pegar o player
        player = Enjin.Login(_adminEmail, _adminPas);
        Debug.Log(player.identities.Length);
        for (int i = 0; i < player.identities.Length; i++)
        {
            Identity identity = player.identities[i];
            Debug.Log(identity.app_id);
            if (identity.app_id == _appId)
            {
                _playerIdentityId = identity.id;
                _playerAddress = identity.ethereum_address;
            }
        }
        //--------------------------------------------

        //Pegar o inventario do player
        _playerInvetory = Enjin.GetCryptoItemsByAddress(_playerAddress);
        foreach(var appId in _playerInvetory.Keys)
        {
            Debug.Log("app id = " + appId);
            List<CryptoItem> appinventory = _playerInvetory[appId];
            foreach (CryptoItem item in appinventory)
            {
                Debug.Log(item.balance + " " + item.name);
                int amount = item.balance;
            }
        }
        //-------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
    }
}
