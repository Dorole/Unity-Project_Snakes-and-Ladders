using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    void Update()
    {
        //debug, delete later
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            _stepsToMove = Random.Range(1, 7); //dice simulation
            Debug.Log("Rolled: " + _stepsToMove);

            if (_doneSteps + _stepsToMove < _route.nodeList.Count)
                StartCoroutine(Move());
            else
                Debug.Log("The rolled number is too high!");
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

        _isMoving = false;
    }

    private bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime));
    }


}
