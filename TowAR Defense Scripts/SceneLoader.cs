using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour {

	public GameObject LoadingScreen;
	public Slider slider;
	public Text ProgressText;
    [SerializeField]
    private int scene;
    [SerializeField]
    //private Text loadingText;
    private TextMeshProUGUI loadingText;
	private bool loadScene = false;

	public void LoadLevel (int SceneIndex)
	{
		StartCoroutine(LoadNewScene(SceneIndex));
	}

    // Updates once per frame
    void Update() {

        // If a new scene is not loading yet...
        if (loadScene == false) {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...change the instruction text to read "Loading..."
            loadingText.text = "Loading...";

            // ...and start a coroutine that will load the desired scene.
            LoadLevel(scene);

        }

        // If the new scene has started loading...
        if (loadScene == true) {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }

    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene(int SceneIndex) {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation operation = SceneManager.LoadSceneAsync (SceneIndex);
		LoadingScreen.SetActive(true);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!operation.isDone) 
        {
			float progress = Mathf.Clamp01(operation.progress/.9f);
			slider.value = progress;
			string progressString = string.Format("{0:0.##}", progress*100f);
			ProgressText.text = progressString  + "%";
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
			yield return null;
        }

    }

}