using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HopeDisplay : MonoBehaviour
{
    /// <summary>
    /// HopeDisplay singleton instance. Use this to reference HopeDisplay functions.
    /// </summary>
    public static HopeDisplay Instance;

    Image displayedImage;

    [Header("Icon")]
    public Sprite EmptyHopeIcon;
    public Sprite OneHopeIcon;
    public Sprite TwoHopeIcon;
    public Sprite FullHopeIcon;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("More than one instance of HopeDisplay present");
        Instance = this;
    }

    /// <summary>
    /// Sets the Hope icon to show full Hope.
    /// </summary>
    public void SetHopeFull()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = FullHopeIcon;
    }

    /// <summary>
    /// Sets the Hope icon to show two Hope.
    /// </summary>
    public void SetHopeTwo()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = TwoHopeIcon;
    }

    /// <summary>
    /// Sets the Hope icon to show one Hope.
    /// </summary>
    public void SetHopeOne()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = OneHopeIcon;
    }

    /// <summary>
    /// Sets the Hope icon to show no Hope.
    /// </summary>
    public void SetHopeEmpty()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = EmptyHopeIcon;
    }
}
