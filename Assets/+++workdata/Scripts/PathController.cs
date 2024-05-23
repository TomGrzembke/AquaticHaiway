using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PathController : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<Vector3> linePos;
    [SerializeField] Transform sealPos;
    [SerializeField] float smoothDivision;
    #endregion

    #region private fields
    LineRenderer lineRend;
    #endregion

    void Awake()
    {
        InputManager.Instance.SubscribeTo(SetPoint, InputManager.Instance.leftclickAction, false);
        InputManager.Instance.SubscribeTo(Undo, InputManager.Instance.rightClickAction, false);
        lineRend = GetComponent<LineRenderer>();

    }

    void Start()
    {
        linePos.Add(sealPos.position);
        SetPoints();
    }

    void SetPoint(InputAction.CallbackContext ctx)
    {
        SetPoint();
    }

    void SetPoint()
    {
        linePos.Add(InputManager.Instance.MousePos);
        SetPoints();
    }

    void Undo()
    {
        for (int i = 1; i < linePos.Count; i++)
        {
            linePos.RemoveAt(i);
            i--;
        }

        SetPoints();
    }

    void Undo(InputAction.CallbackContext ctx)
    {
        Undo();
    }

    void SetPoints()
    {
        lineRend.positionCount = linePos.Count;
        linePos[0] = sealPos.position;
        lineRend.SetPositions(linePos.ToArray());
    }
}