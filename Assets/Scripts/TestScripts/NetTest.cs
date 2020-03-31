using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mirror
{
    public class NetTest : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Debug.Log("player has join");
            base.OnServerAddPlayer(conn);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
