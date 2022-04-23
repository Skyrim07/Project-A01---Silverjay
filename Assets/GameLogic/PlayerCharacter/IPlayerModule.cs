using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerModule
{
    void SetState(bool isActive);
    void Initialize();
    void Tick(float unscaledDeltaTime, float deltaTime);
}
