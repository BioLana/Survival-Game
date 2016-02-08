using UnityEngine;

namespace AssemblyCSharp
{
    public class TransformVectors
    {
        Transform transform;

        public TransformVectors(Transform trans)
        {
            transform = trans;
        }

        public Vector3 Up { get { return transform.up; } }
        public Vector3 Down { get { return -Up; } }
        public Vector3 Right { get { return transform.right; } }
        public Vector3 Left { get { return -Right; } }
        public Vector3 Forward { get { return transform.forward; } }
        public Vector3 Back { get { return -Forward; } }


        public Vector3 UpForward { get { return Up + Forward; } }
        public Vector3 DownForward { get { return Down + Forward; } }
        public Vector3 UpBack { get { return Up + Back; } }
        public Vector3 DownBack { get { return Down + Back; } }


        public Vector3 UpRight { get { return Up + Right; } }
        public Vector3 DownRight { get { return Down + Right; } }
        public Vector3 UpLeft { get { return Up + Left; } }
        public Vector3 DownLeft { get { return Down + Left; } }


        public Vector3 UpRightForward { get { return Up + Right + Forward; } }
        public Vector3 DownRightForward { get { return Down + Right + Forward; } }

        public Vector3 UpLeftForward { get { return Up + Left + Forward; } }
        public Vector3 DownLeftForward { get { return Down + Left + Forward; } }

        public Vector3 UpRightBack { get { return Up + Right + Back; } }
        public Vector3 UpLeftBack { get { return Up + Left + Back; } }

        public Vector3 DownRightBack { get { return Down + Right + Back; } }
        public Vector3 DownLeftBack { get { return Down + Left + Back; } }

    }
}

