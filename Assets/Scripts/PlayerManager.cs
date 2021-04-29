using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using Photon.Pun.Demo.PunBasics;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float stepwidth = 0.01f;
        private ArrayList randomcolors;



#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            //setting random colors

            foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
            {
                randomcolors.Add(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)) ;
            }
            photonView.RPC("UpdateColor", RpcTarget.AllBuffered);
            

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }


        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;

                float offset = Random.Range(-5, 5);
                transform.position = new Vector3(offset, 5f, offset);
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (!photonView.IsMine)
            { return;}
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

        }
        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif
        #if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                float offset = Random.Range(-5,5);
                transform.position = new Vector3(offset, 5f, offset);
            }
        }
        #endregion
        
        #region IPun

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
            }
            else
            {
                // Network player, receive data
            }
        }

        [PunRPC]
        void UpdateColor()
        {
            int count = 0;
            foreach (MeshRenderer m in this.GetComponentsInChildren<MeshRenderer>())
            {
                m.material.color = (Color)randomcolors[count++];
            }
        }

    }
    #endregion
}


