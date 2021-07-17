using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxLife;
    [SerializeField] private int currentLife;

    public Color[] colors;
    public GameObject colorObject;

    public float spawnDelay;
    public LayerMask whatIsColor;

    public Text lifeText;

    private Color currentBackGroundColor;
    private ColorObject[] colorObjects;

    private void Start()
    {
        currentLife = maxLife;
        lifeText.text = string.Format($"Life : {currentLife}");
        SpawnRandomColors();
        StartCoroutine(SetColorsOnRandomTransform());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickScreen();
        }
    }

    private void OnClickScreen()
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(wp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.transform.GetComponent<ColorObject>().color == currentBackGroundColor)
            {
                Debug.Log("맞춤");
            }
            else
            {
                currentLife--;
                //틀림
            }
        }
        else
        {
            currentLife--;
            //틀림
        }

        spawnDelay -= 0.05f; //스폰딜레이 줄이기 난이도 조절
        if (spawnDelay < 0.1f)
        {
            spawnDelay -= 0.01f;
            if (spawnDelay < 0.05f)
                spawnDelay = 0.05f;
        }

        ResetGame();
        StartCoroutine(SetColorsOnRandomTransform());
    }

    private void SpawnRandomColors() //처음한번만 실행합니다.
    {
        colorObjects = new ColorObject[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            ColorObject colorOb = Instantiate(colorObject, Vector3.zero, Quaternion.identity).GetComponent<ColorObject>();
            colorOb.SetColor(colors[i]);
            colorObjects[i] = colorOb;
        }
    }

    private IEnumerator SetColorsOnRandomTransform()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            float randX = Random.Range(-2f, 2f);
            float randY = Random.Range(-4f, 4f);
            Vector2 spawnPos = new Vector2(randX, randY);
            colorObjects[i].SetPosition(spawnPos);
            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log("스폰 끝");
        PickRandomColorAndChangeBackGroundColor();
    }

    private void PickRandomColorAndChangeBackGroundColor()
    {
        int randomIdx = Random.Range(0, 7);
        currentBackGroundColor = colors[randomIdx];

        Camera.main.backgroundColor = currentBackGroundColor;
    }

    private void ResetGame()
    {
        StopAllCoroutines();
        Camera.main.backgroundColor = Color.white;

        lifeText.text = string.Format($"Life : {currentLife}");

        for (int i = 0; i < colors.Length; i++)
        {
            colorObjects[i].gameObject.SetActive(false);
        }
    }
}
