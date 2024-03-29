using Godot;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;

public partial class LoadingScreen : CanvasLayer
{
    private ProgressBar _progressBar;
    public override void _Ready()
    {
        _progressBar = GetNode<ProgressBar>("ProgressBar");
        _progressBar.Value = 0;
    }
    public void SetProgress(float progress)
    {
        _progressBar.Value = progress;
    }
}
