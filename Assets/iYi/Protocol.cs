using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Iyi
{
    public class Protocol
    {
        public static Vector3 ByteToVector3(byte[] value, int startIndex)
        {
            return new Vector3(
                    BitConverter.ToSingle(value, startIndex),
                    BitConverter.ToSingle(value, startIndex + 4),
                    BitConverter.ToSingle(value, startIndex + 8)
                );
        }

        public static Quaternion ByteToQuaternion(byte[] value, int startIndex)
        {
            return new Quaternion(
                    BitConverter.ToSingle(value, startIndex),
                    BitConverter.ToSingle(value, startIndex + 4),
                    BitConverter.ToSingle(value, startIndex + 8),
                    BitConverter.ToSingle(value, startIndex + 12)
                );
        }

        public static Received ToReceived(byte[] data)
        {
            var ms = new MemoryStream(data);

            var datetime = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

            var facePosition = ByteToVector3(data, 8);
            var faceRotation = ByteToQuaternion(data, 20);
            var leftEyePosition = ByteToVector3(data, 36);
            var leftEyeRotation = ByteToQuaternion(data, 48);
            var rightEyePosition = ByteToVector3(data, 64);
            var rightEyeRotation = ByteToQuaternion(data, 76);


            ms.Seek(92, SeekOrigin.Current);
            var dic = new Dictionary<ARKitBlendShapeLocation, float>();
            while (ms.Position < ms.Length)
            {
                byte length = data[ms.Position];
                ms.Seek(1, SeekOrigin.Current);

                byte[] buf;
                buf = new byte[length];
                ms.Read(buf, 0, length);

                var name = Encoding.UTF8.GetString(buf);
                var value = BitConverter.ToSingle(data, (int)ms.Position);
                ms.Seek(4, SeekOrigin.Current);

                dic.Add((ARKitBlendShapeLocation)Enum.Parse(typeof(ARKitBlendShapeLocation), name, false), value);
            }
            return new Received(datetime,
                    facePosition,
                    faceRotation,
                    leftEyePosition,
                    leftEyeRotation,
                    rightEyePosition,
                    rightEyeRotation,
                    dic
            );
        }

    }
}
