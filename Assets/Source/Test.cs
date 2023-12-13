using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

public class Test : MonoBehaviour
{
    public Tilemap InputTilemap;
    public Tilemap outputTilemap;
    public int PatternSize;
    public bool EqualWeights = false;
    public int Width = 5, Height = 5, MaxIterations = 500;

    private ValuesManager<TileBase> valuesManager;
    private WFCCore core;
    private PatternManager patternManager;

    private TileMapOutput output;
    
    public void CreateWFC()
    {
        InputReader reader = new InputReader(InputTilemap);
        var grid = reader.ReadInputToGrid();
        valuesManager = new ValuesManager<TileBase>(grid);
        // StringBuilder builder = null;
        // List<string> list = new List<string>();
        // for (int row = 0; row < grid.Length; row++)
        // {
        //     builder = new StringBuilder();
        //     for (int col = 0; col < grid[0].Length; col++)
        //     {
        //         builder.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
        //     }
        //     list.Add(builder.ToString());
        // }
        // list.Reverse();
        // foreach (var item in list)
        // {
        //     Debug.Log(item);
        // }
        patternManager = new PatternManager(PatternSize);
        patternManager.ProcessGrid(valuesManager, EqualWeights);
        // foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        // {
        //     Debug.Log(dir.ToString() + " " + string.Join(" ", manager.GetPossibleNeighboursForPatternInDirection(0,dir).ToArray()));
        // }
        core = new WFCCore(Width, Height, MaxIterations, patternManager);
        
    }

    public void CreateTilemap()
    {
        output = new TileMapOutput(valuesManager, outputTilemap);
        var result = core.CreateOutputGrid();
        output.CreateOutput(patternManager, result, Width, Height);
    }

    public void ClearOutputTilemap()
    {
        outputTilemap.ClearAllTiles();
    }

    public void SaveTilemap()
    {
        if (output.OutputImage != null)
        {
            outputTilemap = output.OutputImage;
            GameObject objectToSave = outputTilemap.gameObject;
            PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Saved/output.prefab");
        }
    }
    
}
