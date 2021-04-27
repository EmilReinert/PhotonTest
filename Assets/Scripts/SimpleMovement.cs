using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class SimpleMovement : MonoBehaviour
    {
        Animator ani;
        float stepwidth = 0.1f;
        public Color c;
        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
            foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
            {
                m.material.color = c;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.Translate(new Vector3(1, 0, 0) * stepwidth);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.Translate(new Vector3(1, 0, 0) * -stepwidth);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.Translate(new Vector3(0, 0, 1) * -stepwidth);
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                transform.Translate(new Vector3(0, 0, 1) * stepwidth);
            }

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                ani.SetBool("isWalking", false);
            else
                ani.SetBool("isWalking", true);
        }
    }
}