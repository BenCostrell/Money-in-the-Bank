using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTask : Task {

    private Player player;
    private float duration;
    private float timeElapsed;

    public StunTask(float dur, Player pl)
    {
        duration = dur;
        player = pl;
    }

    protected override void Init()
    {
        timeElapsed = 0;
        player.actionable = false;
        Services.EventManager.Register<GameOver>(OnGameOver);
    }

    internal override void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= duration)
        {
            SetStatus(TaskStatus.Success);
        }
    }

    void OnGameOver(GameOver e)
    {
        SetStatus(TaskStatus.Aborted);
    }

    protected override void OnSuccess()
    {
        player.actionable = true;
    }

    protected override void CleanUp()
    {
        Services.EventManager.Unregister<GameOver>(OnGameOver);
    }
}
