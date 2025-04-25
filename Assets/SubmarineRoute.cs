using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineRoute : MonoBehaviour
{
    [System.Serializable]
    public struct PathNode
    {
        public GameObject Node;
        public string Name;
        public bool IsStopPoint;
    }
    [SerializeField] private List<PathNode> Pathing;
    PathNode CurrentNode;
    private int Index;
    private void Awake()
    {
        if(Pathing != null)
        {
            CurrentNode = Pathing[0];
            Index = 0;
            gameObject.transform.position = Pathing[Index].Node.transform.position;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveNext(string DestName) 
    {
        while (Pathing[Index].IsStopPoint == false || Pathing[Index].Name != DestName)
        {

        }
    }
}
