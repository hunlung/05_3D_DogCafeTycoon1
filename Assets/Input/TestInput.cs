//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/TestInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TestInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestInput"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""3fe2bd95-0d71-4e92-a5ff-9ab86167424b"",
            ""actions"": [
                {
                    ""name"": ""7"",
                    ""type"": ""Button"",
                    ""id"": ""a7245e5b-8796-4641-8f7e-1bfa7b78a89d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""8"",
                    ""type"": ""Button"",
                    ""id"": ""fd5bdb49-83d8-45e5-8af3-ddb43e2e4d2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""9"",
                    ""type"": ""Button"",
                    ""id"": ""4cde4993-914d-4f97-994f-4710b88f62b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""10"",
                    ""type"": ""Button"",
                    ""id"": ""05f72a32-ba6b-45b8-bdbf-55ae21c49a14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""11"",
                    ""type"": ""Button"",
                    ""id"": ""17d4167d-244a-49cd-9b02-b5e657716124"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3d863d1e-f21e-477c-b462-9217adff8933"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6f2e2eb-bd84-4e0d-b187-20e59409a1ba"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05c62798-5143-4a04-86f4-2a8b6834096d"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66a8dff7-a56e-405b-a478-be561ef120a0"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""10"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa1e9999-c963-473f-9d29-c7b9fe97839c"",
                    ""path"": ""<Keyboard>/minus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""11"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test__7 = m_Test.FindAction("7", throwIfNotFound: true);
        m_Test__8 = m_Test.FindAction("8", throwIfNotFound: true);
        m_Test__9 = m_Test.FindAction("9", throwIfNotFound: true);
        m_Test__10 = m_Test.FindAction("10", throwIfNotFound: true);
        m_Test__11 = m_Test.FindAction("11", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Test
    private readonly InputActionMap m_Test;
    private List<ITestActions> m_TestActionsCallbackInterfaces = new List<ITestActions>();
    private readonly InputAction m_Test__7;
    private readonly InputAction m_Test__8;
    private readonly InputAction m_Test__9;
    private readonly InputAction m_Test__10;
    private readonly InputAction m_Test__11;
    public struct TestActions
    {
        private @TestInput m_Wrapper;
        public TestActions(@TestInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @_7 => m_Wrapper.m_Test__7;
        public InputAction @_8 => m_Wrapper.m_Test__8;
        public InputAction @_9 => m_Wrapper.m_Test__9;
        public InputAction @_10 => m_Wrapper.m_Test__10;
        public InputAction @_11 => m_Wrapper.m_Test__11;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void AddCallbacks(ITestActions instance)
        {
            if (instance == null || m_Wrapper.m_TestActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestActionsCallbackInterfaces.Add(instance);
            @_7.started += instance.On_7;
            @_7.performed += instance.On_7;
            @_7.canceled += instance.On_7;
            @_8.started += instance.On_8;
            @_8.performed += instance.On_8;
            @_8.canceled += instance.On_8;
            @_9.started += instance.On_9;
            @_9.performed += instance.On_9;
            @_9.canceled += instance.On_9;
            @_10.started += instance.On_10;
            @_10.performed += instance.On_10;
            @_10.canceled += instance.On_10;
            @_11.started += instance.On_11;
            @_11.performed += instance.On_11;
            @_11.canceled += instance.On_11;
        }

        private void UnregisterCallbacks(ITestActions instance)
        {
            @_7.started -= instance.On_7;
            @_7.performed -= instance.On_7;
            @_7.canceled -= instance.On_7;
            @_8.started -= instance.On_8;
            @_8.performed -= instance.On_8;
            @_8.canceled -= instance.On_8;
            @_9.started -= instance.On_9;
            @_9.performed -= instance.On_9;
            @_9.canceled -= instance.On_9;
            @_10.started -= instance.On_10;
            @_10.performed -= instance.On_10;
            @_10.canceled -= instance.On_10;
            @_11.started -= instance.On_11;
            @_11.performed -= instance.On_11;
            @_11.canceled -= instance.On_11;
        }

        public void RemoveCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestActions instance)
        {
            foreach (var item in m_Wrapper.m_TestActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestActions @Test => new TestActions(this);
    public interface ITestActions
    {
        void On_7(InputAction.CallbackContext context);
        void On_8(InputAction.CallbackContext context);
        void On_9(InputAction.CallbackContext context);
        void On_10(InputAction.CallbackContext context);
        void On_11(InputAction.CallbackContext context);
    }
}
