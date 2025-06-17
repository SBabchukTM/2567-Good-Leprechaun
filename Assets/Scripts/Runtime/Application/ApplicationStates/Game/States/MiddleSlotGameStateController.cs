using Application.Services.UserData;
using Application.UI;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using Core;

public class MiddleSlotGameStateController : BaseSlotGameStateController
{
    private readonly IUiService _uiService;
    private readonly IAudioService _audioService;
    private readonly UserDataService _userDataService;

    public MiddleSlotGameStateController(ILogger logger,
     IUiService uiService,
     IAudioService audioService,
     UserDataService userDataService
    ) : base(logger, uiService, audioService, userDataService)
    {
        _uiService = uiService;
        _audioService = audioService;
        _userDataService = userDataService;
    }

    protected override UniTask Initialization()
    {
        _columnCount = 4;
        return base.Initialization();
    }

    protected override void RestartLevel()
    {
        GoTo<MiddleSlotGameStateController>();
    }

}