using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;

namespace UltimateIsometricToolkit.controller
{
    [AddComponentMenu("UIT/CharacterController/Simple Controller")]
    public class MainCharacterController : MonoBehaviour
    {
        // Object instantiations and such.
        private IsoTransform _isoTransform;
        private Vector3 characterPosition;
        public GameObject availableMovingSpace;

        // Movement related variables
        public float movementRange = 3;
        public List<GameObject> movementTiles = new List<GameObject>();
        public List<float> xOffsets = new List<float>();
        public List<float> zOffsets = new List<float>();

        private bool limitedForwardMovementX_Pos = false;
        private bool limitedForwardMovementX_Neg = false;
        private bool limitedForwardMovementZ_Pos = false;
        private bool limitedForwardMovementZ_Neg = false;

        private static bool created = false;


        void Awake()
        {
            _isoTransform = this.GetOrAddComponent<IsoTransform>();
            xOffsets.Add(1);
            xOffsets.Add(-1);
            zOffsets.Add(1);
            zOffsets.Add(-1);
            characterPosition = _isoTransform.Position;
        }

        public bool SpawnIndividualTile(float xPosition, float zPosition)
        {
            Vector3 newPosition = new Vector3(xPosition, characterPosition.y, zPosition);
            bool obstacleInTheWay = false;
            foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Debug.Log("Found obstacle.");
                Debug.Log("Obstacle Position: " + ((IsoTransform)obstacle.GetComponent(typeof(IsoTransform))).Position);
                Debug.Log("New Position: " + newPosition);
                if (((IsoTransform)obstacle.GetComponent(typeof(IsoTransform))).Position == newPosition)
                {
                    obstacleInTheWay = true;
                }
            }

            if (!obstacleInTheWay)
            {
                GameObject obj = (GameObject)GameObject.Instantiate(
                            Resources.Load("g3544"),
                            new Vector3(0, 0, 0),
                            Quaternion.identity);

                obj.AddComponent<BoxCollider>();

                IsoTransform newIsoTransform = obj.GetComponent(typeof(IsoTransform)) as IsoTransform;
                newIsoTransform.Position = newPosition;
                movementTiles.Add(obj);
            }
            else
            {
                Debug.Log("Hit an obstacle!");
            }
            return obstacleInTheWay;
        }

        public void DestroyMovementTiles()
        {
            foreach(GameObject movementTile in movementTiles)
            {
                GameObject.Destroy(movementTile);
            }
            movementTiles = new List<GameObject>();
    }

    public void SpawnMovementTiles()
        {
            limitedForwardMovementX_Pos = false;
            limitedForwardMovementX_Neg = false;
            limitedForwardMovementZ_Pos = false;
            limitedForwardMovementZ_Neg = false;

            for (float range = 1; range <= movementRange; range++)
            {
                foreach (float offset in xOffsets)
                {
                    if (!limitedForwardMovementX_Pos && offset == 1) limitedForwardMovementX_Pos = SpawnIndividualTile(characterPosition.x + offset * range, characterPosition.z);
                    if (!limitedForwardMovementX_Neg && offset == -1) limitedForwardMovementX_Neg = SpawnIndividualTile(characterPosition.x + offset * range, characterPosition.z);
                    for (float leftOverRange = range; leftOverRange <= movementRange - range; leftOverRange++)
                    {
                        foreach (float zOffset in zOffsets)
                        {
                            SpawnIndividualTile(characterPosition.x + offset * range, characterPosition.z + zOffset * leftOverRange);
                        }
                    }
                }

                foreach (float offset in zOffsets)
                {
                    if (!limitedForwardMovementZ_Pos && offset == 1) limitedForwardMovementZ_Pos = SpawnIndividualTile(characterPosition.x, characterPosition.z + offset * range);
                    if (!limitedForwardMovementZ_Neg && offset == -1) limitedForwardMovementZ_Neg = SpawnIndividualTile(characterPosition.x, characterPosition.z + offset * range);
                    for (float leftOverRange = range; leftOverRange <= movementRange - range; leftOverRange++)
                    {
                        foreach (float xOffset in xOffsets)
                        {
                            SpawnIndividualTile(characterPosition.x + xOffset * leftOverRange, characterPosition.z + offset * range);
                        }
                    }
                }
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if (hit.transform.name == "Main_Character")
                    {
                        Debug.Log("Clicked on player");
                        if (movementTiles.Count == 0)
                        {
                            SpawnMovementTiles();
                        }
                        else
                        {
                            DestroyMovementTiles();
                        }
                    }
                    if (movementTiles.Contains(hit.transform.gameObject))
                    {
                        Debug.Log("Clicked a coin!");
                        IsoTransform coinLocation = hit.transform.gameObject.GetComponent(typeof(IsoTransform)) as IsoTransform;
                        _isoTransform.Position = coinLocation.Position;
                        DestroyMovementTiles();
                    }
                }
                else
                {
                    Debug.Log("Not Raycast!");
                }
            }
            characterPosition = _isoTransform.Position;
        }
    }
}
