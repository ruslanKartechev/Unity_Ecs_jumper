namespace Game
{
    public interface INumberBlockView
    {
        void Show(bool animated);
        void Hide(bool animated);
        void Highlight(float height);
    }
}