using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//Basic minion with health that moves on navmesh
namespace TDL {
    public class Minion : AgentMotion {

        public int health = 100;

        void Awake() {
        } 
    }
}
