using UnityEngine;

public class PowerUp : MonoBehaviour
{
   //Enumerate the types, and have the prefabs attached, paddlecontroller calls on the types with what they do
    public enum PowerUpType {ExtraLife, SmallPaddle, ThreeBalls};
    public PowerUpType type;
    private Transform bottomBarrier;
    //Gather the transform of bottomBarrier for garbage collection
    private void Start()
    {
        bottomBarrier = GameObject.FindGameObjectWithTag("End").transform;
    }
    private void Update()
    {
        //Check if this object's Y position, is below (negative is less than)
        if(this.gameObject.transform.position.y < bottomBarrier.transform.position.y)
        {
            //The player has missed the PowerUp, and we destroy for memory collection
            Destroy(this.gameObject);
        }
    }
}
