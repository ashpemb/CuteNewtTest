using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

namespace WaveFunctionCollapse
{
    public class TileBaseValue : IValue<TileBase>
    {
        public TileBaseValue(TileBase tileBase)
        {
            Value = tileBase;
        }

        public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
        {
            return x == y;
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(IValue<TileBase> other)
        {
            return other != null && other.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public TileBase Value { get; }
    }
}
