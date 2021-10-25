using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceColor
    {
        Green,
        Red,
        Blue,
        Yellow
    }

    public PieceColor pieceColor;
    public Color UIColor;

    [SerializeField]
    private Route _route;

    private List<Node> _nodeList = new List<Node>();
    private int _playerPosition;

    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _waitBetweenNodes = 0.1f;

    private int _stepsToMove;
    private int _doneSteps;

    private bool _isMoving;
    private bool _buttonsActive;

    private void Start()
    {
        foreach (Transform node in _route.nodeList)
        {
            Node n = node.GetComponent<Node>();

            if (n != null)
                _nodeList.Add(n);
        }
    }

    private IEnumerator Move()
    {
        if (_isMoving)
            yield break;

        _isMoving = true;

        if (_buttonsActive)
        {
            while (!Interface.instance.fwdButtonPressed && !Interface.instance.backButtonPressed)
                yield return null;
        
            Interface.instance.fwdButton.gameObject.SetActive(false);
            Interface.instance.backButton.gameObject.SetActive(false);
        }

        while (_stepsToMove > 0)
        {
            if (_buttonsActive)
            {
                if (Interface.instance.fwdButtonPressed)
                    _playerPosition++;
                else if (Interface.instance.backButtonPressed)
                    _playerPosition--;
            }    
            else
            {
                if (_doneSteps + _stepsToMove < _route.nodeList.Count) 
                    _playerPosition++;
                else if (_doneSteps + _stepsToMove > _route.nodeList.Count) 
                    _playerPosition--;
            }

            Vector3 nextPosition = _route.nodeList[_playerPosition].transform.position;
            nextPosition.z = -0.05f; //offset - otherwise the player is hidden behind the node 

            while (MoveToNextNode(nextPosition))
            {
                yield return null;
            }

            yield return new WaitForSeconds(_waitBetweenNodes);

            _stepsToMove--;
            //_doneSteps++;
        }

        _doneSteps = _nodeList[_playerPosition].nodeID;

        //snake-ladder movement
        if (_nodeList[_playerPosition].connectedNode != null)
        {
            yield return new WaitForSeconds(_waitBetweenNodes);

            int conNodeId = _nodeList[_playerPosition].connectedNode.nodeID;
            Vector3 nextPosition = _nodeList[_playerPosition].connectedNode.transform.position;
            nextPosition.z = -0.05f;

            while (MoveToNextNode(nextPosition))
            {
                yield return null;
            }

            _doneSteps = conNodeId;
            _playerPosition = conNodeId;

        }

        //checks for a win
        if (_doneSteps == _nodeList.Count - 1)
        {
            GameManager.instance.GameOver();
            yield break;
        }

        GameManager.instance.state = GameManager.States.SwitchPlayer;

        Interface.instance.fwdButtonPressed = false;
        Interface.instance.backButtonPressed = false;

        _buttonsActive = false;
        _isMoving = false;
    }

    private bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime));
    }

    public void Play(int diceNumber)
    {
        _stepsToMove = diceNumber;

        if (_doneSteps + _stepsToMove < _route.nodeList.Count && _doneSteps - _stepsToMove > 0 && GameManager.instance.isPlayerHuman)
        {
            Interface.instance.fwdButton.gameObject.SetActive(true);
            Interface.instance.backButton.gameObject.SetActive(true);
            _buttonsActive = true;
        }

        StartCoroutine(Move());
    }

    //private IEnumerator TimerBeforeSwitch()
    //{
    //    Interface.instance.ShowText("Too high!");
    //    Debug.Log("The rolled number is too high!");

    //    yield return new WaitForSeconds(1.5f);

    //    GameManager.instance.state = GameManager.States.SwitchPlayer;
    //}

}
