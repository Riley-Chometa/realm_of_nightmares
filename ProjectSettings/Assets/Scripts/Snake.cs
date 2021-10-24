using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    public List<Transform> segments= new List<Transform>();
    public Transform segmentPrefab;
   
    public int score = 0;
    public int health = 3;
    public int highestscore=0;
    public Text scoreText;
    public Text healthText;
    public Text highestScoreText;
    private void Start() {
      ResetState();
      PlayerData data = SaveSystem.LoadPlayer();
      this.highestscore = data.highestscore;
      scoreText.text = "Score: "+ score.ToString();
      healthText.text = "Health: "+health.ToString();
      highestScoreText.text = "Highest Score: "+data.highestscore.ToString();

    }
    public void SavePlayer(){
      PlayerData data = SaveSystem.LoadPlayer();
      SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer(){
      
      PlayerData data = SaveSystem.LoadPlayer();
      score = data.score;
      health = data.health;
      highestscore = data.highestscore;
      
      scoreText.text = "Score: "+ score.ToString();
      healthText.text = "Health: "+health.ToString();
      highestScoreText.text = "Highest Score: "+highestscore.ToString();
      Vector3 position;
      position.x = data.position[0];
      position.y = data.position[1];
      position.z = data.position[2];
      transform.position = position;
      for(int i=1;i<segments.Count;i++){
        Destroy(segments[i].gameObject);
      }
      segments.Clear();
      segments.Add(this.transform);
      for(int i = 1;i<data.bodyParts;i++){
        segments.Add(Instantiate(this.segmentPrefab));
      }
      for(int i = segments.Count-1;i>0;i--){
        segments[i].position = segments[i-1].position;
      }
    }
    private void Update() {
      if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){
        direction = Vector2.up;
      }
      else if(Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow)){
        direction = Vector2.down;
      }
      else if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){
        direction = Vector2.left;
      }
      else if(Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.RightArrow)){
        direction = Vector2.right;
      }
    }
    private void FixedUpdate() {
      
      for(int i = segments.Count-1;i>0;i--){
        segments[i].position = segments[i-1].position;
      }
      this.transform.position = new Vector3(
        Mathf.Round(this.transform.position.x + direction.x),
        Mathf.Round(this.transform.position.y + direction.y),
        0.0f
      );
      
    }
    private void Grow(){
      Transform segment = Instantiate(this.segmentPrefab);
      segment.position = segments[segments.Count-1].position;

      segments.Add(segment);

    }

    private void OnTriggerEnter2D(Collider2D other) {
      if(other.tag == "Food"){
        Grow();
        score++;
        scoreText.text = "Score: "+ score.ToString();
        if(score>highestscore){
          highestscore = score;
          highestScoreText.text = "Highest Score: "+highestscore.ToString();
        }
      }
      if(other.tag == "Wall"){
        health--;
        if(this.health>=1){
          healthText.text = "Health: "+health.ToString();
        }else{
          this.health = 3;
          healthText.text = "Health: "+health.ToString();
          score = 0;
          scoreText.text = "Score: "+ score.ToString();
        }
        ResetState();
      }
    }

    private void ResetState(){
      for(int i=1;i<segments.Count;i++){
        Destroy(segments[i].gameObject);
      }
      segments.Clear();
      segments.Add(this.transform);
      for(int i = 1;i<3;i++){
        segments.Add(Instantiate(this.segmentPrefab));
      }
      
      this.transform.position = Vector3.zero;
    }
}
