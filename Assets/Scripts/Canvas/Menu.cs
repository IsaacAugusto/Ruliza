using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private InputField _login;
    [SerializeField] private InputField _password;

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
    }

    void Start()
    {
        
    }

    public void LoginPlayer()
    {
        BlockChainManager.Instance.Login(_login.text, _password.text);
    }

    private void Update()
    {
        if (BlockChainManager.Instance.PlayerConected && !_menuPanel.activeInHierarchy)
        {
            _menuPanel.SetActive(true);
            _loginPanel.SetActive(false);
        }
    }

}
