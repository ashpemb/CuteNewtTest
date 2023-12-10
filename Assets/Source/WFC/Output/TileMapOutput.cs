using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap _outputImage;
        private ValuesManager<TileBase> _valuesManager;
        public Tilemap OutputImage => _outputImage;

        public TileMapOutput(ValuesManager<TileBase> valuesManager, Tilemap outputImage)
        {
            _outputImage = outputImage;
            _valuesManager = valuesManager;
        }
        
        public void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height)
        {
            if (outputValues.Length == 0)
            {
                return;
            }
            
            _outputImage.ClearAllTiles();

            int[][] valueGrid;
            valueGrid = manager.ConvertPatternToValues<TileBase>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = _valuesManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    _outputImage.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }
    }
}
