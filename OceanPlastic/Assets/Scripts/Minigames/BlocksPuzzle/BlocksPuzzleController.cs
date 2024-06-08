using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BlocksPuzzleController : MonoBehaviour
{
    
    List<GridCell> gridCells = new List<GridCell>();

    public UnityEvent onWin;
    
    // Start is called before the first frame update
    void Start()
    {
        gridCells = GetComponentsInChildren<GridCell>().ToList();
    }

    public void Check()
    {
        foreach (var cell in gridCells)
        {
            if (!cell.isChecked)
            {
                return;
            }
        }
        onWin.Invoke();
        Debug.Log("FINISHED");
    }
}
