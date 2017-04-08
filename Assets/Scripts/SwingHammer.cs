using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHammer : Task {
    private float swingDuration;
    private float activeAfterThisTime;
    private float recoveryDuration;
    private Player player;
    private float timeElapsed;

    public SwingHammer(float swingDur, float recoveryDur, float activeFramesStart, Player pl)
    {
        swingDuration = swingDur;
        recoveryDuration = recoveryDur;
        activeAfterThisTime = activeFramesStart;
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

        float targetRotation = -90 * player.transform.localScale.x;

        if (timeElapsed < activeAfterThisTime || timeElapsed > swingDuration)
        {
            player.hammerCollider.enabled = false;
        }
        else
        {
            player.hammerCollider.enabled = true;
        }

        if (timeElapsed <= swingDuration)
        {
            timeElapsed = Mathf.Min(timeElapsed, swingDuration);
            player.hammerPivot.rotation = Quaternion.Euler(Vector3.LerpUnclamped(Vector3.zero, new Vector3(0, 0, targetRotation),
                Easing.BackEaseIn(timeElapsed / swingDuration)));
        }
        else
        {
            player.hammerPivot.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, targetRotation), Vector3.zero,
                Easing.QuadEaseOut((timeElapsed - swingDuration) / recoveryDuration)));
        }

        if (timeElapsed >= (swingDuration + recoveryDuration))
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
