using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtility
{
    public interface IManager
    {
        int Priority { get; }
        void SetUp();
    }
}