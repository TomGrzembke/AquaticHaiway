using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PathController : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields
    LineRenderer lineRend;
    [SerializeField] List<Vector3> linePos;
    #endregion
    void Awake()
    {
        InputManager.Instance.SubscribeTo(SetPoint, InputManager.Instance.leftclickAction, false);
        InputManager.Instance.SubscribeTo(Undo, InputManager.Instance.rightClickAction, false);
        lineRend = GetComponent<LineRenderer>();

    }

    void SetPoint(InputAction.CallbackContext ctx)
    {
        linePos.Add(InputManager.Instance.MousePos);
        SetPoints();
    }

    void Undo(InputAction.CallbackContext ctx)
    {
        if (linePos.Count > 0)
            linePos.RemoveAt(linePos.Count - 1);
        SetPoints();
    }

    void SetPoints()
    {
        lineRend.positionCount = linePos.Count;
        lineRend.SetPositions(linePos.ToArray());
    }
}