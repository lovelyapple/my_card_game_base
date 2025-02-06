using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeController : MonoSingletonBase<InitializeController>
{
    private List<GameModel> _gameModels = new List<GameModel>
    {
        new GameModel(),
    };
    public override void OnAwake()
    {
        foreach(var model in _gameModels)
        {
            model.SetInsance(model);
        }
    }
}
