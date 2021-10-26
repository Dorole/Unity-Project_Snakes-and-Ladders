using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum States
    {
        Waiting,
        RollDice,
        SwitchPlayer
    }

    [Space]
    public States state;
    [Space]
    [SerializeField] private GameObject _rollDiceButton;
    [SerializeField] private Dice _dice;
    [SerializeField] private float _waitBeforeRolling = 1.0f;
    [Space]
    public List<Player> playerList = new List<Player>();
    [HideInInspector] public bool isPlayerHuman;

    private int _activePlayer;


    private void Awake()
    {
        instance = this;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (SaveSettings.players[i] == "HUMAN")
                playerList[i].playerType = Player.PlayerType.Human;

            if (SaveSettings.players[i] == "CPU")
                playerList[i].playerType = Player.PlayerType.CPU;
        }

        if (SaveSettings.players[3] == "NONE")
        {
            playerList[3].piece.gameObject.SetActive(false);
            playerList.RemoveAt(3);
        }

        if (SaveSettings.players[2] == "NONE")
        {
            playerList[2].piece.gameObject.SetActive(false);
            playerList.RemoveAt(2);
        }
    }

    private void Start()
    {
        ActivateButton(false);
        
        _activePlayer = Random.Range(0, playerList.Count);

        Interface.instance.infoText.color = playerList[_activePlayer].piece.UIColor;
        Interface.instance.ShowText(playerList[_activePlayer].playerName + " starts!");
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
                    _rollDiceButton.GetComponent<Image>().color = playerList[_activePlayer].piece.UIColor; //matches player color and rollDiceButton color
                }
                state = States.Waiting;
                break;
            case States.SwitchPlayer:
                _activePlayer++;
                _activePlayer %= playerList.Count;
                Interface.instance.infoText.color = playerList[_activePlayer].piece.UIColor;
                Interface.instance.ShowText(playerList[_activePlayer].playerName + "'s turn!");
                state = States.RollDice;
                break;
        }
    }

    //this coroutine executes CPU player's turn
    private IEnumerator CPUTurn()
    {
        isPlayerHuman = false;

        yield return new WaitForSeconds(_waitBeforeRolling);

        _dice.StartCoroutine("RollDice");
    }

    //called from Dice class
    public void MovePlayer(int diceNumber) 
    {
        Debug.Log(playerList[_activePlayer].playerName + " rolled a " + diceNumber + ".");
       
        playerList[_activePlayer].piece.Play(diceNumber);
    }

    private void ActivateButton(bool active)
    {
        _rollDiceButton.SetActive(active);
    }

    //this method is implemented on button click (human player only)
    public void HumanTurn()
    {
        isPlayerHuman = true;
        _dice.StartCoroutine("RollDice");
        ActivateButton(false);
    }

    public void GameOver()
    {
        Interface.instance.winPanel.SetActive(true);
        Interface.instance.inGamePanel.SetActive(false);

        Interface.instance.winText.color = playerList[_activePlayer].piece.UIColor;
        Interface.instance.WinMessage(playerList[_activePlayer].playerName);

        Debug.Log(playerList[_activePlayer].playerName + " won!");
    }
}
