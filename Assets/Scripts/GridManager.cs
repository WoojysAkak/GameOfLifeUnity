using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject Cell;
    public Material alive;
    public Material Dead;
    public GameObject CellContainer;
    public GameObject[,] _grid;
    private bool[,] toChange;
    public int hauteur = 5;
    public int largeur = 5;
    public int rule = 0;
    public int offset = 0;
    public bool checkStart = false;
    public bool checkGen = false;
    public float timer = 1.0f;
    private float _timer;
    private bool playLoop = false;
    public static GridManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _timer = timer;
        GenMap();

    }
    public void PlayStart()
    {
        checkStart = !checkStart;
    }

    public void borderRule()
    {
        rule = (int)GameObject.Find("GameType").GetComponent<Slider>().value;
        if (rule == 0)
        {
            offset = 20;
        }
        else
        {
            offset = 0;
        }
    }

    public void ChangeHeight()
    {
        hauteur = (int)GameObject.Find("Slider Hauteur").GetComponent<Slider>().value;
    }
    public void ChangeWidth()
    {
        largeur = (int)GameObject.Find("Slider Largeur").GetComponent<Slider>().value;
    }
    public void ChangeSpeed()
    {
        timer = GameObject.Find("Slider Vitesse").GetComponent<Slider>().value;
    }

    public void posCam(int largeur, int hauteur)
    {
        int cameraSize = 0;
        if (hauteur < largeur)
        {
            cameraSize = largeur;
        }
        else
        {
            cameraSize = hauteur;
        }
        Camera.main.orthographicSize = cameraSize / 2;
        Vector3 position = new Vector3(0 + offset / 2, (hauteur + offset) / 2 - 0.5f, -10);
        Camera.main.transform.position = position;
    }

    public void GenMap()
    {
        posCam(largeur,hauteur);
        checkGen = true;
        foreach (Transform Cell in CellContainer.transform)
        {
            Destroy(Cell.gameObject);
        }
        _grid = new GameObject[hauteur + offset, largeur + offset];
        for (int y = 0; y < hauteur + offset; y++)
        {
            for (int x = 0; x < largeur + offset; x++)
            {
                GameObject clone = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity);
                clone.name = (y.ToString() + "," + x.ToString());
                clone.transform.SetParent(CellContainer.transform);
                if (x < offset / 2 || y < offset / 2 || x >= largeur + offset / 2 || y >= hauteur + offset / 2)
                    clone.SetActive(false);
                _grid[y, x] = clone;
            }
        }
    }

    int checkNeighbour(int x, int y)
    {
        int comptage = 0;
        int newX = 0;
        int newY = 0;
        if (rule == 1)
        {
            // bas droite
            if (x <= largeur - 1)
            {
                newX = x + 1;
                newY = y - 1;
                if (y == 0)
                {
                    newY = hauteur - 1;
                }
                if (x == largeur - 1)
                {
                    newX = 0;
                }
                comptage += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }

            // droite
            if (x < largeur - 1)
                comptage += _grid[y, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                comptage += _grid[y, largeur - x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            //haut droite
            if (x <= largeur - 1)
            {
                newX = x + 1;
                newY = y + 1;
                if (y == hauteur - 1)
                {
                    newY = 0;
                }
                if (x == largeur - 1)
                {
                    newX = 0;
                }
                comptage += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
            
            //bas
            if (y > 0)
                comptage += _grid[y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
            {
                comptage += _grid[hauteur - y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            }

            //haut
            if (y < hauteur - 1)
                comptage += _grid[y + 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                comptage += _grid[hauteur - y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            //bas gauche
            if (x >= 0)
            {
                newX = x - 1;
                newY = y - 1;
                if (y == 0)
                {
                    newY = hauteur - 1;
                }
                if (x == 0)
                {
                    newX = largeur - 1;
                }
                comptage += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
                           

            //gauche
            if (x > 0)
                comptage += _grid[y, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                comptage += _grid[y, largeur - x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            //haut gauche
            if (x >= 0)
            {
                newX = x - 1;
                newY = y + 1;
                if (y == hauteur - 1)
                {
                    newY = 0;
                }
                if (x == 0)
                {
                    newX = largeur - 1;
                }
                comptage += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
        }
        else
        {
            if (y == hauteur + 19 || x == largeur + 19 || x == 0 || y == 0)
            {
                _grid[y , x ].GetComponent<CellAttributs>().alive = false;
            }
            if (y - 1 >= 0 && x + 1 < largeur + 19)
                comptage += _grid[y - 1, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x + 1 < largeur + 19)
                comptage += _grid[y, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x + 1 < largeur + 19 && y + 1 < hauteur + 19)
                comptage += _grid[y + 1, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y - 1 >= 0)
                comptage += _grid[y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y + 1 < hauteur + 19)
                comptage += _grid[y + 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x - 1 >= 0 && y - 1 >= 0)
                comptage += _grid[y - 1, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            
            if (x - 1 >= 0)
                comptage += _grid[y, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y + 1 < hauteur + 19 && x - 1 >= 0)
                comptage += _grid[y + 1, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

        }
        return comptage;
    }
    void PlayLoop()
    {
        toChange = new bool[hauteur + offset, largeur + offset];
        for (int y = 0; y < hauteur + offset; y++)
        {
            for (int x = 0; x < largeur + offset; x++)
            {
                toChange[y, x] = _grid[y, x].GetComponent<CellAttributs>().alive;
                switch (checkNeighbour(x, y))
                {
                    case 0:
                        toChange[y, x] = false;
                        break;
                    case 1:
                        toChange[y, x] = false;
                        break;
                    case 3:
                        toChange[y, x] = true;
                        break;
                    case 4:
                        toChange[y, x] = false;
                        break;
                    case 5:
                        toChange[y, x] = false;
                        break;
                    case 6:
                        toChange[y, x] = false;
                        break;
                    case 7:
                        toChange[y, x] = false;
                        break;
                    case 8:
                        toChange[y, x] = false;
                        break;
                }
            }
        }
        for (int y = 0; y < hauteur + offset; y++)
        {
            for (int x = 0; x < largeur + offset; x++)
            {
                _grid[y, x].GetComponent<CellAttributs>().alive = toChange[y, x];
            }
        }

    }

    void Update()
    {
        Vector3 worldPosition;
        if (checkGen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)Mathf.Floor(worldPosition.x + 0.5f);
                int y = (int)Mathf.Floor(worldPosition.y + 0.5f);
                if (x >= 0 && x < largeur + offset && y >= 0 && y < hauteur + offset)
                {
                    _grid[y, x].GetComponent<CellAttributs>().alive = !_grid[y, x].GetComponent<CellAttributs>().alive;
                }
            }
            if (checkStart)
            {
                playLoop = !playLoop;
            }
            if (playLoop && _timer <= 0)
            {
                PlayLoop();
                _timer = timer;
            }
            _timer -= Time.deltaTime;
            playLoop = false;
        }
    }
}


