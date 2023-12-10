using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaveFunctionCollapse
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}
