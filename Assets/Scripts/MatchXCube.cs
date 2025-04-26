using System;
using UnityEngine;

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
            var colVal = 0;
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

                        curObject.SetColour(getColor(colVal++));
                        matchElements[x, y, z] = curObject;
                    }
                }
            }
        }

        private Color getColor(int colIndex)
        {
            return (colIndex % 4) switch
            {
                0 => Color.red,
                1 => Color.blue,
                2 => Color.green,
                _ => Color.yellow
            };
        }
        public void OnDrawGizmos()
        {
            Matrix4x4 prevMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            for (var x = 0; x <= cubeDimensions.x; x++)
            {
                for (var y = 0; y <= cubeDimensions.y; y++)
                {
                    if (x == 0 || x == cubeDimensions.x || y == 0 || y == cubeDimensions.y)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(new Vector3(x * elementSide, y * elementSide, 0),
                            new Vector3(x * elementSide, y * elementSide, cubeDimensions.z * elementSide));
                    }

                    for (var z = 0; z <= cubeDimensions.z; z++)
                    {
                        if (x == 0 || x == cubeDimensions.x || z == 0 || z == cubeDimensions.z)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(new Vector3(x * elementSide, 0, z * elementSide),
                                new Vector3(x * elementSide, cubeDimensions.y * elementSide, z * elementSide));
                        }

                        if (y == 0 || y == cubeDimensions.y || z == 0 || z == cubeDimensions.z)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawLine(new Vector3(0, y * elementSide, z * elementSide),
                                new Vector3(cubeDimensions.x * elementSide, y * elementSide, z * elementSide));
                        }

                    }
                }
            }
            
            Gizmos.matrix = prevMatrix;
        }
    }
}