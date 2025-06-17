using System.Threading;
using Cysharp.Threading.Tasks;
using Application.Game;
using Core.StateMachine;
using ILogger = Core.ILogger;

namespace Application.GameStateMachine
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly EasySlotGameStateController _easySlotGameStateController;
        private readonly MiddleSlotGameStateController _middleSlotGameStateController;
        private readonly HardSlotGameStateController _hardSlotGameStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            EasySlotGameStateController easySlotGameStateController,
            MiddleSlotGameStateController middleSlotGameStateController,
            HardSlotGameStateController hardSlotGameStateController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            _easySlotGameStateController = easySlotGameStateController;
            _middleSlotGameStateController = middleSlotGameStateController;
            _hardSlotGameStateController = hardSlotGameStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_menuStateController, _easySlotGameStateController, _middleSlotGameStateController, _hardSlotGameStateController);
            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}