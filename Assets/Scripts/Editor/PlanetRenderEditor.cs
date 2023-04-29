using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(PlanetRender))]
    public class PlanetRenderEditor : UnityEditor.Editor
    {
        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI()
        {
            var gui = new VisualElement();
            InspectorElement.FillDefaultInspector(gui, serializedObject, this);
            gui.Add(new Button(() =>
            {
                (this.target as PlanetRender).Generate();
            })
            {
                text = "Generate"
            });
            return gui;
        }
    }
}