using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PathController : MonoBehaviour
{
    #region serialized fields
    [SerializeField] List<Vector3> linePos;
    [SerializeField] Transform sealPos;
    [SerializeField] float smoothDivision;
    [SerializeField] Transform endPos;
    [SerializeField] Transform sealContainer;
    [SerializeField] float pointTime = 2f;
    [SerializeField] float stoppingDistance = 0.3f;
    #endregion

    #region private fields
    float pointPathTime = 2f;
    LineRenderer lineRend;
    bool canDraw = true;
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

    public void StartPath()
    {
        SetPoint(endPos.position);
        StartCoroutine(Path());
    }

    void SetPoint(InputAction.CallbackContext ctx)
    {
        if (canDraw)
            SetPoint();
    }

    void SetPoint()
    {
        linePos.Add(InputManager.Instance.MousePos);
        SetPoints();
    }

    void SetPoint(Vector2 goal)
    {
        linePos.Add(goal);
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

    public void SetCanDraw(bool con)
    {
        canDraw = con;
    }

    IEnumerator Path()
    {
        for (int i = 0; i < linePos.Count; i++)
        {
            while (Vector3.Distance(sealPos.position, linePos[i]) > stoppingDistance)
            {
                pointPathTime += Time.deltaTime;
                sealContainer.position = Vector3.Lerp(sealContainer.position, linePos[i], pointPathTime / pointTime);
                yield return null;

            }
        }

    }

    public void ResetGame()
    {
        GameStateManager.ReloadGameScene();

    }
}