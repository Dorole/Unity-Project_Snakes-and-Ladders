using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Player> playerList = new List<Player>();
    [Space]
    public GameObject rollDiceButton;
    public Dice dice;

    private int _activePlayer;
    private int _diceNumber;
    [SerializeField]
    private float _waitBeforeRolling = 1.0f;

    public enum States
    {
        Waiting,
        RollDice,
        SwitchPlayer
    }

    [Space]
    public States state;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateButton(false);
    }

    private void Update()
    {
        switch (state)
        {
            case States.Waiting:
                break;
            case States.RollDice:
                if (playerList[_activePlayer].playerType == Player.PlayerType.CPU)
                    StartCoroutine(CPUTurn());
                else
                {
                    ActivateButton(true);
                    rollDiceButton.GetComponent<Image>().color = playerList[_activePlayer].piece.buttonColor; //matches player color and rollDiceButton color
                }
                state = States.Waiting;
                break;
            case States.SwitchPlayer:
                _activePlayer++;
                _activePlayer %= playerList.Count;
                state = States.RollDice;
                break;
        }
    }

    //this coroutine executes CPU player's turn
    private IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(_waitBeforeRolling);

        dice.StartCoroutine("RollDice");
    }

    //called from Dice class
    public void MovePlayer(int diceNumber) 
    {
        Debug.Log(playerList[_activePlayer].piece.gameObject.name + " rolled " + diceNumber);
        playerList[_activePlayer].piece.Play(diceNumber);
    }


    private void ActivateButton(bool active)
    {
        rollDiceButton.SetActive(active);
    }

    //this method is implemented on button click (human player only)
    public void HumanTurn()
    {
        ActivateButton(false);
        dice.StartCoroutine("RollDice");
    }

}
