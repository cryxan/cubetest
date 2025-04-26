using System;
using UnityEngine;
using Random = System.Random;

namespace MatchX
{
    public class MatchXElement : MonoBehaviour
    {
        private float deltaTime = 0.0f;
        private const float delay = 1.0f;
        private Random random = new Random();

        private Material objectMaterial;
        
        public void SetColour(Color newColor)
        {
            objectMaterial ??= gameObject.GetComponent<Renderer>().material;
            objectMaterial.color = newColor;
        }

        /*
        public void Update()
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > delay)
            {
                deltaTime -= delay;
                
                SetColour(new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1.0f));
            }
        }
        */
    }
}