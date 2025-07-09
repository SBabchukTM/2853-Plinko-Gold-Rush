using System.Threading;
using Core.StateMachine;
using Application.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Application.Services.UserData;
using System;

namespace Application.Game
{
    public class AccountStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly StartSettingsController _startSettingsController;

        private AccountScreen _screen;

        private UserProfileData _newData;

        public AccountStateController(ILogger logger, IUiService uiService, UserDataService userDataService, StartSettingsController startSettingsController) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _startSettingsController = startSettingsController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CopyData();
            CreateScreen();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _screen.OnNameChanged -= ValidateName;
            _screen.OnAgeChanged -= ValidateAge;
            _screen.OnGenderChanged -= ValidateGender;

            await _uiService.HideScreen(ConstScreens.AccountScreen);
        }

        private void CopyData()
        {
            _newData = new();
            var data = _userDataService.GetUserData().UserProfileData;

            _newData.Username = data.Username;
            _newData.Gender = data.Gender;
            _newData.Age = data.Age;
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<AccountScreen>(ConstScreens.AccountScreen);
            _screen.Initialize(_userDataService.GetUserData().UserProfileData);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnSavePressed += async () =>
            {
                SaveProfile();
                await GoTo<MenuStateController>();
            };

            _screen.OnNameChanged += ValidateName;
            _screen.OnAgeChanged += ValidateAge;
            _screen.OnGenderChanged += ValidateGender;
        }

        private void SaveProfile()
        {
            var data = _userDataService.GetUserData().UserProfileData;
            data.Username = _newData.Username;
            data.Age = _newData.Age;
            data.Gender = _newData.Gender;

            _userDataService.SaveUserData();
        }

        private void ValidateGender(string value)
        {
            if(value.Length < 2 || Char.IsDigit(value[0]))
            {
                _screen.SetData(_newData);
                return;
            }

            _newData.Gender = value;
        }

        private void ValidateAge(string value)
        {
            if (!int.TryParse(value, out int age))
            {
                _screen.SetData(_newData);
                return;
            }
            
            if(age < 18 || age > 99)
            {
                _screen.SetData(_newData);
                return;
            }

            _newData.Age = age;
        }

        private void ValidateName(string value)
        {
            if (value.Length < 2 || Char.IsDigit(value[0]))
            {
                _screen.SetData(_newData);
                return;
            }

            _newData.Username = value;
        }
    }
}