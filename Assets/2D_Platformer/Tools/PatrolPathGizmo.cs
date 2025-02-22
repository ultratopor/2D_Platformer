using UnityEngine;
using UnityEditor;
using Mechanics;

[CustomEditor(typeof(PatrolPath))]
public class PatrolPathGizmo : Editor
{
    public void OnSceneGUI()
    {
        var path = target as PatrolPath;
        using (var cc = new EditorGUI.ChangeCheckScope())
        {
            Vector3 sp = path.transform.InverseTransformPoint(Handles.PositionHandle(path.transform.TransformPoint(path.StartPosition), path.transform.rotation));
            Vector3 ep = path.transform.InverseTransformPoint(Handles.PositionHandle(path.transform.TransformPoint(path.EndPosition), path.transform.rotation));
            if (cc.changed)
            {
                sp.y = 0;
                ep.y = 0;
                path.StartPosition = sp;
                path.EndPosition = ep;
            }
        }
        Handles.Label(path.transform.position, (path.StartPosition - path.EndPosition).magnitude.ToString());
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void OnDrawGizmo(PatrolPath path, GizmoType gizmoType)
    {
        var start = path.transform.TransformPoint(path.StartPosition);
        var end = path.transform.TransformPoint(path.EndPosition);
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(start, end, 5);
        Handles.DrawSolidDisc(start, path.transform.forward, 0.1f);
        Handles.DrawSolidDisc(end, path.transform.forward, 0.1f);
    }
}
