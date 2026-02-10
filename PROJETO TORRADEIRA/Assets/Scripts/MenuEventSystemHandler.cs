using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuEventSystemHandler : MonoBehaviour
{
    [Header("References")]
    public List<Selectable> Selectables = new List<Selectable>();

    [Header("Controls")]
    [SerializeField] protected InputActionReference _navigationReference;

    [Header("Animations")]
    [SerializeField] protected bool AnimateSelection = true;
    [SerializeField] protected float _selectAnimationScale = 1.1f;
    [SerializeField] protected float _scaleDuration = 0.25f;
    [SerializeField] protected Selectable _firstSelected;

    [Header("Colors")]
    [SerializeField] protected Color _selectedColor = Color.yellow;
    [SerializeField] protected Color _deselectedColor = Color.white;

    [Header("Scene Settings")]
    [Tooltip("Nome da cena a ser carregada ao clicar no botão Play")]
    [SerializeField] private string playSceneName = "";

    [Tooltip("Nome da cena a ser carregada ao clicar no botão Options")]
    [SerializeField] private string optionsSceneName = "";

    [Tooltip("Nome da cena a ser carregada ao clicar no botão Credits")]
    [SerializeField] private string creditsSceneName = "";

    [Tooltip("Nome da cena a ser carregada ao clicar no botão Quit (opcional para fechar o jogo)")]
    [SerializeField] private string quitSceneName = "";

    protected Dictionary<Selectable, Vector3> _scales = new Dictionary<Selectable, Vector3>();
    protected Dictionary<Selectable, Graphic> _texts = new Dictionary<Selectable, Graphic>();

    protected Selectable _lastSelected;
    protected Tween _scaleUpTween;
    protected Tween _scaleDownTween;
    protected bool _skipNextSelectSound;

    private void Awake()
    {
        foreach (var selectable in Selectables)
        {
            AddSelectionListeners(selectable);
            _scales[selectable] = selectable.transform.localScale;

            Graphic textComp = selectable.GetComponentInChildren<TMP_Text>();
            if (textComp == null) textComp = selectable.GetComponentInChildren<Text>();

            if (textComp != null)
            {
                _texts[selectable] = textComp;
                textComp.color = _deselectedColor;
            }
        }
    }

    private void OnEnable()
    {
        _navigationReference.action.Enable();
        _navigationReference.action.performed += OnNavigate;

        foreach (var selectable in Selectables)
        {
            selectable.transform.localScale = _scales[selectable];
            if (_texts.ContainsKey(selectable)) _texts[selectable].color = _deselectedColor;
        }

        _skipNextSelectSound = true;
        StartCoroutine(SelectFirstAfterFrame());
    }

    private IEnumerator SelectFirstAfterFrame()
    {
        yield return null;

        if (_firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
            _lastSelected = _firstSelected;
        }
    }

    private void OnDisable()
    {
        _navigationReference.action.performed -= OnNavigate;
        _navigationReference.action.Disable();

        _scaleUpTween?.Kill(true);
        _scaleDownTween?.Kill(true);
    }

    private void AddSelectionListeners(Selectable selectable)
    {
        EventTrigger trigger = selectable.GetComponent<EventTrigger>();
        if (trigger == null) trigger = selectable.gameObject.AddComponent<EventTrigger>();

        var selectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(selectEntry);

        var deselectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
        deselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(deselectEntry);

        var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener(data => OnPointerEnter(selectable));
        trigger.triggers.Add(pointerEnter);

        var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        pointerExit.callback.AddListener(data => OnPointerExit(selectable));
        trigger.triggers.Add(pointerExit);

        Button btn = selectable.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => OnButtonClicked(selectable));
        }
    }

    private void OnSelect(BaseEventData eventData)
    {
        _lastSelected = eventData.selectedObject.GetComponent<Selectable>();
        if (_lastSelected == null) return;

        if (AnimateSelection)
        {
            _scaleUpTween?.Kill(true);
            _scaleUpTween = _lastSelected.transform.DOScale(_scales[_lastSelected] * _selectAnimationScale, _scaleDuration);
        }

        if (_texts.ContainsKey(_lastSelected))
        {
            if (AnimateSelection)
                _texts[_lastSelected].DOColor(_selectedColor, 0.2f);
            else
                _texts[_lastSelected].color = _selectedColor;
        }

        _skipNextSelectSound = false;
    }

    private void OnDeselect(BaseEventData eventData)
    {
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        if (sel == null) return;

        if (AnimateSelection)
        {
            _scaleDownTween?.Kill(true);
            _scaleDownTween = sel.transform.DOScale(_scales[sel], _scaleDuration);
        }
        else
        {
            sel.transform.localScale = _scales[sel];
        }

        if (_texts.ContainsKey(sel))
        {
            if (AnimateSelection)
                _texts[sel].DOColor(_deselectedColor, 0.2f);
            else
                _texts[sel].color = _deselectedColor;
        }
    }

    private void OnPointerEnter(Selectable selectable)
    {
        if (selectable == null) return;

        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }

    private void OnPointerExit(Selectable selectable)
    {
        // Não altera o currentSelected para evitar sumir o botão
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
        }
    }

    private void OnButtonClicked(Selectable selectable)
    {
        string name = selectable.gameObject.name.ToLower();

        if (name.Contains("play") && !string.IsNullOrEmpty(playSceneName))
        {
            LoadScene(playSceneName);
        }
        else if (name.Contains("options") && !string.IsNullOrEmpty(optionsSceneName))
        {
            LoadScene(optionsSceneName);
        }
        else if (name.Contains("credits") && !string.IsNullOrEmpty(creditsSceneName))
        {
            LoadScene(creditsSceneName);
        }
        else if (name.Contains("quit"))
        {
            QuitGame();
        }
    }

    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogWarning("Nome da cena vazio para carregar.");
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
