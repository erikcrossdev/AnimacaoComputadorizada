using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public AnimationCurve curve;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float CurrentSpeed;
        public float _animationSpeed;
        public Animator _anim;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                var dist = pathCreator.path.GetClosestTimeOnPath(transform.position);
                distanceTravelled += (speed * Time.deltaTime) * curve.Evaluate(dist);
                CurrentSpeed = curve.Evaluate(dist) * 2f;
                _animationSpeed= Mathf.Clamp(CurrentSpeed, 0.9f, 2.5f);
                _anim.speed = _animationSpeed;
                //Debug.Log($"animSpeed: {animSpeed},Current Speed: {CurrentSpeed}, Curve {curve.Evaluate(dist)}");

                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled , endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled , endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}