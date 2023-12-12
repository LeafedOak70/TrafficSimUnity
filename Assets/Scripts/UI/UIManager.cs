using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_InputField widthInputField;
    public Button generateMapButton;

    private void Start()
    {
        generateMapButton.onClick.AddListener(GenerateMap);
    }

    private void GenerateMap()
    {
        int width = int.Parse(widthInputField.text);
        int height = width;

        // Assuming the Controller script is still present, you can access its methods
        Controller controller = FindObjectOfType<Controller>();

        if (controller != null)
        {
            controller.startMap(width, height);
        }
        controller.changeCamera();

        // Handle other actions or switch to a GameManager if needed
    }
}
