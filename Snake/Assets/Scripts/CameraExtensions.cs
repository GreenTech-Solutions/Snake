using System;
using UnityEngine;

namespace Assets
{
    public static class CameraExtensions
    {
        public static Bounds OrthographicBounds(this Camera camera)
        {
            String sdf;
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
    }
}
