using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundSceneLoading
{
    public partial class Main: Node2D
    {
        private Button _changeSceneButton;
        private Button _changeSceneButton1;
        public override void _Ready()
        {
            _changeSceneButton = GetNode<Button>("Button");
            _changeSceneButton1 = GetNode<Button>("Button1");
            _changeSceneButton.Pressed += ChangeSceneButton_Pressed;
            _changeSceneButton1.Pressed += ChangeSceneButton1_Pressed;
        }

        private void ChangeSceneButton1_Pressed()
        {
            SceneManager.Instance.GoToSceneNoLoadingScreen("res://Scenes/level_1.tscn");
        }

        public override void _ExitTree()
        {
            _changeSceneButton.Pressed -= ChangeSceneButton_Pressed;
            _changeSceneButton1.Pressed -= ChangeSceneButton1_Pressed;
        }
        private void ChangeSceneButton_Pressed()
        {
            SceneManager.Instance.GoToSceneThreadedWithLoadingScreen("res://Scenes/level_1.tscn");
        }
    }
}
