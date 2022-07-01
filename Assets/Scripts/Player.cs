using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using ElephantSDK;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    static string[] PARTSNAME = new string[] { "Wheel", "FrontWheel", "Body", "Engine", "Hood", "Windows" };
    public Vector3 camOffest;
    public Transform pipeBackPoint;
    public GameObject pipeBack;
    public GameObject pipeMid;
    public GameObject lastPart;
    public GameObject otherPartsPoint;
    public GameObject exObject;
    public GameObject exParticle;
    public GameObject midPoint;
    public List<GameObject> wheelPoints;
    public List<GameObject> wheels;
    public Rigidbody[] AllRig;
    public Image[] falseImages;
    public Image carPartImage;
    public Sprite[] carPartsSprite;
    public Sprite falseSprite;
    public bool mouseCheck, backWheelCheck, frontWheelCheck, dead, midWheelCheck, checkDamage, timer;
    public int vector, nextTrack, nextFalseImage;
    public float speed, playerPosX, playerRotY, slerpTime, camSpeed, a;


    [Header("UI")]
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject failPanel;
    public GameObject inGamePanel;

    public Button startButton;
    public Button winButton;
    public Button failButton;

    public bool start, win, fail;
    public int sceneNumb, level;

    void Start()
    {
        sceneNumb = SceneManager.GetActiveScene().buildIndex;
        level = PlayerPrefs.GetInt("Levels");
        startButton.onClick.AddListener(() => StartButton());
        winButton.onClick.AddListener(() => NextButton());
        failButton.onClick.AddListener(() => AgainButton());

        startPanel.SetActive(true);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        inGamePanel.SetActive(false);
        start = false;
        dead = false;
    }

    void Update()
    {
        if (start)
        {
            if (nextFalseImage == 3)
            {
                dead = true;
                startPanel.SetActive(false);
                winPanel.SetActive(false);
                failPanel.SetActive(true);
                inGamePanel.SetActive(false);
            }
            if (nextTrack > 6)
            {
                carPartImage.sprite = carPartsSprite[nextTrack];
            }
            if (!dead)
            {
                PlayerMovement();
            }
        }

        DamageCam();
        Combine();
        FallowCamera();
    }

    private void StartButton()
    {
        start = true;
        inGamePanel.SetActive(true);
        startPanel.SetActive(false);
        Elephant.LevelStarted(level);
    }

    private void NextButton()
    {
        if (sceneNumb == 1)
        {
            SceneManager.LoadScene(2);
        }
        else if (sceneNumb == 2)
        {
            SceneManager.LoadScene(1);
        }
        level++;
        PlayerPrefs.SetInt("Levels", level);
        Elephant.LevelCompleted(level);
    }

    private void AgainButton()
    {
        Elephant.LevelFailed(level);
        SceneManager.LoadScene(sceneNumb);
        PlayerPrefs.SetInt("Levels", level);
    }

    private void PlayerMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseCheck = true;
            playerPosX = 0;
        }

        if (mouseCheck)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                playerPosX = Input.GetTouch(0).deltaPosition.x;
            }
            if (Input.GetMouseButtonUp(0))
            {
                playerPosX = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseCheck = true;
            playerPosX = 0;
        }

        if (mouseCheck)
        {
            if (Input.GetMouseButton(0))
            {
                playerPosX = Input.GetAxis("Mouse X");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                playerPosX = 0;
            }
        }

        if (-1 > playerPosX && 3f > vector)
        {
            vector += 3;
            playerPosX = 0;
            playerRotY = -30;
            mouseCheck = false;
        }
        else if (1 < playerPosX && -3f < vector)
        {
            vector -= 3;
            playerPosX = 0;
            playerRotY = 30;
            mouseCheck = false;
        }

        if (transform.position.x == vector)
        {
            playerRotY = 0;
        }

        if (playerRotY == 0)
        {
            slerpTime = 0.1f;
        }
        else
        {
            slerpTime = 0.1f;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(vector, transform.position.y, transform.position.z), Time.deltaTime * 10);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * speed * -1);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, playerRotY, 0), slerpTime);
    }
    private void Combine()
    {
        if (midWheelCheck)
        {
            pipeBack.SetActive(false);
            pipeMid.SetActive(false);
            wheels[0].transform.position = Vector3.MoveTowards(wheels[0].transform.position, midPoint.transform.position, Time.deltaTime * 15);
            wheels[0].transform.rotation = Quaternion.Slerp(wheels[0].transform.rotation, Quaternion.Euler(midPoint.transform.eulerAngles), slerpTime);
            if (wheels[0].transform.position == midPoint.transform.position && wheels[0].transform.rotation == midPoint.transform.rotation)
            {
                midWheelCheck = false;
            }
        }

        if (backWheelCheck)
        {
            pipeMid.SetActive(false);

            wheels[0].transform.position = Vector3.MoveTowards(wheels[0].transform.position, new Vector3(wheelPoints[0].transform.position.x, wheelPoints[0].transform.position.y, midPoint.transform.position.z), Time.deltaTime * 15);
            wheels[0].transform.rotation = Quaternion.Slerp(wheels[0].transform.rotation, Quaternion.Euler(wheelPoints[0].transform.eulerAngles), slerpTime);

            wheels[1].transform.position = Vector3.MoveTowards(wheels[1].transform.position, new Vector3(wheelPoints[1].transform.position.x, wheelPoints[1].transform.position.y, midPoint.transform.position.z), Time.deltaTime * 15);
            wheels[1].transform.rotation = Quaternion.Slerp(wheels[1].transform.rotation, Quaternion.Euler(wheelPoints[1].transform.eulerAngles), slerpTime);

            if (wheels[1].transform.position == new Vector3(wheelPoints[1].transform.position.x, wheelPoints[1].transform.position.y, midPoint.transform.position.z) && wheels[1].transform.rotation == wheelPoints[1].transform.rotation && wheels[0].transform.position == new Vector3(wheelPoints[0].transform.position.x, wheelPoints[0].transform.position.y, midPoint.transform.position.z) && wheels[0].transform.rotation == wheelPoints[0].transform.rotation)
            {
                pipeBack.SetActive(true);
                pipeBack.transform.position = new Vector3(pipeBackPoint.position.x, pipeBack.transform.position.y, midPoint.transform.position.z);
                backWheelCheck = false;
            }
        }

        if (frontWheelCheck)
        {
            wheels[0].transform.position = Vector3.MoveTowards(wheels[0].transform.position, wheelPoints[0].transform.position, Time.deltaTime * 15);
            wheels[0].transform.rotation = Quaternion.Slerp(wheels[0].transform.rotation, Quaternion.Euler(wheelPoints[0].transform.eulerAngles), slerpTime);

            wheels[1].transform.position = Vector3.MoveTowards(wheels[1].transform.position, wheelPoints[1].transform.position, Time.deltaTime * 15);
            wheels[1].transform.rotation = Quaternion.Slerp(wheels[1].transform.rotation, Quaternion.Euler(wheelPoints[1].transform.eulerAngles), slerpTime);
            pipeBack.transform.position = Vector3.MoveTowards(pipeBack.transform.position, new Vector3(pipeBackPoint.position.x, pipeBack.transform.position.y, pipeBackPoint.position.z), Time.deltaTime * 15);

            wheels[2].transform.position = Vector3.MoveTowards(wheels[2].transform.position, wheelPoints[2].transform.position, Time.deltaTime * 15);
            wheels[2].transform.rotation = Quaternion.Slerp(wheels[2].transform.rotation, Quaternion.Euler(wheelPoints[2].transform.eulerAngles), slerpTime);

            if (wheels[2].transform.position == wheelPoints[2].transform.position && wheels[2].transform.rotation == wheelPoints[2].transform.rotation && wheels[1].transform.position == wheelPoints[1].transform.position && pipeBack.transform.position == new Vector3(pipeBackPoint.position.x, pipeBack.transform.position.y, pipeBackPoint.position.z))
            {
                pipeMid.SetActive(true);
                frontWheelCheck = false;
            }
        }

        if (lastPart != null)
        {
            lastPart.transform.position = Vector3.MoveTowards(lastPart.transform.position, otherPartsPoint.transform.position, Time.deltaTime * 15);
            lastPart.transform.rotation = Quaternion.Slerp(lastPart.transform.rotation, Quaternion.Euler(otherPartsPoint.transform.eulerAngles), slerpTime);
        }
    }

    Vector3 desiredPosition;
    Vector3 smoothedPosition;
    private void FallowCamera()
    {
        desiredPosition = transform.position + camOffest;
        smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, camSpeed);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, smoothedPosition.y, smoothedPosition.z);
    }

    private void DamageCam()
    {
        if (timer)
        {
            a += Time.deltaTime;
        }
        if (a > 0.10f)
        {
            Camera.main.transform.GetComponent<Animator>().SetBool("Damage", false);
            a = 0;
            timer = false;
            Camera.main.transform.position = new Vector3(camOffest.x, camOffest.y, Camera.main.transform.position.z);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            startPanel.SetActive(false);
            winPanel.SetActive(true);
            failPanel.SetActive(false);
            inGamePanel.SetActive(false);
            start = false;
        }

        if (collision.gameObject.tag.ToLower() == PARTSNAME[nextTrack].ToLower())
        {
            if (nextTrack == 0)
            {
                backWheelCheck = true;
                wheels.Add(collision.gameObject);
            }
            else if (nextTrack == 1)
            {
                frontWheelCheck = true;
                wheels.Add(collision.gameObject);
            }
            else
            {
                lastPart = collision.gameObject;
            }
            collision.transform.parent = transform;
            nextTrack++;
        }
        else
        {
            for (int i = 0; i < PARTSNAME.Length; i++)
            {
                if (collision.gameObject.tag.ToLower() == PARTSNAME[i].ToLower() && nextTrack < i)
                {
                    falseImages[nextFalseImage].sprite = falseSprite;
                    falseImages[nextFalseImage].GetComponent<Animator>().SetBool("Next", true);
                    nextFalseImage++;
                }
            }
        }

        if (collision.gameObject.CompareTag("Barrel"))
        {
            transform.GetComponent<Collider>().enabled = false;
            Destroy(collision.gameObject);
            dead = true;
            AllRig = GetComponentsInChildren<Rigidbody>(true);
            for (int i = 0; i < AllRig.Length; i++)
            {
                AllRig[i].isKinematic = false;
            }
            Instantiate(exObject, transform.position, transform.rotation);
            Instantiate(exParticle, transform.position, transform.rotation);
            startPanel.SetActive(false);
            winPanel.SetActive(false);
            failPanel.SetActive(true);
            inGamePanel.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Hammer"))
        {
            if (nextTrack != 0)
            {
                nextTrack -= 1;
            }

            foreach (Transform child in transform)
            {
                if (child.tag.ToLower() == PARTSNAME[nextTrack].ToLower())
                {
                    if (child.tag == "Wheel")
                    {
                        midWheelCheck = true;
                        wheels.Remove(child.gameObject);
                    }
                    if (child.tag == "FrontWheel")
                    {
                        backWheelCheck = true;
                        wheels.Remove(child.gameObject);
                    }

                    child.tag = "Untagged";
                    child.parent = null;
                    child.GetComponent<Rigidbody>().isKinematic = false;
                    Instantiate(exObject, transform.position, transform.rotation);
                }
            }
            falseImages[nextFalseImage].GetComponent<Animator>().SetBool("Next", true);
            falseImages[nextFalseImage].sprite = falseSprite;
            nextFalseImage++;
        }
    }
}
