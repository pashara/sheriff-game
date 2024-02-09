using System.Collections.Generic;
using System.Linq;
using Sheriff.Rules;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalInputManager
{
    private LinkedList<(InputActionMap, InputSender)> activeInputMaps = new LinkedList<(InputActionMap, InputSender)>();
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


    public void PushInputMap(InputActionMap inputMap, InputSender inputSender)
    {
        activeInputMaps.AddLast((inputMap, inputSender));
        UpdateInput();
    }

    public void PopInputMap(InputActionMap inputMap, InputSender inputSender)
    {
        if (activeInputMaps.Count > 0)
        {
            var element = activeInputMaps.FindLast((inputMap, inputSender));
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

        (InputActionMap, InputSender) mapLast = default;
        while (element != null)
        {
            if (element.Value.Item1 != null)
            {
                mapLast = element.Value;
                element.Value.Item1.Disable();
            }

            element = element.Next;
        }

        if (mapLast.Item1 != null)
        {
            mapLast.Item1?.Enable();
            
            Cursor.visible = !(mapLast.Item2 != null && mapLast.Item2.HideInput);
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
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