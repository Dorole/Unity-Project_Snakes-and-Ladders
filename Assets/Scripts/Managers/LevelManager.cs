using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private int _minDistance, _maxDistance;

    private Route _route;

    private List<Node> _nodeList = new List<Node>();
    private List<int> _usedNodesId = new List<int>();

    bool foundAvailableNode = true;
    bool foundAvailableConNode = true;

    private void Awake()
    {
        instance = this;
        _usedNodesId.Clear();
    }

    private void Start()
    {
        _route = FindObjectOfType<Route>();

        foreach (Transform node in _route.nodeList)
        {
            Node n = node.GetComponent<Node>();

            if (n != null)
                _nodeList.Add(n);
        }

        GenerateLevel();

        SetupNodeIcon();
    }

    private void GenerateLevel()
    {
        int ladders = Random.Range(10, 16); //generate min 10, max 15 ladders
        int snakes = ladders;

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

            if (!_usedNodesId.Contains(nodeId))
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

    }

    private void FindSnakeNode()
    {
        int nodeId = 0;
        foundAvailableNode = false;

        while (!foundAvailableNode)
        {
            nodeId = Random.Range(11, 100);

            if (!_usedNodesId.Contains(nodeId))
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

    }

    //minimum distance between two nodes containing a ladder/snake or being connected to nodes with a ladder/snake
    private bool MinimumNodesDistance (int nodeId)
    {
        return !_usedNodesId.Contains(nodeId) && !_usedNodesId.Contains(nodeId + 1) && !_usedNodesId.Contains(nodeId - 1);
    }

    private void SetupNodeIcon()
    {       
        foreach (Node node in _nodeList)
        {
            if (node.connectedNode != null)
            {
                node.conNodeText.text = node.connectedNode.nodeID.ToString();

                if (node.nodeID < node.connectedNode.nodeID)
                    node.icon.color = Color.green;
                else 
                    node.icon.color = Color.red;

                node.icon.gameObject.SetActive(true);
            }
        }
    }
}
