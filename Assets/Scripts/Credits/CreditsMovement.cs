using UnityEngine;
using System.Collections;

namespace TDL
{
    /// <summary>
    /// Controls the movement of the credits
    /// </summary>
    public class CreditsMovement : MonoBehaviour
    {
        #region Variables
        private bool _pressed = false;
        private float _yMousePosition = 0f;
        // public float _creditLimit = 0;
        // float _creditsInitialY = 0;
        public RectTransform _topLimit;
        public RectTransform _initialPosition;
        [Range(1f,10f)]
        public float speed = 1;
        #endregion

        #region Methods
        void Start()
        {
            // _creditLimit = (Screen.height / 2 + 1244) ;
            //_creditsInitialY = 0;// -(3 * Screen.height / 4);
            transform.position = new Vector3(transform.position.x, _initialPosition.position.y, transform.position.z);
        }

        void Update()
        {
            if (_pressed)
            {
                float currentMousePosition = Input.mousePosition.y;
                float deltaY = currentMousePosition - _yMousePosition;
                transform.position = new Vector3(transform.position.x, transform.position.y + deltaY, transform.position.z);
                _yMousePosition = currentMousePosition;
            }
            else
            {
                if (_topLimit.position.y < transform.position.y)//(_creditLimit < transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, _initialPosition.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
                }
            }
        }

        public void ActivateAnimator()
        {
            _pressed = false;
        }

        public void DeactivateAnimator()
        {
            _pressed = true;
            _yMousePosition = Input.mousePosition.y;
        }
        #endregion
    }
}

