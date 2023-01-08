using Godot;
using System;

public class MainScript : Node
{
    [Export]
    public PackedScene mobScene;

    public int score;

    private Player player;
    private Position2D position2D;

    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<AudioStreamPlayer>("Music").Stop();
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        HUDScript hudScript = GetNode<HUDScript>("HUD");
        hudScript.ShowGameOver();
    }

    public void NewGame()
    {
        GetTree().CallGroup("mobsGroup", "queue_free");

        score = 0;

        player = GetNode<Player>("Player");
        position2D = GetNode<Position2D>("StartPosition");
        player.Start(position2D.Position);

        GetNode<Timer>("StartTimer").Start();
        GetNode<AudioStreamPlayer>("Music").Play();

        HUDScript hudScript = GetNode<HUDScript>("HUD");
        hudScript.UpdateScore(score);
        hudScript.ShowMessage("Get Ready!");
    }

    //StartTimer запускает оба других таймера ScoreTimer и MobTimer
    public void OnStartTimerTimeout() 
    {
        GetNode<Timer>("ScoreTimer").Start();
        GetNode<Timer>("MobTimer").Start();
    }

    //ScoreTimer увеличивает количество очков каждую секунду
    public void OnScoreTimerTimeout()
    {
        score++;
        HUDScript hudScript = GetNode<HUDScript>("HUD");
        hudScript.UpdateScore(score);
    }

    //MobTimer создает врага каждые 2 секунды в случайном месте вдоль установленного Path2D
    public void OnMobTimerTimeout()
    {
        //выбор случайного места на Path2D
        PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = GD.Randi();

        //установить направление перпендикулярно Path2D
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        //скорректировать направление на случайный угол randomAngle от +45 до +45 градусов
        float randomAngle = (float) GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        direction += randomAngle;

        //выбрать скорость в диапазоне от 150 до 250 по абсцисс (скорость это вектор)
        Vector2 velocity = new Vector2((float)GD.RandRange(150, 250),0);

        //создать новую сущность (экземпляр) врага (сцены Mob)
        Mob mob = (Mob)mobScene.Instance();

        //присвоить значания позиции, поворота и скорости
        mob.Position = mobSpawnLocation.Position;
        mob.Rotation = direction;
        mob.LinearVelocity = velocity.Rotated(direction);

        //фактический спавн (добавление в главную сцену)
        AddChild(mob);
    }
}
