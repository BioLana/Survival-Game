using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class CustomCollision
    {
        bool Front, Back, Up, Down, Left, Right, 
        UpFront, UpBack, DownFront, DownBack, 
        UpLeft, UpRight, DownLeft, DownRight, 
        UpFrontLeft, UpFrontRight, UpBackLeft, UpBackRight, 
        DownFrontLeft, DownFrontRight, DownBackLeft, DownBackRight;

        public int NumHits;
        TransformVectors transVect;
        //public Vector3 CollNormal = new Vector3(0.0f, 0.0f, 0.0f);

        RaycastHit Front_hit, Back_hit, Up_hit, Down_hit, Left_hit, Right_hit,
        UpFront_hit, UpBack_hit, DownFront_hit, DownBack_hit,
        UpLeft_hit, UpRight_hit, DownLeft_hit, DownRight_hit,
        UpFrontLeft_hit, UpFrontRight_hit, UpBackLeft_hit, UpBackRight_hit,
        DownFrontLeft_hit, DownFrontRight_hit, DownBackLeft_hit, DownBackRight_hit;

        public CustomCollision(Transform trans)
        {
            transVect = new TransformVectors( trans );
        }

        public void GenerateHits(Vector3 playerPosition, 
            float playerDepthCast, float playerVertCast, float playerHorizCast,
            float playerDiagCast
            )
        {
            Front = Physics.Raycast(playerPosition, transVect.Forward, 
                out Front_hit, playerDepthCast);
            Back = Physics.Raycast(playerPosition, transVect.Back,
                out Back_hit, playerDepthCast);
            Up = Physics.Raycast(playerPosition, transVect.Up, 
                out Up_hit, playerVertCast, LayerMask.NameToLayer("PlayerHead"));
            Down = Physics.Raycast(playerPosition, transVect.Down,
                out Down_hit, playerVertCast);
            Left = Physics.Raycast(playerPosition, transVect.Left,
                out Left_hit, playerHorizCast);
            Right = Physics.Raycast(playerPosition, transVect.Right,
                out Right_hit, playerHorizCast);
            UpFront = Physics.Raycast(playerPosition, transVect.UpForward, 
                out UpFront_hit, playerDiagCast);
            DownFront= Physics.Raycast(playerPosition, transVect.DownForward,
                out DownFront_hit, playerDiagCast);
            UpBack = Physics.Raycast(playerPosition, transVect.UpBack,
                out UpBack_hit, playerDiagCast);
            DownBack = Physics.Raycast(playerPosition, transVect.DownBack,
                out DownBack_hit, playerDiagCast);
            UpRight = Physics.Raycast(playerPosition, transVect.UpRight, 
                out UpRight_hit, playerDiagCast);
            DownRight = Physics.Raycast(playerPosition, transVect.DownRight,
                out DownRight_hit, playerDiagCast);
            UpLeft = Physics.Raycast(playerPosition, transVect.UpLeft,
                out UpLeft_hit, playerDiagCast);
            DownLeft = Physics.Raycast(playerPosition, transVect.DownLeft,
                out DownLeft_hit, playerDiagCast);
            UpFrontRight = Physics.Raycast(playerPosition, transVect.UpRightForward, 
                out UpFrontRight_hit, playerDiagCast);
            UpFrontLeft = Physics.Raycast(playerPosition, transVect.UpLeftForward,
                out UpFrontLeft_hit, playerDiagCast);
            DownFrontRight = Physics.Raycast(playerPosition, transVect.DownRightForward,
                out DownFrontRight_hit, playerDiagCast);
            DownFrontLeft= Physics.Raycast(playerPosition, transVect.DownLeftForward,
                out DownFrontLeft_hit, playerDiagCast);
            UpBackRight = Physics.Raycast(playerPosition, transVect.UpRightBack, 
                out UpBackRight_hit, playerDiagCast);
            UpBackLeft = Physics.Raycast(playerPosition, transVect.UpLeftBack,
                out UpBackLeft_hit, playerDiagCast);
            DownBackRight = Physics.Raycast(playerPosition, transVect.DownRightBack,
                out DownBackRight_hit, playerDiagCast);
            DownBackLeft= Physics.Raycast(playerPosition, transVect.DownLeftBack,
                out DownBackLeft_hit, playerDiagCast);

            DebugRayCast(playerPosition);

            CountHits();
        }

        public Vector3 FindCollisionNormal()
        {
            Vector3 CollNormal = new Vector3(0.0f, 0.0f, 0.0f);
            
            CollNormal = Up_hit.normal + Down_hit.normal + Left_hit.normal + 
                Right_hit.normal + Front_hit.normal + Back_hit.normal +
                UpRight_hit.normal + UpLeft_hit.normal + DownRight_hit.normal +
                DownLeft_hit.normal + UpFront_hit.normal + DownFront_hit.normal +
                UpBack_hit.normal + DownBack_hit.normal;

            return CollNormal;
        }

        void CountHits()
        {
            NumHits = 0;

            if (Up) {++NumHits; Debug.Log("Up collision: " + Up);}
            if (Down) {++NumHits; Debug.Log("Down collision: " + Down);}
            if (Left) {++NumHits; Debug.Log("Left collision: " + Left);}
            if (Right) {++NumHits; Debug.Log("Right collision: " + Right);}
            if (Front) {++NumHits; Debug.Log("Front collision: " + Front);}
            if (Back) {++NumHits; Debug.Log("Back collision: " + Back);}
            if (UpFront) {++NumHits; Debug.Log("UpFront collision: " + UpFront);}
            if (UpBack) {++NumHits; Debug.Log("UpBack collision: " + UpBack);}
            if (DownFront) {++NumHits; Debug.Log("DownFront collision: " + DownFront);}
            if (DownBack) {++NumHits; Debug.Log("DownBack collision: " + DownBack);}
            if (UpLeft) {++NumHits; Debug.Log("UpLeft collision: " + UpLeft);}
            if (UpRight) {++NumHits; Debug.Log("UpRight collision: " + UpRight);}
            if (DownLeft) {++NumHits; Debug.Log("DownLeft collision: " + DownLeft);}
            if (DownRight) {++NumHits; Debug.Log("DownRight collision: " + DownRight);}
            if (UpFrontLeft) {++NumHits; Debug.Log("UpFrontLeft collision: " + UpFrontLeft);}
            if (UpFrontRight) {++NumHits; Debug.Log("UpFrontRight collision: " + UpFrontRight);}
            if (DownFrontLeft) {++NumHits; Debug.Log("DownFrontLeft collision: " + DownFrontLeft);}
            if (DownFrontRight) {++NumHits; Debug.Log("DownFrontRight collision: " + DownFrontRight);}
            if (UpBackLeft) {++NumHits; Debug.Log("UpBackLeft collision: " + UpBackLeft);}
            if (UpBackRight) {++NumHits; Debug.Log("UpBackRight collision: " + UpBackRight);}
            if (DownBackLeft) {++NumHits; Debug.Log("DownBackLeft collision: " + DownBackLeft);}
            if (DownBackRight) {++NumHits; Debug.Log("DownBackRight collision: " + DownBackRight);}
        }

        void DebugRayCast(Vector3 playerPosition)
        {
            Debug.DrawRay(playerPosition, transVect.Down, Color.red, 2.0f);
            Debug.DrawRay(playerPosition, transVect.Up, Color.red, 2.0f);
            Debug.DrawRay(playerPosition, transVect.DownLeft, Color.red, 2.0f);
            Debug.DrawRay(playerPosition, transVect.DownRight, Color.red, 2.0f);
            Debug.DrawRay(playerPosition, transVect.DownForward, Color.red, 2.0f);
            Debug.DrawRay(playerPosition, transVect.DownBack, Color.red, 2.0f);
            
        }
    }
}

