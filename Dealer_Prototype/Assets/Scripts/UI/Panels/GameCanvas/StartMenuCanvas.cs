using UnityEngine;

public class StartMenuCanvas : MonoBehaviour
{
    [SerializeField] private BoxCollider Collider_Start;

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
    }

    private void FixedUpdate()
    {
  //      _screenMousePos = inputActions.Default.Aim.ReadValue<Vector2>();
    }

    private void OnSelect()
    {
        var ray = Camera.main.ScreenPointToRay(_screenMousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask_UI))
        {
            BoxCollider hitMesh = hit.collider.GetComponent<BoxCollider>();

            if(hitMesh != null && hitMesh == Collider_Start)
            {
           //     LevelManager.Instance.LoadLevel(Constants.LevelData.GetLevelData(Constants.LevelDataConstants.LevelName.Apartment));
            }

        }
    }
}
