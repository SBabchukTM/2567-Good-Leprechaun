using Application.Services.UserData;
using Application.UI;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;

public class HardSlotGameStateController : BaseSlotGameStateController
{
    private readonly IUiService _uiService;
    private readonly IAudioService _audioService;
    private readonly UserDataService _userDataService;

    public HardSlotGameStateController(ILogger logger,
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
        _columnCount = 5;
        return base.Initialization();
    }

    protected override void RestartLevel()
    {
        GoTo<HardSlotGameStateController>();
    }
}