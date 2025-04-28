using UnityEngine;

namespace MatchX
{
    public class MatchXElement : MonoBehaviour
    {
        public cubeColour Colour { get; private set; }
        public Vector3Int Position;
        
        private Material objectMaterial;
        public void SetColour(cubeColour newColor)
        {
            Colour = newColor;
            objectMaterial ??= gameObject.GetComponent<Renderer>().material;
            objectMaterial.color = getColor(newColor);
        }
        
        private Color getColor(cubeColour colIndex)
        {
            return (colIndex) switch
            {
                cubeColour.red => Color.red,
                cubeColour.blue => Color.blue,
                cubeColour.green => Color.green,
                _ => Color.yellow
            };
        }
    }
}