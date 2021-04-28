using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class SimpleMovement : Photon.Pun.MonoBehaviourPun
    {
        [SerializeField]
        private float directionDampTime = 0.25f;
        Animator ani;
        float stepwidth = 0.1f;
        public Color c;
        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
            foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
            {
                m.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); ;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Prevent control is connected to Photon and represent the localPlayer
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            // failSafe is missing Animator component on GameObject
            if (!ani)
            {
                return;
            }


            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                ani.SetBool("isWalking", false);
            else
                ani.SetBool("isWalking", true);

            // deal with movement
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // prevent negative Speed.
            if (v < 0)
            {
                v = 0;
            }

            // set the Animator Parameters
            //animator.SetFloat("Speed", h * h + v * v);
            ani.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
    }
}