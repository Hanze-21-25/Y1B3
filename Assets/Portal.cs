using UnityEngine;

public class Portal : MonoBehaviour
{
    /*[SerializeField] private PortalArea area;
    [SerializeField] private Rigidbody host;
    [SerializeField] private Camera camera;
    [SerializeField] private Portal linkedPortal;

    private Player subject;

    void Start()
    {
        subject = null;
    }

    private void Update()
    {
        MoveCamera();
    }

    void OnTriggerEnter(Collider other)
    {
        subject = other.gameObject.GetComponent<Player>();

        if (subject != null)
        {
            //subject.transform.position = linkedPortal.transform.position;
            //Debug.Log("Teleported player to level");
            //subject.Rotate(180f);
        }
    }

    private void MoveCamera()
    {
        var toPortal = - GetRelativeVector3(this, area.GetPlayer()); // vector from player to portal;
        var cp = camera.transform.position;
        cp = new Vector3(cp.x * -1, cp.y, cp.z);
        var playerRotation = area.GetPlayer().transform.rotation;
        camera.transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x,playerRotation.y -90, playerRotation.z));
    }

    private Vector3 GetRelativeVector3 (Portal relativeTo, Player player)
    {
        return player.transform.position - relativeTo.transform.position;
    }

    private float GetAngleWithPLayer(out Player player)
    {
        
    }*/
}
