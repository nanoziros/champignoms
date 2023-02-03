using System.Collections.Generic;
using UnityEngine;

namespace Mushroom
{
    public class TendrilNode : MonoBehaviour
    {
        private readonly List<TendrilNode> _childrenNodes = new List<TendrilNode>();

        public void AddTendril(TendrilNode tendrilNode)
        {
            _childrenNodes.Add(tendrilNode);
        }

        public void RemoveTendril(TendrilNode tendrilNode)
        {
            if (_childrenNodes.Contains(tendrilNode))
            {
                _childrenNodes.Remove(tendrilNode);
            }
        }
    }
}
