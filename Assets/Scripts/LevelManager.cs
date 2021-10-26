using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _minDistance, _maxDistance;

    private Route _route;

    private List<Node> _nodeList = new List<Node>();
    private List<int> _usedNodesId = new List<int>();

    bool foundAvailableNode = true;
    bool foundAvailableConNode = true;

    private void Start()
    {
        _route = FindObjectOfType<Route>();

        _usedNodesId.Clear();

        foreach (Transform node in _route.nodeList)
        {
            Node n = node.GetComponent<Node>();

            if (n != null)
                _nodeList.Add(n);
        }

        GenerateLevel();

    }

    private void GenerateLevel()
    {
        int ladders = Random.Range(10, 16); //generate min 10 and max 15 ladders
        int snakes = ladders;

        Debug.Log(ladders + " , " + snakes);

        for (int i = 0; i < ladders; i++)
        {
           FindLadderNode();
        }

        for (int i = 0; i < snakes; i++)
        {
            FindSnakeNode();
        }
    }

    private void FindLadderNode()
    {
        int nodeId = 0;
        foundAvailableNode = false;

        while (!foundAvailableNode)
        {
            nodeId = Random.Range(1, 91);

            if (MinimumNodesDistance(nodeId))
                foundAvailableNode = true;
        }

        _usedNodesId.Add(nodeId);

        int conNodeId = 0;
        foundAvailableConNode = false;

        while (!foundAvailableConNode)
        {
            conNodeId = Random.Range(_minDistance, _maxDistance);
            conNodeId += nodeId;

            if (!_usedNodesId.Contains(conNodeId) && (conNodeId < _nodeList.Count - 1))
                foundAvailableConNode = true;
        }

        _usedNodesId.Add(conNodeId);

        Node n = _nodeList[nodeId];

        n.connectedNode = _nodeList[conNodeId];

        n.icon.gameObject.SetActive(true);
        n.icon.color = Color.green;
        n.conNodeText.text = conNodeId.ToString();
    }

    private void FindSnakeNode()
    {
        int nodeId = 0;
        foundAvailableNode = false;

        while (!foundAvailableNode)
        {
            nodeId = Random.Range(11, 100);

            if (MinimumNodesDistance(nodeId))
                foundAvailableNode = true;
        }

        _usedNodesId.Add(nodeId);

        int conNodeId = 0;
        foundAvailableConNode = false;

        while (!foundAvailableConNode)
        {
            conNodeId = Random.Range(_minDistance, _maxDistance);
            conNodeId = nodeId - conNodeId;

            if (!_usedNodesId.Contains(conNodeId) && (conNodeId > 0))
                foundAvailableConNode = true;
        }

        _usedNodesId.Add(conNodeId);

        Node n = _nodeList[nodeId];
        n.connectedNode = _nodeList[conNodeId];

        n.icon.gameObject.SetActive(true);
        n.icon.color = Color.red;
        n.conNodeText.text = conNodeId.ToString();
    }

    //minimum distance between two nodes containing a ladder/snake or being connected to nodes with a ladder/snake
    private bool MinimumNodesDistance (int nodeId)
    {
        return !_usedNodesId.Contains(nodeId) && !_usedNodesId.Contains(nodeId + 1) && !_usedNodesId.Contains(nodeId - 1);
    }
}
