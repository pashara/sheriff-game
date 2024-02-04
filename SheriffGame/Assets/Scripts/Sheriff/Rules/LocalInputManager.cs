using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class LocalInputManager
{
    private LinkedList<InputActionMap> activeInputMaps = new LinkedList<InputActionMap>();
    private static LocalInputManager _instance;

    public static LocalInputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new();
            }

            return _instance;
        }
    }


    public void PushInputMap(InputActionMap inputMap)
    {
        activeInputMaps.AddLast(inputMap);
        UpdateInput();
    }

    public void PopInputMap(InputActionMap inputMap)
    {
        if (activeInputMaps.Count > 0)
        {
            var element = activeInputMaps.FindLast(inputMap);
            if (element != null)
            {
                activeInputMaps.Remove(element);
            }

            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        // Очистить все InputActionMap'ы
        foreach (var inputMap in InputSystem.ListEnabledActions())
        {
            inputMap.Disable();
        }

        // Включить только активные InputActionMap'ы (крайний в стеке)
        if (activeInputMaps.Count > 0)
        {
            var topInputMap = activeInputMaps.LastOrDefault();
            topInputMap.Enable();
        }
    }
}