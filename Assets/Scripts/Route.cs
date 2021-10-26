using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    private Transform[] _nodes;
    public List<Transform> nodeList = new List<Transform>();

    void Awake()
    {
        SetNodes();
    }

    private void OnValidate()
    {
        SetNodes();
    }

    private void SetNodes()
    {
        nodeList.Clear();
        _nodes = GetComponentsInChildren<Transform>();

        int num = -1;

        foreach (Transform node in _nodes)
        {
            Node n = node.GetComponent<Node>();

            if (n != null)
            {
                num++;
                nodeList.Add(node);
                node.gameObject.name = "Node " + num;

                n.SetNodeID(num);
            }
        }
    }

    private void OnDrawGizmos()
    {
        SetNodes();

        for (int i = 0; i < nodeList.Count; i++)
        {
            Vector3 currentNode = nodeList[i].position;

            if (i > 0)
            {
                Vector3 previousNode = nodeList[i - 1].position;
                Debug.DrawLine(previousNode, currentNode, Color.white);
            }
        }
    }

    
}
