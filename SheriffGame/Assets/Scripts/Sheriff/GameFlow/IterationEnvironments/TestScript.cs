using UnityEngine;

namespace Sheriff.GameFlow.IterationEnvironments
{
    public class Ee
    {
        public string A;

        public Ee(string a)
        {
            A = a;
        }
    }
    public class TestScript
    {
        public TestScript(Ee data)
        {
            Debug.Log($"TestScript `{data.A}`");
        }
    }
}