using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public HealthBar progresBar;
    public GameObject loader;
    
    public void loadScene(string s)
    {
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
        loader.SetActive(true);
        //SceneManager.LoadScene(s);
      StartCoroutine(LoadAsync(s));
    }

    IEnumerator LoadAsync(string scene)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(scene);

        while (!oper.isDone)
        {
            float prog = oper.progress / 0.9f;
            progresBar.setCurrent(prog);
            Debug.Log(prog);
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        progresBar.setMax(1);
        progresBar.setCurrent(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
