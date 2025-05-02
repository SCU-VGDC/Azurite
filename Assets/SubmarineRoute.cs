using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
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
    DG.Tweening.Sequence Route;
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
        PersistentDataScript.Instance.ChangeSubmarineState("name");
        PersistentDataScript.Instance.ChangeSubmarineState("name");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TryMoveNext(out bool success, string DestName = "ThisIsABlankResponse") 
    {
        success = false;
        if (Route.IsPlaying())
        {
            return "";
        }
        Route = DOTween.Sequence();
        int Iterations = 100;
        while (Pathing[Index].Name != DestName)
        {
            Index = (Index + 1)%Pathing.Count;
            Route.Append(gameObject.transform.DOMove(Pathing[Index].Node.transform.position, 10f));
            Iterations--;
            if(Iterations == 0)
            {
                return "";
            }
            if (Pathing[Index].IsStopPoint != false || Pathing[Index].Name == DestName) 
            {
                break;
            }

        }
        success = true;
        return Pathing[Index].Name;
    }
    IEnumerator MoveNextCoroutine(string DestName = "ThisIsABlankResponse")
    {
        do
        {
            Index = (Index + 1) % Pathing.Count;
            yield return gameObject.transform.DOMove(Pathing[Index].Node.transform.position, 10f).WaitForCompletion();

            if (Pathing[Index].IsStopPoint || Pathing[Index].Name == DestName)
            {
                break;
            }

        } while (Pathing[Index].Name != DestName);
    }
}
