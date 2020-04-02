using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CameraStruct
{
    public CameraScript camera;
    public RenderTexture cameraView;
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] CameraStruct[] cameraStruct;
    [SerializeField] GameObject Spy;
    Collider spyCol;
   
    [SerializeField] float shutDownTimer = 10;
    [SerializeField] RawImage cameraRenderer;
    [SerializeField] int tempImageChanger = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        spyCol = Spy.GetComponent<Collider>();
        shutDownCamera(cameraStruct[0].camera);

    }

    // Update is called once per frame
    void Update()
    {

        //example to see if the spy is detected
        // Debug.Log(isSpySeenByAnyCamera(cameras));
        Debug.Log(isAgentSeenByThisCamera(cameraStruct[0].camera));

        //example how to see all the cameras that know where the spy is located
        /* List<GameObject> camerasThatSeeTheSpyGO = camerasThatSeeTheSpy(cameras);
        for(int i = 0; i < camerasThatSeeTheSpyGO.Count; i++)
        {
            Debug.Log(camerasThatSeeTheSpyGO[i].name + " detect the spy");
        }*/

        //cameraRenderer.texture = getCameraView(cameras, 0);
        //cameraRenderer.texture = getCameraView(cameras, tempImageChanger);
        
        
    
    }

    RenderTexture updateHackerCameraView(int index)
    {
        return cameraStruct[index].cameraView;
        // cameraRenderer.texture = cameraStruct[index].cameraView;
    }
    bool isAgentSeenByThisCamera(CameraScript[] TracingCameras, int index)
    {
        return TracingCameras[index].isObjectVisible(Spy, spyCol);
    }
    bool isAgentSeenByThisCamera(CameraScript TracingCamera)
    {
        return TracingCamera.isObjectVisible(Spy, spyCol);
    }
    bool isAgentSeenByAnyCamera(CameraScript[] TracingCameras)
    {
        for (int i = 0; i < TracingCameras.Length; i++)
        {
            if (TracingCameras[i].isObjectVisible(Spy, spyCol))
                return true;
        }
        return false;
    }
    RenderTexture getCameraView(CameraScript[] TracingCameras, int index)
    {
        /*   Camera camera = cameras[index].gameObject.GetComponent<Camera>();

           // Make a new texture and read the active Render Texture into it.
           Texture2D image = new Texture2D(camera.pixelWidth, camera.pixelHeight);
           image.ReadPixels(new Rect(0, 0, camera.pixelWidth, camera.pixelWidth), 0, 0);
           image.Apply();

           //  Sprite sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);

           if (TracingCameras[index].cameraActive)
           {
               return image;
           }
           Debug.LogWarning("camera is not active");
           return null;

       //    return cameras[index].gameObject.GetComponent<Camera>().activeTexture;
       */
       return TracingCameras[index].gameObject.GetComponent<Camera>().targetTexture;
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
