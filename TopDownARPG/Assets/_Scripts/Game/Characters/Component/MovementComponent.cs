
    using UnityEngine;

    public class MovementComponent
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        public MovementComponent(Rigidbody rigidbody,Transform transform)
        {
            _rigidbody = rigidbody;
            _transform = transform;
        }
        
        public void Move(Vector3 vector,float speed,float rotationSpeed,bool rotation = true)
        {
            Vector3 movement = new Vector3(vector.x, 0,vector.y);
            if (rotation)
            {
                RotateTowards(_transform, movement, rotationSpeed);
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
        
        private Vector3 GetCurrentHorizontalVelocity()
        {
            Vector3 horizontalVelocity = _rigidbody.velocity;
            horizontalVelocity.y = 0f;
            return horizontalVelocity;
        }
        
        
    }
