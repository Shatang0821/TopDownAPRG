
    using UnityEngine;

    public class MoveComponent
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        public MoveComponent(Rigidbody rigidbody,Transform transform)
        {
            _rigidbody = rigidbody;
            _transform = transform;
        }
        
        public void Move(Vector3 vector,float speed)
        {
            Vector3 movement = new Vector3(vector.x, 0,vector.y);

            if (movement != Vector3.zero)
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(movement), 0.3f);
                //_transform.rotation = Quaternion.LookRotation(movement);
            }

            Vector3 currentHorizontalVelocity = GetCurrentHorizontalVelocity();
            _rigidbody.AddForce(movement * speed - currentHorizontalVelocity,
                ForceMode.VelocityChange);
            //_transform.Translate(movement * (speed * Time.deltaTime),Space.World);
        }

        private Vector3 GetCurrentHorizontalVelocity()
        {
            Vector3 horizontalVelocity = _rigidbody.velocity;
            horizontalVelocity.y = 0f;
            return horizontalVelocity;
        }
    }
