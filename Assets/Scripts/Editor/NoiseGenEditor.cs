using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(PlanetGenerator))]
    public class NoiseGenEditor : UnityEditor.Editor
    {
        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI()
        {
            var gui = new VisualElement();
            InspectorElement.FillDefaultInspector(gui, serializedObject, this);
            gui.Add(new Button(() =>
            {
                (this.target as PlanetGenerator).Generate();
            })
            {
                text = "Generate"
            });
            gui.Add(new Button(() =>
            {
                var ng = (this.target as PlanetGenerator);
                ng.seed = (uint)(Random.value * 100000);
                ng.Generate();
            })
            {
                text = "Generate Random Seed"
            });
            return gui;
        }
    }
}