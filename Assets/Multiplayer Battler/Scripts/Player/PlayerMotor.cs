using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCars.Player
{
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 3;

        private bool isSetUp = false;

        public void Setup()
        {
            isSetUp = true;
        }

        void Update()
        {
            if (!isSetUp) return; //if not set up, don't do the thing

            transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        }
    }
}