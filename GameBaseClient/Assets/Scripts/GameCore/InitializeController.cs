using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeController : MonoSingletonBase<InitializeController>
{
    private enum State
    {
        None,
        RequestGo,
        Going,
    }
    private State _state = State.None;
    private List<GameModel> _gameModels = new List<GameModel>
    {
        new GameModel(),
    };
    public override void OnAwake()
    {
        foreach (var model in _gameModels)
        {
            model.SetInsance(model);
        }

        var sceneTransit = new SceneTransit();
        sceneTransit.SetInsance(sceneTransit);

        _state = State.RequestGo;
    }
    public void Update()
    {
        if (_state == State.RequestGo)
        {
            SceneTransit.GotoScene("Title");
            _state = State.Going;
        }
    }
}
