using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    public ObjectToSpeed[] movedObjects;

    private void FixedUpdate()
    {
        for (int i = 0; i < movedObjects.Length; i++)
        {
            var movedObject = movedObjects[i];
            movedObject.objectTransform.position += new Vector3(movedObject.moveSpeed, 0, 0)
                * Time.fixedDeltaTime * SpeedMultiplier;
        }
    }

    public float SpeedMultiplier { get; set; } = 1;


    [System.Serializable]
    public class ObjectToSpeed
    {
        public Transform objectTransform;
        public float moveSpeed;
    }
}
