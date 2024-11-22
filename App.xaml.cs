namespace Un2TrekApp
{
    public partial class App : Application
    {
        public static string StorageUserInfoKey = "StorageUserInfoKey";

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
