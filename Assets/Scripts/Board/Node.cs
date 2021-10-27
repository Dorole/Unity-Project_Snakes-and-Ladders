using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int nodeID; //node order number in the route
    public Node connectedNode; //the node where ladder/snake is leading to

    public Image icon;
    public Text conNodeText;

    [SerializeField] private Text _numberText;

    public void SetNodeID(int nodeId)
    {
        nodeID = nodeId;

        if (_numberText != null)
            _numberText.text = nodeID.ToString();
    }

    //public void SetNodeIcon() 
    //{
       
    //   if (connectedNode != null)
    //   {
    //        conNodeText.text = connectedNode.nodeID.ToString();

    //        if (nodeID < connectedNode.nodeID)
    //            icon.color = Color.green;
    //        else
    //            icon.color = Color.red;

    //        icon.gameObject.SetActive(true);
    //   }
    //}

    private void OnDrawGizmos()
    {
        if (connectedNode != null)
        {
            Color color = (connectedNode.nodeID > nodeID) ? Color.green : Color.red;

            Debug.DrawLine(transform.position, connectedNode.transform.position, color);
        }
    }

    //potentially rearrange pieces here if there are 2 or more pieces on the same node
}
