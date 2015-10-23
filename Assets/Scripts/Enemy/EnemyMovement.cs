using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.    
        NavMeshAgent nav;               // Reference to the nav mesh agent.
		//Paint in center
		Vector3 pos = new Vector3 (0, 0, 0);

        void Awake ()
        {
            // Set up the references.   
            nav = GetComponent <NavMeshAgent> ();
        }


        void Update ()
        {

            // ... set the destination of the nav mesh agent to the player.
			nav.SetDestination (pos);
            
        }
    }
}