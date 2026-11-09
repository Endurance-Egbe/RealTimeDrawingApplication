namespace View.ViewModels.DatabaseServices
{
    public static class DrawingComponentServiceProvider
    {
        public static DrawingComponentService Current { get; private set; } =
            new DrawingComponentService(new SqliteDrawingComponentRepository());

        public static void SetCurrent(DrawingComponentService drawingComponentService)
        {
            Current = drawingComponentService;
        }
    }
}
