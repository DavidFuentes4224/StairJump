using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ColorManager;

public class MainMenuUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject additionalOptions = null;
    [SerializeField] private TextMeshProUGUI textMesh = null;
    [SerializeField] private AvatarRenderer avatar= null;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private GameObject m_saveIcon = null;
    [SerializeField] private GameObject m_mutedIcon = null;
    [SerializeField] private GameObject m_menuSound = null;

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        SetAdditionalOptions(false);
        ColorManager.ColorSelected += OnColorSelected;
        SetMuteIcons(SaveManager.Instance.IsMuted());
    }

    private void OnDestroy()
    {
        ColorManager.ColorSelected -= OnColorSelected;
    }

    private void OnColorSelected(ColorSelectedArgs e)
    {
        SetSaved(false);
    }

    public void SetSaved(bool value)
    {
        m_saveIcon.SetActive(value);
    }

    public void SetAdditionalOptions(bool enable)
    {
        additionalOptions.SetActive(enable);
    }

    public void UpdateSelectedSpriteNumber()
    {
        var number = avatar.GetSelectedSpriteNumber();
        textMesh.text = number.ToString();
    }

    public void OpenCharacterCreator()
    {
        m_animator.SetBool("OpenOptions", true);
    }

    public void CloseCharacterCreator()
    {
        m_animator.SetBool("OpenOptions", false);
    }

    public void ToggleSound()
    {
        var active = SaveManager.Instance.ToggleMute();
        SetMuteIcons(active);
    }

    private void SetMuteIcons(bool active)
    {
        m_mutedIcon.SetActive(active);
        m_menuSound.SetActive(!active);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }    
}
