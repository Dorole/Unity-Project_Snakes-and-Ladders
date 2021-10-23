using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int _activePlayer;
    private int _diceNumber;
    public List<Player> playerList = new List<Player>();

    [SerializeField]
    private float _waitBeforeRolling = 2.0f;

    public enum States
    {
        Waiting,
        RollDice,
        SwitchPlayer
    }

    public States state;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (playerList[_activePlayer].playerType == Player.PlayerType.CPU)
        {
            switch (state)
            {
                case States.Waiting:
                    break;
                case States.RollDice:
                    StartCoroutine(RollDiceWithDelay());
                    state = States.Waiting;
                    break;
                case States.SwitchPlayer:
                    break;
            }
        }
    }

    private IEnumerator RollDiceWithDelay()
    {
        yield return new WaitForSeconds(_waitBeforeRolling);

        _diceNumber = Random.Range(1, 7);
        Debug.Log("Rolled: " + _diceNumber);

        //player moves
        playerList[_activePlayer].piece.Play(_diceNumber);
       
    }
}
