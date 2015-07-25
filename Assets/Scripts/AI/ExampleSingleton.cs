using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TDL {
    class ExampleSingleton : MonoBehaviour {

        public static ExampleSingleton Instance {
            get { return _instance; }
        }

        private static ExampleSingleton _instance;

        void Awake() {
            _instance = this;
        }
    }
}
