using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentumDisplay : MonoBehaviour
{
    /// <summary>
    /// MomentumDisplay singleton instance. Use this to reference MomentumDisplay functions.
    /// </summary>
    public static MomentumDisplay Instance;

    Image displayedImage;
    
    [Header("Icon")]
    public Sprite EmptyMomentumIcon;
    public Sprite OneMomentumIcon;
    public Sprite TwoMomentumIcon;
    public Sprite FullMomentumIcon;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("More than one instance of MomentumDisplay present");
        Instance = this;
    }

    /// <summary>
    /// Sets the momentum icon to show full momentum.
    /// </summary>
    public void SetMomentumFull()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = FullMomentumIcon;
    }

    /// <summary>
    /// Sets the momentum icon to show two momentum.
    /// </summary>
    public void SetMomentumTwo()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = TwoMomentumIcon;
    }

    /// <summary>
    /// Sets the momentum icon to show one momentum.
    /// </summary>
    public void SetMomentumOne()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = OneMomentumIcon;
    }

    /// <summary>
    /// Sets the momentum icon to show no momentum.
    /// </summary>
    public void SetMomentumEmpty()
    {
        displayedImage = GetComponent<Image>();
        displayedImage.sprite = EmptyMomentumIcon;
    }
}
