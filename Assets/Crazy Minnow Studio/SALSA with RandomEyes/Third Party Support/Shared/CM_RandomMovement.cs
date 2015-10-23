using UnityEngine;
using System.Collections;
using CrazyMinnow.SALSA;

namespace CrazyMinnow.SALSA
{
	public class CM_RandomMovement : MonoBehaviour 
	{
		public Vector3 rotationAmount = new Vector3(5f, 5f, 5f); // The amount of rotation desired on each euler axis
		public Vector2 rotationUpdateRange = new Vector2(1f, 3f); // A rotation update delay timer will be derived from this range

		public Vector2 movementSpeed = new Vector2(0.1f, 1.5f); // A movement speed will be derived from this range
		public Vector2 movementUpdateRange = new Vector2(0.5f, 1.75f); // A movement speed update delay timer will be derived from this range

		public GameObject[] targets; // The head you want to move

		private float rotationUpdateDelay; // The random rotation update delay derived from the rotationUpdateRange
		private float moveSpeedUpdateDelay; // The random movement speed update delay derived from the movementUpdateRange

		private float[] xStart; // The starting X rotation
		private float[] yStart; // The starting Y rotation
		private float[] zStart; // The starting Z rotation

		private float[] xRot; // The next destination X rotation
		private float[] yRot; // The next destination Y rotation
		private float[] zRot; // The next destination Z rotation

		private float moveSpeed; // The move speed derived from the movementSpeed range

		/// <summary>
		/// Get rotation and delay starting values, and start the update coroutines
		/// </summary>
		void Start () 
		{
			xStart = new float[targets.Length];
			yStart = new float[targets.Length];
			zStart = new float[targets.Length];

			xRot = new float[targets.Length];
			yRot = new float[targets.Length];
			zRot = new float[targets.Length];

			for (int i=0; i<targets.Length; i++)
			{
				xStart[i] = targets[i].transform.rotation.eulerAngles.x; // Get the starting X rotation
				yStart[i] = targets[i].transform.rotation.eulerAngles.y; // Get the starting Y rotation
				zStart[i] = targets[i].transform.rotation.eulerAngles.z; // Get the starting Z rotation
			}

			// Get the starting rotationUpdateDelay
			rotationUpdateDelay = Random.Range(rotationUpdateRange.x, rotationUpdateRange.y);

			// Get the starting speedUpdateDelay
			moveSpeedUpdateDelay = Random.Range(movementUpdateRange.x, movementUpdateRange.y);

			// Get the starting moveSpeed
			moveSpeed = Random.Range(movementSpeed.x, movementSpeed.y);

			StartCoroutine(GetNewRotation(rotationUpdateDelay)); // Start the rotation coroutine
			StartCoroutine(GetNewSpeed(moveSpeedUpdateDelay)); // Start the movement speed coroutine
		}
		
		/// <summary>
		/// Lerp update the heads rotation
		/// </summary>
		void Update () 
		{
			for (int i=0; i<targets.Length; i++)
			{
				targets[i].transform.rotation = Quaternion.Lerp(
					targets[i].transform.rotation, Quaternion.Euler(xRot[i], yRot[i], zRot[i]), Time.deltaTime * moveSpeed);
			}
		}

		/// <summary>
		/// Coroutine to update the desired rotation destination and the rotationUpdateDelay timer
		/// </summary>
		/// <returns>The new rotation.</returns>
		/// <param name="updateDelay">Update delay.</param>
		IEnumerator GetNewRotation(float updateDelay)
		{
			while (true)
			{
				for (int i=0; i<targets.Length; i++)
				{
					// A random X range surrounding the starting X rotation
					this.xRot[i] = Random.Range((this.xStart[i] - this.rotationAmount.x/2), (this.xStart[i] + this.rotationAmount.x/2));
					// A random Y range surrounding the starting Y rotation
					this.yRot[i] = Random.Range((this.yStart[i] - this.rotationAmount.y/2), (this.yStart[i] + this.rotationAmount.y/2));
					// A random Z range surrounding the starting Z rotation
					this.zRot[i] = Random.Range((this.zStart[i] - this.rotationAmount.z/2), (this.zStart[i] + this.rotationAmount.z/2));
				}

				yield return new WaitForSeconds(updateDelay);

				// Update the rotationUpdateDelay timer
				this.rotationUpdateDelay = Random.Range(this.rotationUpdateRange.x, this.rotationUpdateRange.y);
			}
		}

		/// <summary>
		/// Coroutine to update the desired movement speed and the moveSpeedUpdateDelay timer
		/// </summary>
		/// <returns>The new speed.</returns>
		/// <param name="updateFrequency">Update frequency.</param>
		IEnumerator GetNewSpeed(float updateFrequency)
		{
			while (true)
			{
				// A random movement speed surrounding the movementSpeed
				this.moveSpeed = Random.Range(this.movementSpeed.x, this.movementSpeed.y);

				yield return new WaitForSeconds(updateFrequency);

				// Update the moveSpeedUpdateDelay timer
				this.moveSpeedUpdateDelay = Random.Range(this.movementUpdateRange.x, this.movementUpdateRange.y);
			}
		}
	}
}