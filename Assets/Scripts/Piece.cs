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
    public Color buttonColor;
    
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

        while (_stepsToMove > 0)
        {
            _playerPosition++;
            Vector3 nextPosition = _route.nodeList[_playerPosition].transform.position;
            nextPosition.z = -0.05f; //offset - otherwise the player is hidden behind the node 

            while(MoveToNextNode(nextPosition))
            {
                yield return null;
            }

            yield return new WaitForSeconds(_waitBetweenNodes);

            _stepsToMove--;
            _doneSteps++;
        }

        //snake-ladder movement
        if(_nodeList[_playerPosition].connectedNode != null)
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

        //check for a win

        GameManager.instance.state = GameManager.States.SwitchPlayer;
        _isMoving = false;
    }

    private bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime));
    }

    public void Play(int diceNumber)
    {
        _stepsToMove = diceNumber;

        if (_doneSteps + _stepsToMove < _route.nodeList.Count)
            StartCoroutine(Move());
        else
            Debug.Log("The rolled number is too high!");
    }
}
