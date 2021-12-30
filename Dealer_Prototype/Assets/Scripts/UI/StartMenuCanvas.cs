using UnityEngine.InputSystem;
using UnityEngine;

public class StartMenuCanvas : MonoBehaviour
{
    [SerializeField] private BoxCollider Collider_Start;

    private PlayerInputActions inputActions;

    private Vector2 _screenMousePos;

    private int LayerMask_UI;

    private static StartMenuCanvas _instance;

    public static StartMenuCanvas Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        LayerMask_UI = 1<<LayerMask.NameToLayer("UI");

        inputActions = new PlayerInputActions();

        inputActions.Default.Select.performed += ctx => OnSelect(ctx);

        inputActions.Enable();
    }

    private void FixedUpdate()
    {
        _screenMousePos = inputActions.Default.Aim.ReadValue<Vector2>();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask_UI))
        {
            BoxCollider hitMesh = hit.collider.GetComponent<BoxCollider>();

            if(hitMesh != null && hitMesh == Collider_Start)
            {
                LevelManager.Instance.LoadLevel(LevelManager.LevelName.Apartment);
            }

        }
    }
}
