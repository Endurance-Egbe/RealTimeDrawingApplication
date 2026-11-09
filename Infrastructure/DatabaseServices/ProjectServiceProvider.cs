namespace View.ViewModels.DatabaseServices
{
    public static class ProjectServiceProvider
    {
        public static ProjectService Current { get; private set; } =
            new ProjectService(new SqliteProjectRepository());

        public static void SetCurrent(ProjectService service)
        {
            Current = service;
        }
    }
}
