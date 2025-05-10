using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.Events;
using _Project.Scripts.Main.InputSystem;
using _Project.Scripts.Main.Utilities.Scripts;
using _Project.Scripts.Main.Utilities.Scripts.EventBus;
using UnityEngine;

namespace _Project.Scripts.Main.Game.PlayerTurn.Systems
{
    public class TargetSystem : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private LayerMask _entityLayer;
  
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Vector2 _mousePointer;
        
        private float _curveHeight = 2f;
        public bool IsTargeting { get; private set; }

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

    
        private void OnEnable()
        {
            _inputReader.PointEvent += OnPoint;
        }

        private void OnDisable()
        {
            _inputReader.PointEvent -= OnPoint;
        }

        public void StartTargeting(Vector2 startPosition)
        {
            _startPosition = startPosition;
            IsTargeting = true;
        }

        public void StopTargeting()
        {
            IsTargeting = false;
            _endPosition = default;
            ClearArrows();
        }
        
        private void Update()
        {
            if (!IsTargeting)
                return;
            
            if (_endPosition != GetMouseWorldPosition())
            {
                ClearArrows();
                DrawArrows();
                if (IsTouchEntity(out IEntity entity) && entity is Player)
                    EventBus<ArrowGrayColorEvent>.Raise(new ArrowGrayColorEvent());
                else
                    EventBus<ArrowDefaultColorEvent>.Raise(new ArrowDefaultColorEvent());
            }
        }

        private void ClearArrows()
        {
            PrefabPoolManager.Instance.RemoveAllFromParent(transform);
        }
        
        private void DrawArrows()
        {
            Vector3 p0 = _startPosition;

            _endPosition = GetMouseWorldPosition();
            Vector3 p2 = _endPosition;

            // Compute a midpoint for the curve with added height
            Vector3 controlPoint = (p0 + p2) / 2 + Vector3.up * _curveHeight;
        
            float r = Vector3.Distance(p0, p2);
            int length = (int)r;
            GameObject prevGameObject = null;
        
            for (int i = 0; i <= length; i++)
            {
                float t = i / r;
                Vector3 point = MathUtil.CalculateQuadraticBezierPoint(t, p0, controlPoint, p2);
                
                GameObject obj  = PrefabPoolManager.Instance.GetObjectFromPool(_arrowPrefab,point, Quaternion.identity,transform);
                //Determine look at rotation
                if (prevGameObject)
                {
                    float angle = MathUtil.LookAt2D(prevGameObject.transform.position, obj.transform.position);
                    prevGameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    prevGameObject = obj;
                }
                else
                    prevGameObject = obj;
            
                if (i == length)
                {
                    float angle = MathUtil.LookAt2D(obj.transform.position, _endPosition);
                    obj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                }
                
            }
        }
        
        public bool IsTouchEntity(out IEntity entity)
        {
            Vector2 worldPoint = GetMouseWorldPosition();
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, _entityLayer);
            if (hit.collider && hit.collider.gameObject.TryGetComponent(out IEntity val))
            {
                entity = val;
                return true;
            }
            entity = null;
            return false;
        }
        
        private void OnPoint(Vector2 input)
        {
            _mousePointer = input;
        }

        private Vector2 GetMouseWorldPosition() => _camera.ScreenToWorldPoint(_mousePointer);

    }
}