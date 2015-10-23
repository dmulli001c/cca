using UnityEngine;

namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
    
        public GameObject enemy;                // The enemy prefab to be spawned.
        public float spawnTime = 3f;            // How long between each spawn.
		public int numMaxOfObjects = 6;         // Max number of objects.  
		public GameObject[] gameObjects;     	// Array of created object.


		private int counterObjects = 0;         // Objects counter.
      



        void Start ()
        {
			gameObjects = new GameObject[numMaxOfObjects];

            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating ("Spawn", spawnTime, spawnTime);
        }




        void Spawn ()
        {

			int number = Random.Range (1, Screen.width);			
			//Paint out of screen, finish (Check if the point is visible in camara)
			Vector3 pos = new Vector3 (number, 100, 0);
			Vector3 posWorld = Camera.main.ScreenToWorldPoint (pos);


			//if have less object of allowed
			if (counterObjects < numMaxOfObjects) {
				counterObjects++;
				// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
				GameObject auxObject = Instantiate (enemy, posWorld, Quaternion.identity) as GameObject;

				gameObjects [counterObjects] = auxObject;
			}

        }
    }
}