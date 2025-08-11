using UnityEngine;

public class CubeGizmoDrawer : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;
    public Vector3 gizmoSize = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, gizmoSize);
    }
}
