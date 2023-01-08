using Godot;

public class MainScript : Node
{
    [Export]
    public PackedScene mobScene;

    public int score;

    private Timer mobTimer;
    private Timer scoreTimer;
    private Timer startTimer;

    private HUDScript hudScript;
    private Player playerScript;

    private AudioStreamPlayer musicASP;
    private AudioStreamPlayer deathSoundASP;

    public override void _Ready()
    {
        mobTimer = GetNode<Timer>("MobTimer");
        scoreTimer = GetNode<Timer>("ScoreTimer");
        startTimer = GetNode<Timer>("StartTimer");

        hudScript = GetNode<HUDScript>("HUD");
        playerScript = GetNode<Player>("Player");

        musicASP = GetNode<AudioStreamPlayer>("Music");
        deathSoundASP = GetNode<AudioStreamPlayer>("DeathSound");
    }

    public void GameOver()
    {
        mobTimer.Stop();
        scoreTimer.Stop();
        musicASP.Stop();
        deathSoundASP.Play();
        hudScript.ShowGameOver();
    }

    public void NewGame()
    {
        score = 0;

        //уничтожить группу врагов прошлого уровня
        GetTree().CallGroup("mobsGroup", "queue_free"); 

        //поместить игрока в стартовую позицию
        Position2D position2D = GetNode<Position2D>("StartPosition");
        playerScript.Start(position2D.Position);  

        startTimer.Start();
        musicASP.Play();

        hudScript.UpdateScore(score);
        hudScript.ShowMessage("Get Ready!");
    }

    //StartTimer запускает оба других таймера ScoreTimer и MobTimer
    public void OnStartTimerTimeout() 
    {
        scoreTimer.Start();
        mobTimer.Start();
    }

    //ScoreTimer увеличивает количество очков каждую секунду
    public void OnScoreTimerTimeout()
    {
        score++;
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

        //скорректировать направление на случайный угол randomAngle от -45 до +45 градусов
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

        //фактическое появление (добавление в главную сцену)
        AddChild(mob);
    }
}