using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SlotHolder : MonoBehaviour, IDropHandler
{
    public int id;
    public int score;
    public Button checkButton;
    private bool allHoldersFilled = false;
    public bool filled = false;
    public Text EasyScoreText;

    [SerializeField] private GameObject monthpanel1, monthpanel2, gameoverpanel, EasyConfetti;
    //[SerializeField] private GameObject wrongAnswer, correctAnswer;
    [SerializeField] public GameObject star0, star1, star2, star3;

    public GameObject GameOverPanel { get { return gameoverpanel; } }

    public GameObject MonthPanel1 { get { return monthpanel1; } }
    
    public GameObject MonthPanel2 { get { return monthpanel2; } }

    //public GameObject WeekPanel { get { return weekpanel; } }

    public void OnDrop(PointerEventData eventData)
    {
       
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<DragAndDrop>().id == id)
            {
                Debug.Log("Correct");
                eventData.pointerDrag.GetComponent<DragAndDrop>().SetScore(score);
            }
            else
            {
                Debug.Log("Wrong");
            }
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            filled = true;
        }
        checkAnswer();

    }
    void Start()
    {
        checkButton.interactable = false;
    }


    private int maxPanels = 2; // total number of panels
    private int currentPanel = 0;
    public void checkAnswer()
    {
        DragAndDrop[] dragObjects = FindObjectsOfType<DragAndDrop>();
        SlotHolder[] slotHolders = FindObjectsOfType<SlotHolder>();
        allHoldersFilled = true;
        bool emptySlotExists = false;

        foreach (SlotHolder slotHolder in slotHolders)
        {
            if (!slotHolder.filled)
            {
                allHoldersFilled = false;
                emptySlotExists = true;
                break;
            }
        }

        if (allHoldersFilled)
        {
            foreach (DragAndDrop dragObject in dragObjects)
            {
                RectTransform dragObjectRect = dragObject.GetComponent<RectTransform>();
                bool correctPlacement = false;
                foreach (SlotHolder slotHolder in slotHolders)
                {
                    if (dragObjectRect.anchoredPosition == slotHolder.GetComponent<RectTransform>().anchoredPosition)
                    {
                        if (dragObject.id == slotHolder.id)
                        {
                            //dragObject.GetComponent<Image>().color = Color.green;
                            score++;
                            EasyScoreText.text = score + "/12";
                            correctPlacement = true;
                        }
                        else
                        {
                            //dragObject.GetComponent<Image>().color = Color.red;
                        }
                    }
                }

                if (!correctPlacement)
                {
                    //dragObject.GetComponent<Image>().color = Color.white;
                } 
            }

            int currentPanel = 0; // variable to keep track of current panel
            int maxPanels = 2; // total number of panels

            // Wait until checkButton is clicked before changing panels
            checkButton.onClick.AddListener(delegate {
                if (currentPanel == 0)
                {
                    Invoke("ActivateMonthPanel2", 1f);
                }
                else if (currentPanel == 1)
                {
                    Invoke("EasyActivateGameOverPanel", 1f);
                }

                currentPanel++;

                // Check if current panel is greater than max panels
                if (currentPanel > maxPanels)
                {
                    currentPanel = maxPanels; // Set current panel to max panels
                }
            });

            checkButton.interactable = true;
        }
        else
        {
            checkButton.interactable = false;
            if (emptySlotExists) Debug.Log("Not all holders are filled");
        }
    }

    
    void ActivateMonthPanel2(){   
        MonthPanel2.gameObject.SetActive(true);
        MonthPanel1.gameObject.SetActive(false);

    }
    void ActivateMonthPanel1()
    {
        MonthPanel2.gameObject.SetActive(false);
        MonthPanel1.gameObject.SetActive(true);
    }

        void EasyActivateGameOverPanel(){
        GameOverPanel.gameObject.SetActive(true);
        EasyConfetti.gameObject.SetActive(true);
        MonthPanel2.gameObject.SetActive(false);
        MonthPanel1.gameObject.SetActive(false);

    }
}


