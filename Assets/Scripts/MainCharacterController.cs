using UnityEngine;
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

        void Awake()
        {
            _isoTransform = this.GetOrAddComponent<IsoTransform>();
            xOffsets.Add(1);
            xOffsets.Add(-1);
            zOffsets.Add(1);
            zOffsets.Add(-1);
            characterPosition = _isoTransform.Position;
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
            for (int x = 1; x <= movementRange; x++)
            {
                foreach (float offset in xOffsets)
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(
                        Resources.Load("g3544"),
                        new Vector3(0, 0, 0),
                        Quaternion.identity);

                    obj.AddComponent<BoxCollider>();

                    IsoTransform newIsoTransform = obj.GetComponent(typeof(IsoTransform)) as IsoTransform;
                    newIsoTransform.Position = new Vector3(characterPosition.x + offset * x, characterPosition.y, characterPosition.z);
                    movementTiles.Add(obj);
                }

                foreach (float offset in zOffsets)
                {
                    GameObject obj = (GameObject)GameObject.Instantiate(
                        Resources.Load("g3544"),
                        new Vector3(0, 0, 0),
                        Quaternion.identity);

                    obj.AddComponent<BoxCollider>();

                    IsoTransform newIsoTransform = obj.GetComponent(typeof(IsoTransform)) as IsoTransform;
                    newIsoTransform.Position = new Vector3(characterPosition.x, characterPosition.y, characterPosition.z + offset * x);
                    movementTiles.Add(obj);
                }
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.name == "g4846")
                    {
                        Debug.Log("Clicked on player");
                        if (movementTiles.Count == 0) {
                            SpawnMovementTiles();
                        } else
                        {
                            DestroyMovementTiles();
                        }
                    }
                    if(movementTiles.Contains(hit.transform.gameObject))
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
