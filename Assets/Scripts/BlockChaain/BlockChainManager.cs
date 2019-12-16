using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLegacyEnjinSDK;

public class BlockChainManager : MonoBehaviour
{
    public static BlockChainManager Instance;

    public App App;
    public User Admin;
    public bool PlayerConected = false;

    private string _plataformURL = "https://kovan.cloud.enjin.io/";
    #region login e senha admin
    private string _adminEmail = "isaac.augusto@hotmail.com";
    private string _adminPas = "Is@@c1234567";
    #endregion
    private string _developAcessToken;
    private int _appId = 1747;
    private string _plataformID = "5";
    [SerializeField] private int _playerIdentityId;
    [SerializeField] private string _playerAddress;
    private User player;
    private string _playerLogin;
    private string _playerPassword;
    private Dictionary<int, List<CryptoItem>> _playerInvetory;
    public Dictionary<string, int> list;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Teste()
    {
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
    }

    public void Login(string email, string senha)
    {
        // pegar o player
        player = Enjin.Login(email, senha);
        for (int i = 0; i < player.identities.Length; i++)
        {
            Identity identity = player.identities[i];
            Debug.Log(identity.app_id);
            if (identity.app_id == _appId)
            {
                _playerIdentityId = identity.id;
                _playerAddress = identity.ethereum_address;
                PlayerConected = true;
            }
        }
    }

    private void Start()
    {
        // iniciar o admin e a plataforma
        Admin = Enjin.StartPlatform(_plataformURL, _adminEmail, _adminPas);
        _developAcessToken = Enjin.AccessToken;
        //-----------------------------------
        
    }

}
