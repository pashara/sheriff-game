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
        // foreach (var inputMap in InputSystem.ListEnabledActions())
        // {
        //     inputMap.Disable();
        // }


        var element = activeInputMaps.First;

        InputActionMap mapLast = null;
        while (element != null)
        {
            if (element.Value != null)
            {
                mapLast = element.Value;
                element.Value.Disable();
            }

            element = element.Next;
        }

        if (mapLast != null)
        {
            mapLast?.Enable();
        }

        
        // foreach (var inputMap in activeInputMaps)
        // {
        //     if (inputMap != null)
        //         inputMap.Disable();
        // }
        //
        // foreach (var inputMap in activeInputMaps)
        // {
        //     if (inputMap != null)
        //         inputMap.Disable();
        // }
        //
        // if (activeInputMaps.Count > 0)
        // {
        //     var topInputMap = activeInputMaps.LastOrDefault();
        //     topInputMap.Enable();
        // }
        //
        // // Включить только активные InputActionMap'ы (крайний в стеке)
        // if (activeInputMaps.Count > 0)
        // {
        //     var topInputMap = activeInputMaps.LastOrDefault();
        //     topInputMap.Enable();
        // }
    }
}