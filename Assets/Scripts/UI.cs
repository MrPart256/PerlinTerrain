using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI : MonoBehaviour
{
    [SerializeField] private MeshGenerator _meshGenerator;
    [SerializeField] private Slider _subdivisionsSlider;
    [SerializeField] private Toggle _enableWireframeToggle;
    [SerializeField] private Toggle _enableTextureToggle;
    [SerializeField] private Slider _noiseScaleSlider;
    [SerializeField] private Slider _octaves;
    [SerializeField] private Slider _persistance;
    [SerializeField] private Slider _lacunarity;
    [SerializeField] private TMP_Dropdown _dropdown;
    
    private MathFunction[] _functions = new MathFunction[]
    {
        new EmptyFunction(),
        new SinFunction(),
        new SinFunctionDivided(4),
        new SinFunctionDivided(16)
    };
    
    private void Start()
    {
        _subdivisionsSlider.onValueChanged.AddListener((aga) => UpdateMesh());
        _noiseScaleSlider.onValueChanged.AddListener((aga)=> UpdateMesh());
        _octaves.onValueChanged.AddListener((aga)=> UpdateMesh());
        _persistance.onValueChanged.AddListener((aga)=> UpdateMesh());
        _lacunarity.onValueChanged.AddListener((aga)=> UpdateMesh());
        _dropdown.onValueChanged.AddListener((aga)=> UpdateMesh());
        _enableWireframeToggle.onValueChanged.AddListener(EnableWireframe);
        _enableTextureToggle.onValueChanged.AddListener(EnableTextures);
    }

    private void UpdateMesh()
    {
        _meshGenerator.UpdateMesh((int)_subdivisionsSlider.value, _noiseScaleSlider.value, (int)_octaves.value, _persistance.value,_lacunarity.value, _functions[_dropdown.value]);
    }

    private void EnableWireframe(bool state)
    {
        _meshGenerator.EnableWireframe(state);
    }

    private void EnableTextures(bool state)
    {
        _meshGenerator.EnableTextures(state);
    }
}