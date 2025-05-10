using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

namespace _Project.Scripts.Main.VirtualMouse
{
    [RequireComponent(typeof(VirtualMouseInput))]
    public class VirtualMouseController : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasRectTransform;
        private VirtualMouseInput _virtualMouseInput;
    
        private Camera _mainCamera;
        private GameObject _lastGameObject;
        private int _uiLayerMask;
    
        private void Awake()
        {
            _virtualMouseInput = GetComponent<VirtualMouseInput>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _uiLayerMask = ~LayerMask.GetMask("UI");
        }

        private void Update()
        {
            transform.localScale = Vector3.one *  1f / _canvasRectTransform.localScale.x;
            transform.SetAsLastSibling();
            DetectNoneUI();
        }

        private void DetectNoneUI()
        {
            Ray ray = _mainCamera.ScreenPointToRay(_virtualMouseInput.virtualMouse.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100,_uiLayerMask))
            {
                if (_lastGameObject && _lastGameObject != hit.collider.gameObject)
                {
                    if (_lastGameObject.TryGetComponent(out IOnMouseExitHandler onMouseExitHandler))
                        onMouseExitHandler.OnMouseExitVirtual();
                    _lastGameObject = null;
                }
   
                if (_lastGameObject == null)
                {
                    _lastGameObject = hit.collider.gameObject;
                    if(hit.collider.gameObject.TryGetComponent(out IOnMouseEnterHandler onMouseEnterHandler))
                        onMouseEnterHandler.OnMouseEnterVirtual();
                }
            }
            else
            {
                if (_lastGameObject !=null)
                {
                    if (_lastGameObject.TryGetComponent(out IOnMouseExitHandler onMouseExitHandler))
                        onMouseExitHandler.OnMouseExitVirtual();
                    _lastGameObject = null;
                }
            }
        }

        private void LateUpdate()
        {
            Vector2 virtualMousePosition = _virtualMouseInput.virtualMouse.position.value;
            virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
            virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);
            InputState.Change(_virtualMouseInput.virtualMouse.position, virtualMousePosition);
   
        }
    }
}
