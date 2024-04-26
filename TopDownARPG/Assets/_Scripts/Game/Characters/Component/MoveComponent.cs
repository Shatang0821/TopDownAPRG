
    using UnityEngine;

    public class MoveComponent
    {
        private Transform _transform;

        public MoveComponent(Transform transform)
        {
            _transform = transform;
        }
        
        public void Move(Vector3 vector)
        {
            Vector3 movement = new Vector3(vector.x, 0,vector.y);

            if (movement != Vector3.zero)
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            }
            _transform.Translate(movement * (5 * Time.deltaTime),Space.World);
        }
    }
