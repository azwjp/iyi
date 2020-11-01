using System;
using System.Collections.Generic;
using UnityEngine;

namespace Iyi
{
    public class Received
    {
        public DateTime timestamp { get; private set; }
        public Dictionary<ARKitBlendShapeLocation, float> data { get; private set; }
        public Vector3 facePosition { get; private set; }
        public Quaternion faceRotation { get; private set; }
        public Vector3 leftEyePosition { get; private set; }
        public Quaternion leftEyeRotation { get; private set; }
        public Vector3 rightEyePosition { get; private set; }
        public Quaternion rightEyeRotation { get; private set; }

        public Received(
            DateTime timestamp,
            Vector3 facePosition,
            Quaternion faceRotation,
            Vector3 leftEyePosition,
            Quaternion leftEyeRotation,
            Vector3 rightEyePosition,
            Quaternion rightEyeRotation,
            Dictionary<ARKitBlendShapeLocation, float> data
        )
        {
            this.timestamp = timestamp;
            this.facePosition = facePosition;
            this.faceRotation = faceRotation;
            this.leftEyePosition = leftEyePosition;
            this.leftEyeRotation = leftEyeRotation;
            this.rightEyePosition = rightEyePosition;
            this.rightEyeRotation = rightEyeRotation;
            this.data = data;
        }
    }
}
