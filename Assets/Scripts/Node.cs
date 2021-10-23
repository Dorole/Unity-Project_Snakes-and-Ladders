using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField]
    private int _nodeID; //node order number in the route
    [SerializeField]
    private Text _numberText;
    [SerializeField]
    private Node _connectedNode; //the node where ladder/snake is leading to

    public void SetNodeID(int nodeId)
    {
        _nodeID = nodeId;

        if (_numberText != null)
            _numberText.text = _nodeID.ToString();
    }

    private void OnDrawGizmos()
    {
        if (_connectedNode != null)
        {
            Color color = (_connectedNode._nodeID > _nodeID) ? Color.green : Color.red;

            Debug.DrawLine(transform.position, _connectedNode.transform.position, color);
        }
    }
}
