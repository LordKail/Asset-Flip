using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
        //SceneManager.LoadScene("Board_Scene");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Current scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == "Board_Scene") {
                SceneManager.LoadScene("Board_Scene_2");
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    EnemyCharacterController controller = (EnemyCharacterController) enemy.GetComponent(typeof(EnemyCharacterController));
                    controller.SetLightSprite();
                }
            }
            else
            {
                SceneManager.LoadScene("Board_Scene");
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    EnemyCharacterController controller = (EnemyCharacterController) enemy.GetComponent(typeof(EnemyCharacterController));
                    controller.SetDarkSprite();
                }
            }

        }
    }
}
