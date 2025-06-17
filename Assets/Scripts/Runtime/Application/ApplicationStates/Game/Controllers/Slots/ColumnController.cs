using Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ColumnController : BaseController
{
    private ColumnModel _columnModel;
    private ColumnView _columnView;
    private CancellationTokenSource _spinCancellationTokenSource;

    public ColumnController(ColumnModel model, ColumnView view)
    {
        _columnModel = model;
        _columnView = view;
    }

    public override async UniTask Run(CancellationToken cancellationToken)
    {
        await base.Run(cancellationToken);

        _spinCancellationTokenSource = new CancellationTokenSource();

        await SpinAsync();
    }

    public override async UniTask Stop()
    {
        try
        {
            await base.Stop();
            await StopSpin();
            await SnapToClosestElement();
            await UniTask.CompletedTask;
        }
        catch
        {
            await UniTask.CompletedTask;
        }
    }

    private async UniTask SpinAsync()
    {
        float spinSpeed = _columnModel.Height * 3;

        while (!_spinCancellationTokenSource.Token.IsCancellationRequested)
        {
            MoveColumn(spinSpeed);
            await UniTask.Yield(_spinCancellationTokenSource.Token);
        }
    }

    private void MoveColumn(float speed)
    {
        float positionY = _columnView.GetPositionY() - speed * Time.deltaTime;

        if (positionY <= -_columnView.GetHeight())
        {
            positionY += _columnView.GetHeight();
        }

        _columnView.SetPositionY(positionY);
    }

    public async UniTask ReplaceRewards()
    {
        await _columnView.ReplaceWithRandomElements();
    }

    public async UniTask SnapToClosestElement()
    {
        float currentPosition = _columnView.GetPositionY();

        float targetPosition = -_columnView.GetHeight();

        float animationDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float newPosition = Mathf.Lerp(currentPosition, targetPosition, elapsedTime / animationDuration);
            _columnView.SetPositionY(newPosition);

            await UniTask.Yield();
        }

        _columnView.SetPositionY(targetPosition);
    }

    public async UniTask StopSpin()
    {
        _spinCancellationTokenSource?.Cancel();
        _spinCancellationTokenSource?.Dispose();
        _columnModel.IsSpinning = false;
        await UniTask.CompletedTask;
    }
}