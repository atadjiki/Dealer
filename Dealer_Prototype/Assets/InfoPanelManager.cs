using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelManager : MonoBehaviour
{
    private static InfoPanelManager _instance;

    public static InfoPanelManager Instance { get { return _instance; } }

    [SerializeField] private GameObject textMeshPrefab;

    private Dictionary<GameObject, TextMeshProUGUI> targets;

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

        targets = new Dictionary<GameObject, TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        foreach(GameObject target in targets.Keys)
        {
            
        }
    }

    public void RegisterTarget(GameObject target)
    {
        if(targets.ContainsKey(target) == false)
        {
            GameObject textMeshObject = Object.Instantiate<GameObject>(textMeshPrefab, this.transform);
            TextMeshProUGUI textMesh = textMeshObject.GetComponent<TextMeshProUGUI>();
            textMesh.text = target.name;

            targets.Add(target, textMesh);
        }
    }

    public void UnRegisterTarget(GameObject target)
    {

        Destroy(targets[target].gameObject);
            targets.Remove(target);
        
    }
}