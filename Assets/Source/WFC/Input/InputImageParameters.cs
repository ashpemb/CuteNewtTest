using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputImageParameters
    {
        private Vector2Int? _bottomLeftTileCoords = null;
        private Vector2Int? _topRightTileCoords = null;
        private BoundsInt _inputTileMapBounds;
        private TileBase[] _inputTileMapTilesArray;
        private Queue<TileContainer> _queueOfTiles = new Queue<TileContainer>();
        private int _width = 0, _height = 0;
        private Tilemap _inputTileMap;

        public InputImageParameters(Tilemap inputTileMap)
        {
            _inputTileMap = inputTileMap;
            _inputTileMapBounds = _inputTileMap.cellBounds;
            _inputTileMapTilesArray = _inputTileMap.GetTilesBlock(_inputTileMapBounds);
            ExtractNoneEmptyTile();
            VerifyInputTiles();
        }

        private void VerifyInputTiles()
        {
            if (_topRightTileCoords == null || _bottomLeftTileCoords == null)
            {
                throw new Exception("Input tilemap is empty");
            }

            int minX = _bottomLeftTileCoords.Value.x;
            int maxX = _topRightTileCoords.Value.x;
            int minY = _bottomLeftTileCoords.Value.y;
            int maxY = _topRightTileCoords.Value.y;

            _width = Math.Abs(maxX - minX) + 1;
            _height = Math.Abs(maxY - minY) + 1;

            int tileCount = _width * _height;
            if (_queueOfTiles.Count != tileCount)
            {
                throw new Exception("Tilemap has empty fields");
            }

            if (_queueOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new Exception("Tilemap has outliers beyond rectangle");
            }
        }

        private void ExtractNoneEmptyTile()
        {
            //Convert tilemap from 1D to 2D array
            for (int row = 0; row < _inputTileMapBounds.size.y; row++)
            {
                for (int col = 0; col < _inputTileMapBounds.size.x; col++)
                {
                    int index = col + (row * _inputTileMapBounds.size.x);
                    
                    TileBase tile = _inputTileMapTilesArray[index];
                    if (_bottomLeftTileCoords == null && tile != null)
                    {
                        _bottomLeftTileCoords = new Vector2Int(col, row);
                    }

                    if (tile != null)
                    {
                        _queueOfTiles.Enqueue(new TileContainer(tile, col, row));
                        _topRightTileCoords = new Vector2Int(col, row);
                    }
                }
            }
        }

        public Queue<TileContainer> QueueOfTiles
        {
            get => _queueOfTiles;
            set => _queueOfTiles = value;
        }

        public int Width => _width;

        public int Height => _height;
    }
}
