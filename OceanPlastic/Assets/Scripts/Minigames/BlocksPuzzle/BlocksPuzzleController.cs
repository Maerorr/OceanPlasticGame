using System.Collections.Generic;
using System.Linq;

public class BlocksPuzzleController : Minigame
{
    List<GridCell> gridCells = new List<GridCell>();
    
    // Start is called before the first frame update
    void Start()
    {
        gridCells = GetComponentsInChildren<GridCell>().ToList();
        MinigameInit();
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
    }
}
