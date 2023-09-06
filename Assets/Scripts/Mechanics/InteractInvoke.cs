using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Animations;
using Platformer.Core;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D), typeof(Invokable))]
    public class InteractInvoke : MonoBehaviour
    {
        public Invokable invokableObject;
        bool isInteractable = false;
        // Start is called before the first frame update
        void Start()
        {
            invokableObject = GetComponent<Invokable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isInteractable && (
                Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("Jump"))) {
                invokableObject.Invoke();
            }
        }
        void OnCollisionStay2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                isInteractable = true;
            }
        }
        
        void OnCollisionExit2d(Collision2D collision) {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                isInteractable = false;
            }
        }
    }
}