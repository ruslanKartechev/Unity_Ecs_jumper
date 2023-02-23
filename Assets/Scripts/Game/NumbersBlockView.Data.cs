using TMPro;

namespace Game
{
    public partial class NumbersBlockView
    {
        [System.Serializable]
        private class Data
        {
            public Data(TextMeshPro text, float value)
            {
                Text = text;
                Value = value;
            }

            public TextMeshPro Text;
            public float Value;
        }
    }
}