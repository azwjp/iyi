using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Iyi
{
    public abstract class AvaterAnimatorBase : MonoBehaviour
    {
        [SerializeField]
        List<SkinnedMeshRenderer> blendShapes;
        List<(SkinnedMeshRenderer, List<(int, ARKitBlendShapeLocation)>)> blendIndex;
        [SerializeField]
        List<Transform> headBones;
        Quaternion baseHeadRotation = Quaternion.identity;
        [SerializeField]
        Transform avaterPosition;
        Vector3 baseAvaterPosition = Vector3.zero;
        Vector3 rootAvaterPosition = Vector3.zero;
        [SerializeField]
        Transform leftEye;
        Quaternion baseLeftEye = Quaternion.identity;
        Quaternion rootLeftEye = Quaternion.identity;
        [SerializeField]
        Transform rightEye;
        Quaternion baseRightEye = Quaternion.identity;
        Quaternion rootRightEye = Quaternion.identity;

        [SerializeField]
        Transform area;
        Vector3 baseAreaPosition = Vector3.zero;
        Vector3 rootAreaPosition = Vector3.zero;

        Dictionary<ARKitBlendShapeLocation, float> coefficient = new Dictionary<ARKitBlendShapeLocation, float>();
        [SerializeField]
        float bodyCoefficient = 0.01f;


        protected virtual void Start()
        {
            rootAvaterPosition = avaterPosition.localPosition;
            rootLeftEye = leftEye.localRotation;
            rootRightEye = rightEye.localRotation;

            // You can adjust these value to fit your face 
            coefficient.Add(ARKitBlendShapeLocation.MouthFunnel, 0);
            coefficient.Add(ARKitBlendShapeLocation.MouthPucker, 2f);
            coefficient.Add(ARKitBlendShapeLocation.MouthClose, 1.1f);
            coefficient.Add(ARKitBlendShapeLocation.BrowDownLeft, 1.5f);
            coefficient.Add(ARKitBlendShapeLocation.BrowDownRight, 1.5f);
            coefficient.Add(ARKitBlendShapeLocation.BrowInnerUp, 1.5f);
            coefficient.Add(ARKitBlendShapeLocation.MouthFrownLeft, 1.5f);
            coefficient.Add(ARKitBlendShapeLocation.MouthFrownRight, 1.5f);

            blendIndex = blendShapes.Select(smr =>
                (smr, Enumerable.Range(0, smr.sharedMesh.blendShapeCount)
                    .Select(i => (i, smr.sharedMesh.GetBlendShapeName(i)))
                    .Where(p => Enum.IsDefined(typeof(ARKitBlendShapeLocation), p.Item2))
                    .Select((p, index) => (p.Item1, (ARKitBlendShapeLocation)Enum.Parse(typeof(ARKitBlendShapeLocation), p.Item2)))
                    .ToList()
                )
            ).ToList();
        }
        
        protected virtual void Update()
        {
            if (GetFaceData() != null)
            {
                var faceData = GetFaceData();
                blendIndex.ForEach(bi => bi.Item2.ForEach(location =>
                {
                    bi.Item1.SetBlendShapeWeight(location.Item1, Mathf.Clamp((coefficient.ContainsKey(location.Item2) ? coefficient[location.Item2] : 1) * faceData.data[location.Item2] * 100, 0, 100));
                }));

                var pos = faceData.facePosition * bodyCoefficient;
                pos = new Vector3(pos.x, pos.y, -pos.z);
                avaterPosition.localPosition = rootAvaterPosition + baseAvaterPosition + pos;

                var adjustedFaceRotation = ConvertRotation(faceData.faceRotation);
                avaterPosition.transform.localRotation = baseHeadRotation * adjustedFaceRotation;
                var rot = Quaternion.Lerp(Quaternion.identity, baseHeadRotation * adjustedFaceRotation, 1.0f / headBones.Count);
                headBones.ForEach(bone => bone.localRotation = rot);

                var adjustedLeftEyeRotation = ConvertRotation(faceData.leftEyeRotation);
                leftEye.localRotation = adjustedLeftEyeRotation * baseLeftEye * rootLeftEye;
                var adjustedRightEyeRotation = ConvertRotation(faceData.rightEyeRotation);
                rightEye.localRotation = adjustedRightEyeRotation * baseRightEye * rootRightEye;

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DelaySeconds(3f, () =>
                {
                    if (GetFaceData() != null)
                    {
                        var faceData = GetFaceData();
                        var pos = faceData.facePosition * bodyCoefficient;
                        baseAvaterPosition = new Vector3(-pos.x, -pos.y, +pos.z);
                        var adjustedFaceRotation = Quaternion.Inverse(ConvertRotation(faceData.faceRotation));
                        baseHeadRotation = adjustedFaceRotation;
                        var adjustedLeftEyeRotation = Quaternion.Inverse(ConvertRotation(faceData.leftEyeRotation));
                        baseLeftEye = adjustedLeftEyeRotation;
                        var adjustedRightEyeRotation = Quaternion.Inverse(ConvertRotation(faceData.rightEyeRotation));
                        baseRightEye = adjustedRightEyeRotation;
                        area.transform.localRotation = Quaternion.Euler(0, avaterPosition.transform.localRotation.eulerAngles.y, 0);
                    }
                }));

            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(DelaySeconds(3f, () =>
                {
                    if (GetFaceData() != null)
                    {
                        var faceData = GetFaceData();
                        var pos = faceData.facePosition * bodyCoefficient;
                        baseAvaterPosition = new Vector3(-pos.x, -pos.y, +pos.z);
                        var adjustedFaceRotation = ConvertRotation(faceData.faceRotation);
                        baseHeadRotation = Quaternion.Inverse(adjustedFaceRotation);
                        var adjustedLeftEyeRotation = ConvertRotation(faceData.leftEyeRotation);
                        baseLeftEye = Quaternion.Inverse(adjustedLeftEyeRotation);
                        var adjustedRightEyeRotation = ConvertRotation(faceData.rightEyeRotation);
                        baseRightEye = Quaternion.Inverse(adjustedRightEyeRotation);
                    }
                }));

            }
        }

        const float threshould = 20;

        Quaternion ConvertRotation(Quaternion source)
        {
            return new Quaternion(source.x, source.y, -source.z, -source.w);
        }

        private IEnumerator DelaySeconds(float waitTime, Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }

        protected abstract Received GetFaceData();
    }

}
