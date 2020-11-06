using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] Transform discIndicator;
    [SerializeField] Animation wrongMoveTextAnim;

    GameManager gameManager;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDiscIndicator(bool canShow, string towerName)
    {
        discIndicator.gameObject.SetActive(canShow);
        if (towerName == "Tower_A")
            discIndicator.position = gameManager.towerAContentOrganizer.transform.position;
        else if (towerName == "Tower_B")
            discIndicator.position = gameManager.towerBContentOrganizer.transform.position;
        else
            discIndicator.position = gameManager.towerCContentOrganizer.transform.position;
    }

    public void ShowWrongMoveText()
    {
        wrongMoveTextAnim.Play();
    }
}
