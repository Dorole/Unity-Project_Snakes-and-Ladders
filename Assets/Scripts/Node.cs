using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int nodeID; //node order number in the route
    public Node connectedNode; //the node where ladder/snake is leading to
    
    [SerializeField]
    private Text _numberText;

    public void SetNodeID(int nodeId)
    {
        nodeID = nodeId;

        if (_numberText != null)
            _numberText.text = nodeID.ToString();
    }

    private void OnDrawGizmos()
    {
        if (connectedNode != null)
        {
            Color color = (connectedNode.nodeID > nodeID) ? Color.green : Color.red;

            Debug.DrawLine(transform.position, connectedNode.transform.position, color);
        }
    }
}
