using BackgroundSceneLoading;
using Godot;
using System;

public partial class Level2 : Node2D
{
    private Button _changeSceneButton;
    public override void _Ready()
    {
        _changeSceneButton = GetNode<Button>("Button");
        _changeSceneButton.Pressed += ChangeSceneButton_Pressed;
    }
    public override void _ExitTree()
    {
        _changeSceneButton.Pressed -= ChangeSceneButton_Pressed;
    }
    private void ChangeSceneButton_Pressed()
    {
        SceneManager.Instance.GoToSceneThreadedWithLoadingScreen("res://Scenes/main.tscn");
    }
}
