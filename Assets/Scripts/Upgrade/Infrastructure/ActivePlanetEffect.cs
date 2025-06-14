using UnityEngine;

public abstract class ActivePlanetEffect
{
    protected readonly Transform origin;
    private float timer;
    protected float interval;

    public ActivePlanetEffect(Transform origin, float interval)
    {
        this.origin = origin;
        this.interval = interval;
        this.timer = interval;
    }

    public void Tick(float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0f)
        {
            timer = interval;
            Trigger();
        }
    }

    protected abstract void Trigger();
}
