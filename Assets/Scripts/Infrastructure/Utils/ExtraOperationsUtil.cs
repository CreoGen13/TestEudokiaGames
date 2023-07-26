using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Utils
{
    public class ExtraOperationsUtil
    {
        /// <summary>
        /// Returns result of comparison between vector1 and vector2
        /// </summary>
        /// <returns>
        /// true if vector1 is less than vector 2
        /// </returns>
        public static bool IsVectorLessAbs(Vector3 vector1, Vector3 vector2)
        {
            var vectorSmallerAbs = new Vector3(
                Mathf.Abs(vector1.x),
                Mathf.Abs(vector1.y),
                Mathf.Abs(vector1.z));
            var vectorLargerAbs = new Vector3(
                Mathf.Abs(vector2.x),
                Mathf.Abs(vector2.y),
                Mathf.Abs(vector2.z));
            return
                vectorSmallerAbs.x < vectorLargerAbs.x &&
                vectorSmallerAbs.y < vectorLargerAbs.y &&
                vectorSmallerAbs.z < vectorLargerAbs.z;
        }

        public static Vector3 GetRandomPlanePosition(GameObject plane)
        {
            List<Vector3> verticesList = new List<Vector3>(plane.GetComponent<MeshFilter>().sharedMesh.vertices);
            Vector3 leftTop = plane.transform.TransformPoint(verticesList[0]);
            Vector3 rightTop = plane.transform.TransformPoint(verticesList[10]);
            Vector3 leftBottom = plane.transform.TransformPoint(verticesList[110]);
            Vector3 rightBottom = plane.transform.TransformPoint(verticesList[120]);
            Vector3 xAxis = rightTop - leftTop;
            Vector3 zAxis = leftBottom - leftTop;
            Vector3 rndPointOnPlane = leftTop + xAxis * Random.value + zAxis * Random.value;
 
            // spawn a sphere on the plane to test the position
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //phere.transform.position = rndPointOnPlane + plane.transform.up * 0.5f;
            
            return rndPointOnPlane;
        }
    }
}