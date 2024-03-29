using Godot;
using Godot.Collections;
using System.Linq;
namespace BackgroundSceneLoading
{
    public partial class SceneManager : Node
    {
        public static SceneManager Instance { get; private set; }
        private string _sceneToLoad = "";
        private double _time = 0;
        private double _minimumWaitTime = 2f;
        private bool _useThread = false;
        private string _loadingScreenScenePath = "res://Scenes/loading_screen.tscn";
        private LoadingScreen _loadingScreen;
        private Array _progressArray;
        public override void _Ready()
        {
            if (Instance == null)
            {
                Instance = this;
                _loadingScreen = ResourceLoader.Load<PackedScene>(_loadingScreenScenePath).Instantiate<LoadingScreen>();
            }
            else
            {
                QueueFree();
            }
        }
        /// <summary>
        /// Go to scene without loading screen and without using threads
        /// </summary>
        /// <param name="scenePath"></param>
        public void GoToSceneNoLoadingScreen(string scenePath)
        {
            var packedScene = ResourceLoader.Load<PackedScene>(scenePath);
            var scene = packedScene.Instantiate();
            var root = GetTree().Root;
            // get current scene and remove it
            var currentScene = root.GetChild(root.GetChildCount() - 1);
            currentScene.QueueFree();
            root.AddChild(scene);
        }
        /// <summary>
        /// Go to scene with loading screen and using threads
        /// </summary>
        /// <param name="scenePath"></param>
        /// <param name="minimumWaitTime"></param>
        /// <param name="useSubThreads"></param>
        public void GoToSceneThreadedWithLoadingScreen(string scenePath, double minimumWaitTime = 2f, bool useSubThreads = false)
        {
            AddChild(_loadingScreen);
            _progressArray = new Array();
            _useThread = true;
            _minimumWaitTime = minimumWaitTime;
            _sceneToLoad = scenePath;
            // currently retun value is discarded 
            // but if you need to log more information, you can use the return value
            _ = ResourceLoader.LoadThreadedRequest(path: scenePath, useSubThreads: useSubThreads);
        }
        public override void _Process(double delta)
        {

            if (_useThread)
            {
                _time += delta;
                
                if (ResourceLoader.LoadThreadedGetStatus(_sceneToLoad, _progressArray) == ResourceLoader.ThreadLoadStatus.InProgress)
                {
                    _loadingScreen.SetProgress(_progressArray.First().AsSingle() * 100);
                }
                else if (ResourceLoader.LoadThreadedGetStatus(_sceneToLoad,_progressArray) == ResourceLoader.ThreadLoadStatus.Loaded && _time > _minimumWaitTime)
                {
                    // You can also use CallDeferred method to run at idle time
                    //CallDeferred(MethodName.LoadScene);
                    LoadScene();
                }
                // or use dummy % for loading screen
                if (_time < _minimumWaitTime)
                {
                    _loadingScreen.SetProgress((float)(_time / _minimumWaitTime) * 100);
                }
            }
        }
        private void LoadScene()
        {
            if (ResourceLoader.LoadThreadedGet(_sceneToLoad) is PackedScene packedScene)
            {
                var scene = packedScene.Instantiate();
                var root = GetTree().Root;
                // remove current scene
                var currentScene = root.GetChild(root.GetChildCount() - 1);
                currentScene.QueueFree();
                root.AddChild(scene);
            }
            RemoveChild(_loadingScreen);
            ResetParam();
        }
        public override void _ExitTree()
        {
            // if you add node manually, make sure to free it
            _loadingScreen?.QueueFree();
        }
        private void ResetParam()
        {
            _time = 0;
            _useThread = false;
        }
    }
}
