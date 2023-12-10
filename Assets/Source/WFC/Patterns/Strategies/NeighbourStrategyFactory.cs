using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategyFactory
    {
        private Dictionary<string, Type> _strategies;

        public NeighbourStrategyFactory()
        {
            LoadTypesIFindNeighbourStrategy();
        }

        private void LoadTypesIFindNeighbourStrategy()
        {
            _strategies = new Dictionary<string, Type>();
            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(IFindNeighbourStrategy).ToString()) != null)
                {
                    _strategies.Add(type.Name.ToLower(), type);
                }
            }
        }

        public IFindNeighbourStrategy CreateInstance(string strategyName)
        {
            Type t = GetTypeToCreate(strategyName);
            if (t == null)
            {
                t = GetTypeToCreate("more");
            }
            return Activator.CreateInstance(t) as IFindNeighbourStrategy;
        }

        private Type GetTypeToCreate(string strategyName)
        {
            //return _strategies.FirstOrDefault(possibleStrat => possibleStrat.Key.Contains(strategyName)).Value;
            foreach (var possibleStrategy in _strategies)
            {
                if (possibleStrategy.Key.Contains(strategyName))
                {
                    return _strategies[possibleStrategy.Key];
                }
            }
            
            return null;
        }
    }
}
