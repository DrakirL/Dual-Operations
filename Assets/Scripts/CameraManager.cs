using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject Spy;
    Collider spyCol;
    [SerializeField] CameraScript[] cameras;
    [SerializeField] float shutDownTimer = 10;


    //test
    [SerializeField] Camera tcamera; 
    [SerializeField] SpriteRenderer observer;
    // Start is called before the first frame update
    void Start()
    {
        spyCol = Spy.GetComponent<Collider>();
        shutDownCamera(cameras[0]);
    }

    // Update is called once per frame
    void Update()
    {

        //example to see if the spy is detected
        // Debug.Log(isSpySeenByAnyCamera(cameras));
        Debug.Log(isSpySeenByThisCamera(cameras[0]));
        
        //example how to see all the cameras that know where the spy is located
        /* List<GameObject> camerasThatSeeTheSpyGO = camerasThatSeeTheSpy(cameras);
        for(int i = 0; i < camerasThatSeeTheSpyGO.Count; i++)
        {
            Debug.Log(camerasThatSeeTheSpyGO[i].name + " detect the spy");
        }*/

        //  observer.sprite = getCameraView(cameras, 1);
    }


    bool isSpySeenByThisCamera(CameraScript[] TracingCameras, int index)
    {
        return TracingCameras[index].isObjectVisible(Spy, spyCol);
    }
    bool isSpySeenByThisCamera(CameraScript TracingCamera)
    {
        return TracingCamera.isObjectVisible(Spy, spyCol);
    }
    bool isSpySeenByAnyCamera(CameraScript[] TracingCameras)
    {
        for (int i = 0; i < TracingCameras.Length; i++)
        {
            if (TracingCameras[i].isObjectVisible(Spy, spyCol))
                return true;
        }
        return false;
    }
    Sprite getCameraView(CameraScript[] TracingCameras, int index)
    {
        tcamera = cameras[index].gameObject.GetComponent<Camera>();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(tcamera.targetTexture.width, tcamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, tcamera.targetTexture.width, tcamera.targetTexture.height), 0, 0);
        image.Apply();

        Sprite sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
        if (TracingCameras[index].cameraActive)
        {
            return sprite;
        }
        Debug.LogWarning("camera is not active");
        return null;
        
    //    return cameras[index].gameObject.GetComponent<Camera>().activeTexture;
    }
    List<GameObject> camerasThatSeeTheSpy(CameraScript[] TracingCameras)
    {
        List<GameObject> tempList = new List<GameObject>(); 
        for (int i = 0; i < TracingCameras.Length; i++)
        {
            if (TracingCameras[i].isObjectVisible(Spy, spyCol))
                tempList.Add(TracingCameras[i].gameObject);
        }
        return tempList;
    }






    public void shutDownCamera(CameraScript camera)
    {
        if (camera.cameraActive)
        {
            camera.cameraActive = false;
            StartCoroutine(acivateCamera(camera, shutDownTimer));
        }
    }

    private IEnumerator acivateCamera(CameraScript camera, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        camera.cameraActive = true;
    }
}
