using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PSlotHolder : MonoBehaviour, IDropHandler
{
    public int id;
    public bool filled = false;
    public int Pscore;
    public Button PcheckButton;
    public Button PresetButton;
    private bool allHoldersFilled = false;
    public Text EasyScoreText;
    public Vector2 initialPosition;
    public AudioSource PracticeCongrats, soundkeepitup, soundwelldone, soundawesome;


    [SerializeField] private GameObject pgameoverpanel, PeasyConfetti;
    [SerializeField] public GameObject star0, star1, star2, star3, keepitup, welldone, awesome;


    public GameObject PGameOverPanel { get { return pgameoverpanel; } }

    public Text TextScore { get { return EasyScoreText; } }
    

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<PDragAndDrop>().id == id)
            {
                Debug.Log("Correct");
                // eventData.pointerDrag.GetComponent<PDragAndDrop>().PSetScore(Pscore);
            }
            else
            {
                Debug.Log("Wrong");
            }
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            filled = true;
        }

        // Check if all holders are filled
        PSlotHolder[] slotHolders = FindObjectsOfType<PSlotHolder>();
        bool allHoldersFilled = true;
        foreach (PSlotHolder slotHolder in slotHolders)
        {
            if (!slotHolder.filled)
            {
                allHoldersFilled = false;
                Pscore = 0;
                break;
            }
        }

        if (allHoldersFilled)
        {
            checkAnswer();
        }
    }
    
    void Start()
    {
        PcheckButton.interactable = false;
        PresetButton.onClick.AddListener(ResetSlots); // Add listener to reset butto

    }
    public void ResetSlots()
    {
        PcheckButton.interactable = false; // Disable check button
        PDragAndDrop[] dragObjects = FindObjectsOfType<PDragAndDrop>();
        PSlotHolder[] slotHolders = FindObjectsOfType<PSlotHolder>();

        foreach (PDragAndDrop dragObject in dragObjects)
        {
            dragObject.ResetObjects(); // Reset position of drag objects
        }
        Pscore = 0; // Reset score
        EasyScoreText.text = Pscore + "/7"; // Update score text
        star0.SetActive(false); // Deactivate all stars

        // Set all slot holders to not filled
        foreach (PSlotHolder slotHolder in slotHolders)
        {
            slotHolder.filled = false;
            Pscore = 0; // Reset score
            EasyScoreText.text = Pscore + "/7"; // Update score text
        }
    }


    public void checkAnswer()
    {
        PDragAndDrop[] dragObjects = FindObjectsOfType<PDragAndDrop>();
        PSlotHolder[] slotHolders = FindObjectsOfType<PSlotHolder>();
        allHoldersFilled = true;
        bool emptySlotExists = false;

        foreach (PSlotHolder slotHolder in slotHolders)
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
            PcheckButton.onClick.AddListener(delegate
            {
                int score = 0;
                foreach (PDragAndDrop dragObject in dragObjects)
                {
                    RectTransform dragObjectRect = dragObject.GetComponent<RectTransform>();
                    foreach (PSlotHolder slotHolder in slotHolders)
                    {
                        if (dragObjectRect.anchoredPosition == slotHolder.GetComponent<RectTransform>().anchoredPosition)
                        {
                            if (dragObject.id == slotHolder.id)
                            {
                                dragObject.GetComponent<Image>().color = Color.green;
                                score++;
                            }
                            else
                            {
                                dragObject.GetComponent<Image>().color = Color.red;
                            }
                        }
                    }
                }

                // Update score and stars
                if (score == 7)
                {
                    star3.gameObject.SetActive(true);
                    Invoke("activateAwesome", 0.5f);
                    awesome.gameObject.SetActive(true);
                    welldone.gameObject.SetActive(false);
                    keepitup.gameObject.SetActive(false);
                    Invoke("ActivateGameOverPanel", 1f);
                }
                else if (score >= 5 && score <= 6)
                {
                    star2.gameObject.SetActive(true);
                    Invoke("activateWelldone", 0.5f);
                    awesome.gameObject.SetActive(false);
                    welldone.gameObject.SetActive(true);
                    keepitup.gameObject.SetActive(false);
                    Invoke("ActivateGameOverPanel", 1f);
                }
                else if (score <= 4 && score != 0)
                {
                    awesome.gameObject.SetActive(false);
                    Invoke("activateKeepitUp", 0.5f);
                    welldone.gameObject.SetActive(false);
                    keepitup.gameObject.SetActive(true);
                    star1.gameObject.SetActive(true);
                    Invoke("ActivateGameOverPanel", 1f);
                }
                else if (score == 0)
                {
                    awesome.gameObject.SetActive(false);
                    Invoke("activateKeepitUp", 0.5f);
                    welldone.gameObject.SetActive(false);
                    keepitup.gameObject.SetActive(true);
                    star0.gameObject.SetActive(true);
                    Invoke("ActivateGameOverPanel", 1f);
                }

                // Update score text and reset check button
                Pscore = score;
                TextScore.text = Pscore + "/7";
                PcheckButton.onClick.RemoveAllListeners();
                PcheckButton.interactable = false;
            });

            PcheckButton.interactable = true;
        }
        else
        {
            PcheckButton.interactable = false;
            if (emptySlotExists) Debug.Log("Not all holders are filled");
        }
    }



    void ActivateGameOverPanel()
    {
        PGameOverPanel.gameObject.SetActive(true);
        PracticeCongrats.Play();
        PeasyConfetti.gameObject.SetActive(true);
    }

    void activateAwesome()
    {
        soundawesome.Play();

    }

    void activateWelldone()
    {
        soundwelldone.Play();

    }

    void activateKeepitUp()
    {
        soundkeepitup.Play();
    }


}



