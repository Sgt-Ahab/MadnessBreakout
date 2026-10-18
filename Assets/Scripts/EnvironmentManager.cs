using UnityEngine;

//Manages the random tracks + backgrounds for MainLevel Scene
public class EnvironmentManager : MonoBehaviour
{
    //Gather our variables by separating with Headers, Media is the Arrays of the Images from LevelBGs
    //bgTracks is the array we put in from Music
    [Header("Media")]
    public Sprite[] bgImages;
    public AudioClip[] bgTracks;
    //Pieces of each media
    [Header("Components")]
    public SpriteRenderer bgImage;
    public AudioSource musicPlayer;

    //Indexing for preventing reusage per run
    private int lastIMGindex = -1;
    private int lastWAVindex = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssembleEnvironment();
    }

    public void AssembleEnvironment()
    {
        //This is the randomizing of each array choice and setting Audio + Image
        //Randomizing Image
        //Unified selection variable
        int selection = -1;
        if(bgImages.Length > 1)
        {
            selection = Random.Range(0, bgImages.Length);
            //Re-roll same numbers
            while(selection == lastIMGindex)
            {
                selection = Random.Range(0, bgImages.Length);
            }
            //Stamp the index
            lastIMGindex = selection;
            //Make the sprite the selection
            bgImage.sprite = bgImages[selection];
        }
        //Randomizing Image
        if (bgTracks.Length > 1)
        {
            selection = Random.Range(0, bgTracks.Length);
            //Re-roll same numbers
            while (selection == lastWAVindex)
            {
                selection = Random.Range(0, bgTracks.Length);
            }
            //Stamp the index
            lastWAVindex = selection;
            //Make the song play the selection
            musicPlayer.clip = bgTracks[selection];
            musicPlayer.Play();
        }
    }
}
