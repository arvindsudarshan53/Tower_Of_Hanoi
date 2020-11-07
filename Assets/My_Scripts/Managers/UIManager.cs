using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;


    [Header("Setup Menu")]
    [SerializeField] Slider noOfDiscSlider;
    [SerializeField] TextMeshProUGUI noOfDiscValDisp;
    [SerializeField] GameObject setupMenu;
    [SerializeField] Toggle autoModeToggle;

    [Header("In Game UI")]
    [SerializeField] GameObject inGameUI;
    [SerializeField] TextMeshProUGUI noOfMovesDispText, bestMovesDispText;
    [SerializeField] GameObject undoButton;
    [SerializeField] Transform discIndicator;
    [SerializeField] Animation wrongMoveTextAnim;
    [SerializeField] Animation youWonTextAnim;
    [SerializeField] Animation solvedTextAnim;

    GameManager gameManager;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        UpdateNoOfDisc();
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayNoOfMoves();
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

    public void ShowYouWonText()
    {
        youWonTextAnim.Play();
    }

    public void ShowSolvedText()
    {
        solvedTextAnim.Play();
    }

    public void UpdateNoOfDisc()
    {
        int chosenDiscsNos = (int)noOfDiscSlider.value;
        noOfDiscValDisp.text = chosenDiscsNos.ToString();
        gameManager.chosenNoOfDiscs = chosenDiscsNos;
    }

    public void ShowSetupMenu(bool canShow)
    {
        setupMenu.SetActive(canShow);
    }

    public void ShowInGameUI(bool canShow)
    {
        inGameUI.SetActive(canShow);
    }

    public void ShowUndoButton(bool canShow)
    {
        undoButton.SetActive(canShow);
    }

    public void UpdateAutoModeBool()
    {
        gameManager.isAutoModeOn = autoModeToggle.isOn;
    }

    void DisplayNoOfMoves()
    {
        noOfMovesDispText.text = "Moves : " + gameManager.noOfMoves;
        bestMovesDispText.text = "Best Moves : " + gameManager.bestMovesCount;
    }


}
