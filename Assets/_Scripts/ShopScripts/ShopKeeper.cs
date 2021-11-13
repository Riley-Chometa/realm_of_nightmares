using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject floatingText;

    public GameObject triggerFloor;

    // Add in audio triggers!
    [SerializeField]
    private AudioSource audioPlayer;
    public AudioClip[] audioClips;

    private GameObject temptText;
    private GameObject tempBox;
    private Transform tm;
    void Start()
    {
        tm = this.gameObject.GetComponent<Transform>();
    }

    public void startDialog(int audioIndex){
        audioPlayer.PlayOneShot(audioClips[audioIndex]);
        tempBox = Instantiate(dialogBox, tm.position + new Vector3(1.8f, 0.2f, 0), Quaternion.identity, tm);
        temptText = Instantiate(floatingText, tempBox.GetComponent<Transform>().position, Quaternion.identity, tempBox.transform);
        temptText.GetComponent<Transform>().localScale = new Vector3(1,1,1);
        var tempRenderer = temptText.GetComponent<MeshRenderer>();
        var tempMesh = temptText.GetComponent<TextMesh>();
        tempMesh.text = "This is the store!";   
        tempMesh.color = Color.black; 
        tempRenderer.sortingLayerName = "aboveTable";
        tempRenderer.sortingOrder = 12;
    }

    public void changeText(string newText){
            if (temptText){
                temptText.GetComponent<TextMesh>().text = newText;
            }
    }
    public void stopDialog(){
        Destroy(tempBox, 2);
        Destroy(temptText, 2);
    }

}
