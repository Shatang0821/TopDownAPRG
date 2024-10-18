
    using System;
    using UnityEngine;

    public class MovementComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    
        public void Move(Vector3 vector,float speed,float rotationSpeed,bool rotation = true)
        {
            Vector3 movement = new Vector3(vector.x, 0,vector.y);
            if (rotation)
            {
                RotateTowards(transform, movement, rotationSpeed);
            }
            Vector3 currentHorizontalVelocity = GetCurrentHorizontalVelocity();
            _rigidbody.AddForce(movement * speed - currentHorizontalVelocity,
                ForceMode.VelocityChange);
        }
        
        /// <summary>
        /// 指定されたTransformの回転を目標方向へ滑らかに変更します。
        /// </summary>
        /// <param name="transform">回転させるTransform</param>
        /// <param name="targetDirection">目標方向</param>
        /// <param name="rotationSpeed">回転速度（0から1の間で指定）</param>
        public void RotateTowards(Transform transform, Vector3 targetDirection, float rotationSpeed)
        {
            if (targetDirection == Vector3.zero) return; // 0ベクトルの場合は回転しない

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            if (rotationSpeed == 0)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed); 
            }
            
        }
        
        /// <summary>
        /// 水平方向の速度を取得します。Y軸を無視します。
        /// </summary>
        /// <returns></returns>
        private Vector3 GetCurrentHorizontalVelocity()
        {
            Vector3 horizontalVelocity = _rigidbody.velocity;
            horizontalVelocity.y = 0f;
            return horizontalVelocity;
        }
        
        
    }
