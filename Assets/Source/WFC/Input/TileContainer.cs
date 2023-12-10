using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class TileContainer
    {
        public TileBase Tile;
        public int X;
        public int Y;

        public TileContainer(TileBase tile, int x, int y)
        {
            Tile = tile;
            X = x;
            Y = y;
        }
    }
}
