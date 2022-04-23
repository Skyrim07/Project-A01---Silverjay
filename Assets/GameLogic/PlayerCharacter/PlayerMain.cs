using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class PlayerMain : MonoBehaviour, IPlayerModule
{
    public bool active = true;
    public void Initialize()
    {

    }
    public void SetState(bool active)
    {
        this.active = active;
    }
    public void Tick(float unscaledDeltaTime, float deltaTime)
    {
        
    }

    public void TeleportToSpawnPoint(int spawnPoint)
    {
        Transform tf = spawnPoint>=SceneController.instance.spawnPoints.Count? SceneController.instance.spawnPoints[0] : SceneController.instance.spawnPoints[spawnPoint];
        CommonReference.instance.playerModel.PlayerMovement.SetOrientation(tf.localScale.x<0?false:true);
        Teleport(tf.position);
    }
    public void TeleportToCheckPoint(int checkPoint)
    {
        Transform tf = SceneController.instance.checkPoints[checkPoint];
        CommonReference.instance.playerModel.PlayerMovement.SetOrientation(tf.localScale.x < 0 ? false : true);
        Teleport(tf.position);
    }
    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }

}
