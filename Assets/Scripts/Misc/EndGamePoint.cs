using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePoint : MonoBehaviour
{
    private GameManager _gm;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _gm.ShowEndGamePainel();
        }
        Debug.Log(collision.name);
    }
}
