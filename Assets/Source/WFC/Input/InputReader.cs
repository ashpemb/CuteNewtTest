using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputReader : IInputReader<TileBase>
    {

        private Tilemap _inputTileMap;

        public InputReader(Tilemap input)
        {
            _inputTileMap = input;
        }
        
        public IValue<TileBase>[][] ReadInputToGrid()
        {
            var grid = ReadInputTileMap();

            TileBaseValue[][] gridOfValues = null;

            if (grid != null)
            {
                gridOfValues = MyCollectionExtension.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);
                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[0].Length; col++)
                    {
                        gridOfValues[row][col] = new TileBaseValue(grid[row][col]);
                    }
                }
            }

            return gridOfValues;
        }

        private TileBase[][] ReadInputTileMap()
        {
            InputImageParameters imageParameters = new InputImageParameters(_inputTileMap);
            return CreateTileBaseGrid(imageParameters);
        }

        private TileBase[][] CreateTileBaseGrid(InputImageParameters imageParameters)
        {
            TileBase[][] gridOfInputTiles = null;
            gridOfInputTiles =
                MyCollectionExtension.CreateJaggedArray<TileBase[][]>(imageParameters.Height, imageParameters.Width);
            for (int row = 0; row < imageParameters.Height; row++)
            {
                for (int col = 0; col < imageParameters.Width; col++)
                {
                    gridOfInputTiles[row][col] = imageParameters.QueueOfTiles.Dequeue().Tile;
                }
            }

            return gridOfInputTiles;
        }
    }
}
