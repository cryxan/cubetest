using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatchX
{
    public class MatchXCube : MonoBehaviour
    {
        [SerializeField] private MatchXElement elementGameObjectPrefab;
        [SerializeField] private Vector3Int cubeDimensions;
        [SerializeField] private float elementSide = 1.0f;
        
        private MatchXElement[,,] matchElements;

        public void Start()
        {
            var spawnOffset = elementSide / 2.0f;
            matchElements = new MatchXElement[cubeDimensions.x, cubeDimensions.y, cubeDimensions.z];
            for (int x = 0; x < cubeDimensions.x; x++)
            {
                for (int y = 0; y < cubeDimensions.y; y++)
                {
                    for (int z = 0; z < cubeDimensions.z; z++)
                    {
                        var curObject = Instantiate(elementGameObjectPrefab, 
                            new Vector3(x*elementSide+spawnOffset, y*elementSide+spawnOffset,z*elementSide+spawnOffset), 
                            Quaternion.identity, 
                            transform);

                        curObject.SetColour(RandomCubeColour());
                        curObject.Position = new Vector3Int(x, y, z);
                        matchElements[x, y, z] = curObject;
                    }
                }
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit hitInfo;
                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    var hitComponent = hitInfo.collider.gameObject.GetComponent<MatchXElement>();
                    Debug.Log($"Hit element {hitComponent.Position} - {hitComponent.Colour}");
                    FindConnectedCubes(hitComponent.Position);
                }
            }
        }

        private Vector3Int[] cubeOffsets = new[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(0, 0, -1),
        }; 
        
        private void FindConnectedCubes(Vector3Int startPosition)
        {
            var hitDictionary = new Dictionary<Vector3Int, MatchXElement>();
            var matchElement = matchElements[startPosition.x, startPosition.y, startPosition.z];
            var hitColour = matchElement.Colour;
            
            hitDictionary = FindMatchs(hitDictionary, startPosition, hitColour);

            if (hitDictionary.Count <= 1) return;
            
            Debug.Log($"Found {hitDictionary.Count} matches");

            foreach (var hit in hitDictionary)
            {
                var hitValue = hit.Value;
                var pos = hitValue.gameObject.transform.position;
                pos.y += 10;
                hitValue.gameObject.SetActive(false);
            }
        }

        private Dictionary<Vector3Int, MatchXElement> FindMatchs(Dictionary<Vector3Int, MatchXElement> hits, Vector3Int startPosition, cubeColour hitColour)
        {
            if (!IsInBounds(startPosition) ||
                hits.ContainsKey(startPosition)) return hits;

            var matchElement = matchElements[startPosition.x, startPosition.y, startPosition.z];

            if (hitColour != matchElement.Colour) return hits;
            
            hits.Add(startPosition, matchElement);

            foreach (var offset in cubeOffsets)
            {
                hits = FindMatchs(hits, startPosition + offset, hitColour);
            }

            return hits;
        }

        private bool IsInBounds(Vector3Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.z >= 0 &&
                   position.x < cubeDimensions.x && position.y < cubeDimensions.y && position.z < cubeDimensions.z;
        }
        private cubeColour RandomCubeColour()
        {
            return (cubeColour)Random.Range(0, (int)cubeColour.numCols);
        }
        public void OnDrawGizmos()
        {
            Matrix4x4 prevMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.green;

            for (var xFaceDepth = 0; xFaceDepth <= cubeDimensions.x; xFaceDepth += cubeDimensions.x)
            {
                for (var y = 0; y <= cubeDimensions.y; y++)
                {
                    Gizmos.color = y == 0 || y == cubeDimensions.y ? Color.white : Color.green;
                    Gizmos.DrawLine(
                        new Vector3(xFaceDepth * elementSide, y * elementSide, 0),
                        new Vector3(xFaceDepth * elementSide, y * elementSide, cubeDimensions.z * elementSide));
                }

                for (var z = 0; z <= cubeDimensions.z; z++)
                {
                    Gizmos.color = z == 0 || z == cubeDimensions.y ? Color.white : Color.green;
                    Gizmos.DrawLine(
                        new Vector3(xFaceDepth * elementSide, 0, z * elementSide),
                        new Vector3(xFaceDepth * elementSide, cubeDimensions.y * elementSide, z * elementSide));
                }
            }
            
            
            for (var yFaceDepth = 0; yFaceDepth <= cubeDimensions.y; yFaceDepth += cubeDimensions.y)
            {
                Gizmos.color = Color.red;
                for (var x = 1; x < cubeDimensions.x; x++)
                {
                    Gizmos.DrawLine(
                        new Vector3(x*elementSide, yFaceDepth * elementSide, 0),
                        new Vector3(x*elementSide, yFaceDepth * elementSide, cubeDimensions.z*elementSide));
                }

                for (var z = 0; z <= cubeDimensions.z; z++)
                {
                    Gizmos.color = z == 0 || z == cubeDimensions.y ? Color.white : Color.red;
                    Gizmos.DrawLine(
                        new Vector3(0, yFaceDepth * elementSide, z*elementSide),
                        new Vector3(cubeDimensions.x*elementSide, yFaceDepth * elementSide, z*elementSide));
                }
            }
            
            Gizmos.color = Color.blue;
            for (var zFaceDepth = 0; zFaceDepth <= cubeDimensions.y; zFaceDepth += cubeDimensions.y)
            {
                for (var x = 1; x < cubeDimensions.x; x++)
                {
                    Gizmos.DrawLine(
                        new Vector3(x*elementSide, 0, zFaceDepth * elementSide),
                        new Vector3(x*elementSide, cubeDimensions.y*elementSide, zFaceDepth * elementSide));
                }

                for (var y = 1; y < cubeDimensions.y; y++)
                {
                    Gizmos.DrawLine(
                        new Vector3(0, y*elementSide,  zFaceDepth * elementSide),
                        new Vector3(cubeDimensions.x*elementSide, y*elementSide,  zFaceDepth * elementSide));
                }
            }
            Gizmos.matrix = prevMatrix;
        }
    }
}