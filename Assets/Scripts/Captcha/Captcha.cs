using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class Captcha : MonoBehaviour
{
    [Header("Size settings")]
    [SerializeField] int _minSize = 3;
    [SerializeField] int _maxSize = 5;

    [Header("Events duration")]
    [SerializeField] float _buttonFadeDuration = 4.0f;
    [SerializeField] float _popupDuration = 3.0f;

    [Header("Required UI elements")]
    [SerializeField] TMP_Text _helpTextToClick;
    [SerializeField] GameObject _successImageObject;
    [SerializeField] GameObject _failureImageObject;
    [SerializeField] GameObject _phoneWelcomePanelObject;

    [Header("Animals settings")]
    [SerializeField] string[] _animalsNames;
    [SerializeField] Sprite[] _animalsImages;

    [Header("Background sprites settings")]
    [SerializeField] Sprite _backgroundSprite;
    [SerializeField] Color[] _backgroundColors;
    public GameObject[] outputReponse;
    public AudioSource src;


    private RectTransform _rectTransform;
    private GridLayoutGroup _layoutGroup;

    private string _animalToClickName;
    private int _animalToClickCount;

    private int _nbGoodAnswers = 0;

    private List<GameObject> _allButtons;
    private List<Coroutine> _allButtonsFadeCoroutines;

    public void HideWelcomePanel()
    {
        _phoneWelcomePanelObject.SetActive(false);
    }

    private void Awake()
    {
        _allButtons = new List<GameObject>();
        _allButtonsFadeCoroutines = new List<Coroutine>();

        _rectTransform = GetComponent<RectTransform>();
        _layoutGroup = GetComponent<GridLayoutGroup>();

        GenerateCaptcha();
    }

    private void GenerateCaptcha()
    {
        int captchaSize = Random.Range(_minSize, _maxSize + 1);
        _animalToClickName = _animalsNames[Random.Range(0, _animalsImages.Length)];
        _helpTextToClick.text = _animalToClickName;

        float imagesWidth = _rectTransform.rect.height / captchaSize;
        _layoutGroup.cellSize = new Vector2(imagesWidth, imagesWidth);
        _layoutGroup.constraintCount = captchaSize;

        for (int i = 0; i < captchaSize * captchaSize; i++)
        {
            int animalImageToGenerateIndex = Random.Range(0, _animalsImages.Length);

            // Create Button
            GameObject buttonObject = new GameObject("CaptchaButton" + i);
            buttonObject.transform.parent = transform;

            Button button = buttonObject.AddComponent<Button>();
            button.onClick.AddListener(() => { ClickImage(buttonObject); });

            RectTransform buttonRectTransform = buttonObject.AddComponent<RectTransform>();
            buttonRectTransform.localScale = new Vector3(1, 1, 1);
            buttonRectTransform.localRotation = Quaternion.identity;
            buttonRectTransform.localPosition = new Vector3(buttonRectTransform.localPosition.x, buttonRectTransform.localPosition.y, 0);

            Image buttonBackground = buttonObject.AddComponent<Image>();
            buttonBackground.sprite = _backgroundSprite;
            buttonBackground.color = GetRandomBackgroundColor();

            CaptchaImage captchaImage = buttonObject.AddComponent<CaptchaImage>();
            captchaImage.ImageName = _animalsNames[animalImageToGenerateIndex];

            // Update goal
            if (_animalsNames[animalImageToGenerateIndex] == _animalToClickName)
            {
                _animalToClickCount++;
            }

            // Create Image
            GameObject buttonImageObject = new GameObject("CaptchaImage" + i);
            buttonImageObject.transform.parent = buttonObject.transform;

            RectTransform imageRectTransform = buttonImageObject.AddComponent<RectTransform>();
            imageRectTransform.localScale = new Vector3(1, 1, 1);
            imageRectTransform.offsetMin = new Vector2(0, 0);
            imageRectTransform.offsetMax = new Vector2(0, 0);
            imageRectTransform.pivot = new Vector2(0.5f, 0.5f);
            imageRectTransform.sizeDelta = _layoutGroup.cellSize;
            imageRectTransform.localRotation = Quaternion.identity;
            imageRectTransform.localPosition = new Vector3(imageRectTransform.localPosition.x, imageRectTransform.localPosition.y, 0);

            Image image = buttonImageObject.AddComponent<Image>();
            image.sprite = _animalsImages[animalImageToGenerateIndex];

            _allButtons.Add(buttonObject);
        }
    }

    private void CleanupCaptcha()
    {
        foreach (Coroutine coroutine in _allButtonsFadeCoroutines)
        {
            StopCoroutine(coroutine);
        }

        foreach (GameObject buttonToDestroy in _allButtons)
        {
            buttonToDestroy.GetComponent<Button>().StopAllCoroutines();
            buttonToDestroy.GetComponent<Button>().gameObject.SetActive(false);
            Destroy(buttonToDestroy);
        }

        _allButtons = new List<GameObject>();
        _allButtonsFadeCoroutines = new List<Coroutine>();
    }

    private void ClickImage(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (button.GetComponent<CaptchaImage>().ImageName == _animalToClickName)
        {
            _allButtonsFadeCoroutines.Add(StartCoroutine(FadeAndChangeButtonImage(button)));
        }
        else
        {
            StartCoroutine(ShowFailureAndGenerateNextCaptcha());
        }
    }

    private IEnumerator ShowSuccessAndGenerateNextCaptcha()
    {
        _successImageObject.SetActive(true);
        yield return new WaitForSeconds(_popupDuration);
        _successImageObject.SetActive(false);
        CleanupCaptcha();
        _animalToClickCount = 0;
        _phoneWelcomePanelObject.SetActive(true);
        GenerateCaptcha();
    }

    private IEnumerator ShowFailureAndGenerateNextCaptcha()
    {
        _failureImageObject.SetActive(true);
        yield return new WaitForSeconds(_popupDuration);
        _failureImageObject.SetActive(false);
        CleanupCaptcha();
        _animalToClickCount = 0;
        _phoneWelcomePanelObject.SetActive(true);
        GenerateCaptcha();
    }

    private IEnumerator FadeAndChangeButtonImage(GameObject button)
    {
        button.GetComponent<Button>().onClick.RemoveAllListeners();

        // Fade out
        Color buttonColor = button.GetComponent<Image>().color;
        Color imageColor = button.transform.GetChild(0).GetComponent<Image>().color;

        for (float elapsedTime = 0f; elapsedTime < _buttonFadeDuration; elapsedTime += Time.deltaTime)
        {
            float normalizedTime = elapsedTime / _buttonFadeDuration;
            button.GetComponent<Image>().color = Color.Lerp(buttonColor, new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0), normalizedTime);
            button.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(imageColor, new Color(imageColor.r, imageColor.g, imageColor.b, 0), normalizedTime);
            yield return null;
        }

        // Change image
        int animalImageToGenerateIndex = Random.Range(0, _animalsImages.Length);
        button.GetComponent<Image>().color = GetRandomBackgroundColor();
        buttonColor = button.GetComponent<Image>().color;
        button.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0);
        button.GetComponent<CaptchaImage>().ImageName = _animalsNames[animalImageToGenerateIndex];
        button.transform.GetChild(0).GetComponent<Image>().sprite = _animalsImages[animalImageToGenerateIndex];

        // Update goal
        if (_animalsNames[animalImageToGenerateIndex] == _animalToClickName)
        {
            _animalToClickCount++;
        }

        // Check if captcha is solved
        _animalToClickCount--;

        if (_animalToClickCount == 0 && !_failureImageObject.activeSelf)
        {
            GameManager.Instance.IncreaseSolvedQuestionsCount();
            outputReponse[_nbGoodAnswers].GetComponent<TMP_Text>().alpha = 255.0f;
            _nbGoodAnswers += 1;
            src.PlayOneShot(src.clip);
            StartCoroutine(ShowSuccessAndGenerateNextCaptcha());
        }

        // Fade in
        buttonColor = button.GetComponent<Image>().color;
        imageColor = button.transform.GetChild(0).GetComponent<Image>().color;

        for (float elapsedTime = 0f; elapsedTime < _buttonFadeDuration; elapsedTime += Time.deltaTime)
        {
            float normalizedTime = elapsedTime / _buttonFadeDuration;
            button.GetComponent<Image>().color = Color.Lerp(buttonColor, new Color(buttonColor.r, buttonColor.g, buttonColor.b, 255), normalizedTime);
            button.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(imageColor, new Color(imageColor.r, imageColor.g, imageColor.b, 1), normalizedTime);
            yield return null;
        }

        button.GetComponent<Button>().onClick.AddListener(() => { ClickImage(button); });
    }

    private Color GetRandomBackgroundColor()
    {
        return _backgroundColors[Random.Range(0, _backgroundColors.Length)];
    }
}
